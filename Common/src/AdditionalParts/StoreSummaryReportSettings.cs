using System.Linq;
using Resto.Common.Localization;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class StoreSummaryReportSettings
    {
        public override string ToString()
        {
            return NameLocal;
        }

        public StoreSummaryReportSettings Clone()
        {
            return (StoreSummaryReportSettings)DeepClone(GuidGenerator.Next());
        }

        public string SettingsComment
        {
            get
            {
                string result = "";
                if (Filter.SecondaryAccounts.Count > 0)
                {
                    result += Resources.StoreSummaryReportSettingsAccounts;
                    result += AccountNames;
                }
                if (Filter.Counteragents.Count > 0)
                {
                    result += Resources.StoreSummaryReportSettingsContractor;
                    result += CounterAgentNames;
                }
                if (Filter.PrimaryStores.Count > 0)
                {
                    result += Resources.StoreSummaryReportSettingsStorages;
                    result += StoreNames;
                }
                if (Filter.TransactionTypes.Count > 0)
                {
                    result += Resources.StoreSummaryReportSettingsOperationTypes;
                    result += OperationTypeNames;
                }
                if (Grouping.Detalizations.Count > 0)
                {
                    result += Resources.StoreSummaryReportSettingsSpecificationBy;
                    result += DetalizationNames;
                }
                return result;
            }
        }

        public string DetalizationNames
        {
            get
            {
                string result = "";
                foreach (StoreOperationsDetalization det in Grouping.Detalizations)
                    result += det.GetLocalName() + ", ";
                return result.Trim().TrimEnd(',');
            }
        }

        public string StoreNames
        {
            get
            {
                string result = "";
                foreach (Store store in Filter.PrimaryStores)
                    result += store.NameLocal + ", ";
                return result.Trim().TrimEnd(',');
            }
        }

        public string CounterAgentNames
        {
            get
            {
                string result = "";
                foreach (User user in Filter.Counteragents)
                    result += user.NameLocal + ", ";
                return result.Trim().TrimEnd(',');
            }
        }

        public string OperationTypeNames
        {
            get
            {
                string result = "";
                foreach (TransactionType trans in Filter.TransactionTypes)
                    result += trans.GetLocalName() + ", ";
                return result.Trim().TrimEnd(',');
            }
        }

        public string AccountNames
        {
            get
            {
                string result = "";
                foreach (Account account in Filter.SecondaryAccounts)
                    result += account.NameLocal + ", ";
                return result.Trim().TrimEnd(',');
            }
        }

        public void TruncateOuterEntitys()
        {
            Filter.Counteragents.Set(Filter.Counteragents.ContainsInCache());
            Filter.PrimaryStores.Set(Filter.PrimaryStores.ContainsInCache());
            Filter.Products.Set(Filter.Products.ContainsInCache());
            Filter.SecondaryAccounts.Set(Filter.SecondaryAccounts.ContainsInCache());
            Filter.SecondaryProducts.Set(Filter.SecondaryProducts.ContainsInCache());
            Filter.DocumentTypes.Set(Filter.DocumentTypes.ToList().Where(docType => DocumentType.VALUES.Contains(docType)));
            Filter.TransactionTypes.Set(Filter.TransactionTypes.ToList().Where(transType => TransactionType.VALUES.Contains(transType)));
            Grouping.Detalizations.Set(Grouping.Detalizations.ToList().Where(det => StoreOperationsDetalization.VALUES.Contains(det)));
            if (!DateDetalization.VALUES.Contains(Grouping.DateDetalization))
            {
                Grouping.DateDetalization = DateDetalization.TOTAL_ONLY;
            }
        }
    }
}