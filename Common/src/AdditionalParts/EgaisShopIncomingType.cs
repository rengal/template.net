using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Common.Localization;

namespace Resto.Data
{
    public partial class EgaisShopIncomingType
    {
        public override string ToString()
        {
            return this.GetLocalName();
        }
    }
}
