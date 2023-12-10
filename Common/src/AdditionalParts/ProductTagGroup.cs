using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class ProductTagGroup
    {
        [NotNull]
        [Pure]
        public IEnumerable<ProductTag> GetTags()
        {
            return EntityManager.INSTANCE.GetAll<ProductTag>(t => Equals(this, t.ProductTagGroup));
        }

        [NotNull]
        [Pure]
        public IEnumerable<ProductTag> GetNotDeletedTags()
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<ProductTag>(t => Equals(this, t.ProductTagGroup));
        }
    }
}
