
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resto.Data
{
    public partial class ProductTag
    {
        #region Overrides of PersistedEntity

        public override string ToString()
        {
            return NameLocal;
        }

        #endregion
    }
}
