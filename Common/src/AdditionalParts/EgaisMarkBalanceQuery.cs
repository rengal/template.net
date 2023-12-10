using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class EgaisMarkBalanceQuery
    {
        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_MARK_BALANCE_QUERY; }
        }

        public override string DocumentFullCaption
        {
            get
            {
                return string.Format(Resources.EgaisMarkBalanceQueryFullCaption, DateIncoming);
            }
        }

        /// <summary>
        /// Возвращает справку 2, по которой осуществлялся запрос, если она есть
        /// </summary>
        [NotNull]
        public string GetBRegId()
        {
            string bRegId;
            return Parameters.TryGetValue(EgaisQueryParameter.BY_FORM2.Code, out bRegId) ? bRegId : string.Empty;
        }

        /// <summary>
        /// Документ готов к отправке
        /// </summary>
        public override bool ReadyToSend
        {
            get { return Status.Editable && !Deleted && Parameters.Any(); }
        }
    }
}
