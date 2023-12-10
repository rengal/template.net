using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Properties;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class SupplierInfo : IWithContainerId
    {
        /// <summary>
        /// Возвращает список SupplierInfo по поставщику <see cref="supplier"/>.
        /// </summary>
        public static List<SupplierInfo> GetSupplierInfosBySupplier(User supplier)
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<SupplierInfo>().Where(supInfo => supInfo.Supplier.Equals(supplier)).ToList();
        }

        /// <summary>
        /// Возвращает список SupplierInfo по поставщику <see cref="supplier"/> и продукту <see cref="nativeProduct"/>.
        /// </summary>
        public static List<SupplierInfo> GetSupplierInfosBySupplierAndNativeProduct(User supplier, Product nativeProduct)
        {
            return GetSupplierInfosBySupplier(supplier).Where(supInfo => supInfo.NativeProduct.Equals(nativeProduct)).ToList();
        }

        /// <summary>
        /// Возвращает связку SupplierInfo по поставщику <see cref="supplier"/>, продукту <see cref="nativeProduct"/> и контейнеру <see cref="containerId"/>.
        /// </summary>
        public static SupplierInfo GetSupplierInfoBy(User supplier, Product nativeProduct, Guid containerId)
        {
            return GetSupplierInfosBySupplierAndNativeProduct(supplier, nativeProduct).FirstOrDefault(supInfo => supInfo.ContainerId == containerId);
        }
                
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SupplierInfo))
                return false;
            if (Id == (obj as SupplierInfo).Id)
                return true;

            var supInfo = (SupplierInfo)obj;
            return ContainerId == supInfo.ContainerId && NativeProduct == supInfo.NativeProduct && Supplier == supInfo.Supplier;
        }

        public override int GetHashCode()
        {
            return new { ContainerId, NativeProduct, Supplier }.GetHashCode();
        }

        public static bool operator ==(SupplierInfo supInfo1, SupplierInfo supInfo2)
        {
            if (ReferenceEquals(supInfo1, supInfo2))
                return true;
            if ((supInfo1 as object) == null || (supInfo2 as object) == null)
                return false;

            return supInfo1.Equals(supInfo2);
        }

        public static bool operator !=(SupplierInfo supInfo1, SupplierInfo supInfo2)
        {
            return !(supInfo1 == supInfo2);
        }

        public override string ToString()
        {
            return string.Format(Resources.SupplierInfoToString, supplier.NameLocal, NativeProduct.NameLocal);
        }
    }
}