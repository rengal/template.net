using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resto.Data
{
    public partial class IncomingInventoryStep
    {
        public bool IsInvalid
        {
            get { return step == INVALID.step; }
        }

        public bool IsFirst
        {
            get { return step == FIRST.step; }
        }

        public bool IsSecond
        {
            get { return step == SECOND.step; }
        }

        public bool IsThird
        {
            get { return step == THIRD.step; }
        }
    }
}
