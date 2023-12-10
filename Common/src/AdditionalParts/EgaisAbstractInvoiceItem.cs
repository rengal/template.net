using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class EgaisAbstractInvoiceItem : IEgaisRecordWithMarks
    {
        /// <summary>
        /// Все марки из всех коробок
        /// </summary>
        [CanBeNull]
        public List<EgaisMark> Marks
        {
            get { return MarksByBox?.SelectMany(box => box.Marks).ToList(); }
        }

        /// <summary>
        /// Количество марок
        /// </summary>
        public int MarkCount
        {
            get
            {
                var notConfirmedCount = 0;
                if (MarksNotConfirmed != null)
                {
                    notConfirmedCount = MarksNotConfirmed.Count;
                }
                return MarksByBox?.Sum(markBox => markBox.Marks.Count) - notConfirmedCount ?? 0;
            }
        }
    }
}
