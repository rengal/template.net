using System;
using System.Collections.Generic;
using Resto.Common.Localization;

namespace Resto.Data
{
    public partial class TransactionType
    {
        public string IdString
        {
            get { return Id; }
        }

        public string LocalShortName
        {
            get { return this.GetLocalShortName(); }
        }

        public override bool Equals(object obj)
        {
            var transactionType = obj as TransactionType;
            return transactionType != null && __value == transactionType.__value;
        }

        public override int GetHashCode()
        {
            return __value.GetHashCode();
        }

        public static bool operator ==(TransactionType left, TransactionType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TransactionType left, TransactionType right)
        {
            return !Equals(left, right);
        }
    }
}