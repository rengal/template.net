using System;
using System.Collections.Generic;
using Resto.Common.Localization;

namespace Resto.Data
{
    public partial class DeliveryStatus
    {
        public bool IsClosed
        {
            get { return this == CLOSED || this == CANCELLED; }
        }
    }
}
