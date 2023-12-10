using System.Collections.Generic;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class StoreReportManagerSetupList
    {
        public static StoreReportManagerSetupList INSTANCE
        {
            get
            {
                IList<StoreReportManagerSetupList> states =
                    EntityManager.INSTANCE.GetAllNotDeleted<StoreReportManagerSetupList>();
                if (states.Count == 0)
                {
                    return new StoreReportManagerSetupList(GuidGenerator.Next());
                }
                if (states.Count > 1)
                {
                    throw new RestoException("Too many CafeState objects found: " + states);
                }
                return states[0];
            }
        }

        public Dictionary<string, StoreReportManagerSetup> GetAll(StoreReportManagerSetupType setupType)
        {
            if (list.ContainsKey(setupType)) return list[setupType];
            return new Dictionary<string, StoreReportManagerSetup>();
        }
    }
}