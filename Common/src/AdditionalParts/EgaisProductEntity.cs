using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    public partial class EgaisProductEntity
    {
        /// <summary>
        /// Возвращает <see cref="ProductInfoV1"/> если он не null, 
        /// иначе <see cref="ProductInfoV2"/>
        /// </summary>
        public EgaisProductInfo GetV1First()
        {
            return ProductInfoV1 ?? ProductInfoV2;
        }

        /// <summary>
        /// Возвращает <see cref="ProductInfoV2"/> если он не null, 
        /// иначе <see cref="ProductInfoV1"/>
        /// </summary>
        public EgaisProductInfo GetV2First()
        {
            return ProductInfoV2 ?? ProductInfoV1;
        }
    }
}
