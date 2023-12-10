using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class OlapReportSetting
    {
        public OlapReportSetting(string name)
            : this(Guid.NewGuid(), new LocalizableValue(name), null)
        {
        }

        /// <summary>
        /// Получает признак места последнего изменения настроек. 
        /// </summary>
        /// <value><c>true</c> - настройки изменены на этом сервере, <c>false</c> - в другом случае.</value>
        public bool IsLocal
        {
            get { return LastModifyNode == null || LastModifyNode.Value == ServerInstance.INSTANCE.CurrentNode.Id; }
        }

        /// <summary>
        /// <c>true</c>, если данный пресет доступен для редактирования
        /// текущему пользователю
        /// </summary>
        public bool IsEditable
        {
            get
            {
                // Если пресет был изменён на другом сервере, то редактировать его нельзя
                // (это обычно бывает, когда пресет среплицирован с чейна на подключенный
                // к нему РМС - такие пресеты на РМС'ах недоступны для редактирования).
                if (!IsLocal)
                {
                    return false;
                }

                // Пользователь с правом CAN_EDIT_SHARED_OLAP_REPORTS может редактировать все пресеты
                // (как общие, так и личные); пользователь без этого права - только личные.
                return IsDefault ||
                       (!isShared && Equals(owner, ServerSession.CurrentSession.GetCurrentUser())) ||
                       Permission.CAN_EDIT_SHARED_OLAP_REPORTS.IsAllowedForCurrentUser;
            }
        }

        /// <summary>
        /// Признак того, что данный пресет является виртуальным пресетом по умолчанию
        /// (обычно это первый пункт в выпадающем списке пресетов: "ОЛАП-отчёт по продажам", "ОЛАП-отчёт по проводкам" и т.п.).
        /// </summary>
        /// <remarks>Оставляю как поле, а не как свойство. Автосвойством это оформить не получится,
        /// т.к. будут проблемы при десериализации, а если делать обычным свойством, то поле всё равно придётся объявлять</remarks>
        [Transient]
        public bool IsDefault;

        /// <summary>
        /// Возвращает множество доступных текущему пользователю пресетов указанного типа
        /// </summary>
        /// <param name="reportType">Тип отчётов</param>
        public static IEnumerable<OlapReportSetting> GetAllAvailablePresets(OlapReports reportType)
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<OlapReportSetting>(
                olapReportSetting =>
                    olapReportSetting.ReportType == reportType &&
                    !olapReportSetting.NameLocal.IsNullOrWhiteSpace() &&
                    (olapReportSetting.IsShared ||
                     olapReportSetting.Owner == null ||
                     Equals(olapReportSetting.Owner, ServerSession.CurrentSession.GetCurrentUser())))
                .OrderBy(olapReportSetting => olapReportSetting.NameLocal, new StringNumbersComparer());
        }

        public override string ToString()
        {
            return NameLocal;
        }
    }
}