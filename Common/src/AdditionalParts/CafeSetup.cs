using System;
using System.Collections.Generic;
using Resto.Common;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.UI.Common;

namespace Resto.Data
{
    public partial class CafeSetup : IWithOperationalDaySettings
    {
        public static CafeSetup INSTANCE
        {
            get
            {
                CafeSetup setup = EntityManager.INSTANCE.GetSingleton<CafeSetup>();
                if (setup == null)
                {
                    throw new RestoException("No CafeSetup object found");
                }
                return setup;
            }
        }

        public static bool IsCafeSetupLoaded
        {
            get
            {
                IList<CafeSetup> setups = EntityManager.INSTANCE.GetAll<CafeSetup>();
                return setups.Count == 1;
            }
        }

        //Определение текущего времени документов из настроек для RMS
        public DateTime GetDocumentDefaultTime(DocumentType docType)
        {
            DateTime docTime = DateTime.Now;
            if (DocSettings != null && DocSettings.DefaultDocumentTimes.ContainsKey(docType))
            {
                DefaultDocumentTime defTime = DocSettings.DefaultDocumentTimes[docType];
                if (defTime.TimeType != DocumentTimeType.CURRENT_TIME)
                {
                    docTime = (new DateTime(docTime.Year, docTime.Month, docTime.Day)).AddMinutes(defTime.DocumentTime.Minutes);
                }
            }
            return docTime;
        }

        private int? nomenclatureUpdateInterval;

        /// <summary>
        /// Задает период обновления данных номенклатуры (в милисекундах).
        /// </summary>
        public int? NomenclatureUpdateInterval
        {
            get
            {
                if (nomenclatureUpdateInterval == null)
                {
                    var call = ServiceClientFactory.ProductsService.GetNomenclatureUpdateInterval();
                    try
                    {
                        nomenclatureUpdateInterval = call.CallSync();
                    }
                    catch { }
                }

                return nomenclatureUpdateInterval;
            }
        }

        /// <summary>
        /// Содержит время начала и окончания учетного дня в минутах.
        /// Для изменения настроек учетного дня использовать это свойство
        /// вместо <see cref="BusinessDateSettings"/> и <see cref="OperationalDaySettings"/>
        /// т.к. должен обновляться прейскурант.
        /// </summary>
        /// <remarks>
        /// Обновляется на РМС при сохранении CafeSetup.
        /// На стороне Чейна не редактируются.
        /// На РМС должны совпадать с настройкой из CafeSetup.
        /// Не получилось полностью вынести из CafeSetup в DepartmentEntity
        /// и Corporation, т.к., неизвестно в каких отчетах какую дату использовать
        /// чтобы не было расхождений с чейном.
        /// </remarks>
        public Pair<int, int> OperationalDayTime
        {
            get => new Pair<int, int>(BusinessDateSettings.DayStartTime.Minutes, OperationalDaySettings.DayCloseTime.Minutes);
            set
            {
                var prevDayStart = BusinessDateSettings.DayStartTime.Minutes;
                var prevDayClose = OperationalDaySettings.DayCloseTime.Minutes;
                if (prevDayStart == value.First && prevDayClose == value.Second)
                {
                    return;
                }

                BusinessDateSettings.DayStartTime = new DayTime(value.First);
                OperationalDaySettings.DayCloseTime = new DayTime(value.Second);
                // if (CompanySetup.IsRMS)
                // {
                //     PriceListItemRepositoryInstance.INSTANCE.UpdateNow();
                // } //todo debugnow
            }
        }
    }
}