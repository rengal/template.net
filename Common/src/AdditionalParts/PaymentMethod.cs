using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    public partial class PaymentMethod
    {
        public override string ToString()
        {
            return NameLocal;
        }
    }
}
