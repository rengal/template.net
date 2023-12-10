using Resto.Common.Interfaces;

namespace Resto.Data
{
    public partial class EgaisAbstractMarkDocumentItem : IEgaisRecordWithMarks, IEgaisRecordWithBRegId
    {
        /// <summary>
        /// Количество марок
        /// </summary>
        public int MarkCount
        {
            get { return Marks.Count; }
        }
    }
}
