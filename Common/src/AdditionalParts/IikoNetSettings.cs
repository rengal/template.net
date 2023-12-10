using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Настройки подключения к iikoNet.
    /// </summary>
    public partial class IikoNetSettings : IIikoNetLocalSettings
    {
        /// <summary>
        /// Возвращает локальные настройки текущего подразделения на RMS,
        /// либо "настройки по-умолчанию для типа оплаты", если мы находимся на чейне.
        /// </summary>
        public IIikoNetLocalSettings ForCurrentDepartment
        {
            get
            {
                return ServerInstance.INSTANCE.CurrentNode.Chain
                    ? (IIikoNetLocalSettings)this
                    : departmentSpecific.GetOrAdd(
                        ServerInstance.INSTANCE.CurrentNode.RmsDepartment,
                        entity => new IikoNetDepartmentSettings(this));
            }
        }
    }
}
