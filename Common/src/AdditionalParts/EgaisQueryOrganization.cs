using System;
using System.Linq;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    partial class EgaisQueryOrganization
    {
        public override EgaisDocumentTypes Type
        {
            get { return EgaisDocumentTypes.EGAIS_QUERY_ORGANIZATION; }
        }

        /// <summary>
        /// Готов к отправке (необходимые поля заполнены)
        /// </summary>
        public override bool ReadyToSend
        {
            get { return !SourceRarId.IsNullOrWhiteSpace() && Parameters.Any() && Status.Editable && !Deleted; }
        }

        public override string DocumentFullCaption
        {
            get
            {
                return string.Format(Resources.EgaisQueryOrganizationFullCaption, DocumentNumber.IsNullOrWhiteSpace() ? string.Empty : DocumentNumber + " ", DateIncoming ?? DateTime.Today);
            }
        }

        /// <summary>
        /// Возвращает ИНН организации, если он есть
        /// </summary>
        public string GetInn()
        {
            string inn;
            return Parameters.TryGetValue(EgaisQueryParameter.BY_INN.Code, out inn) ? inn : string.Empty;
        }

        /// <summary>
        /// Возвращает РАР организации, если он есть
        /// </summary>
        public string GetFsRarId()
        {
            string fsRarId;
            return Parameters.TryGetValue(EgaisQueryParameter.BY_FSRAR_ID.Code, out fsRarId) ? fsRarId : string.Empty;
        }
    }
}
