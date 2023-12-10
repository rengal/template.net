using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class EgaisTransactionPart
    {
        /// <summary>
        /// Создает траназкцию с заполенными справками 1, 2 и пустыми остальными значениями
        /// </summary>
        [NotNull]
        public static EgaisTransactionPart CreateFakeTransaction(EgaisTransactionPartKey key)
        {
            return new EgaisTransactionPart(Guid.NewGuid(),
                key,
                DateTime.MinValue,
                new decimal(), new decimal(), new decimal(), new decimal(), new decimal(), null,
                new Guid(), new Guid(), string.Empty, string.Empty, string.Empty, false, new decimal(), null);
        }
    }
}
