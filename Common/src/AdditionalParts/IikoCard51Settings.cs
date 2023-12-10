using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Data
{
    public sealed partial class IikoCard51Settings
    {
        //чтобы не плодить при каждом обращении новый объект в куче под выброс, кешируем один раз пустые настройки.
        private static readonly IikoCard51DepartmentSettings EmptySettings = new IikoCard51DepartmentSettings();

        /// <summary>
        /// Возвращает локальные настройки текущего подразделения на RMS.
        /// Для фронта.
        /// </summary>
        public IikoCard51DepartmentSettings ForCurrentDepartment
        {
            get
            {
                if (ServerInstance.INSTANCE.CurrentNode.Chain)
                    throw new InvalidOperationException("Chain can not have department settings.");
                IikoCard51DepartmentSettings departmentSettings;
                return departmentSpecific.TryGetValue(ServerInstance.INSTANCE.CurrentNode.RmsDepartment,
                    out departmentSettings)
                    ? departmentSettings
                    : EmptySettings;
            }
        }

        /// <summary>
        /// Для фронта.
        /// </summary>
        [NotNull]
        public string Password
        {
            get { return ForCurrentDepartment.Password; }
        }

        /// <summary>
        /// Для фронта.
        /// </summary>
        [NotNull]
        public string PosServerAddress
        {
            get { return ForCurrentDepartment.PosServerAddress; }
        }

        public ICollection<IikoCard51MarketingCampaign> ApplicableMarketingCampaigns(IikoCard51PaymentType paymentType)
        {
            return MarketingCampaigns.GetOrDefault(paymentType.Id, new List<IikoCard51MarketingCampaign>())
                .Where(mc => mc.IsApplicable)
                .ToList();
        }

        [NotNull]
        public string GetPosServerAddress()
        {
            if (PosServerLocationCallCenter == IikoCard51PosServerLocation.SERVER)
                return new Uri(ServerSession.CurrentSession.ServerUrl).GetComponents(
                           UriComponents.Scheme | UriComponents.Host, UriFormat.UriEscaped) + ":7001/";
            if (PosServerLocationCallCenter == IikoCard51PosServerLocation.MANUAL)
                return PosServerAddressCallCenter;

            throw new InvalidEnumArgumentException(
                string.Format("Not supported pos server location for callcenter: {0}", PosServerLocationCallCenter));
        }
    }
}