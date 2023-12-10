using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Common.Interfaces;

namespace Resto.Data
{
    public partial class EgaisRequestRepealItem : IEgaisRecordWithMarks, IEgaisRecordWithBRegId
    {
        /// <summary>
        /// Количество марок
        /// </summary>
        public int MarkCount => Marks.Count;
    }
}
