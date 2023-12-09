using System;
using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;
using System.Linq;

namespace Resto.Framework.Common.Barcodes
{
    /// <summary>
    /// Линейный штрихкод EAN13.
    /// </summary>
    public sealed class BarcodeEan13 : IBarcode, IEquatable<BarcodeEan13>
    {
        #region Fields
        private const string InternalBarcodePrefix = "2";
        private const int ZeroCharCode = '0';
        public const int ProductArticleMaxSize = 5;
        public const decimal ProductAmountMin = 0m;
        public const decimal ProductWeightAmountMax = 99.999m;
        public static readonly decimal ProductPieceAmountMax = Math.Floor(CommonConstants.MaximumItemAmount);
        [NotNull]
        private readonly string barcodeString = string.Empty;
        #endregion

        #region Ctor

        /// <summary>
        /// Пустой конструктор для корректной работы десериализатора
        /// </summary>
        [UsedImplicitly]
        public BarcodeEan13()
        {

        }

        /// <summary>
        /// Создаёт экземпляр штрихкода по строке <paramref name="barcodeString"/>
        /// </summary>
        /// <param name="barcodeString">Строка с цифрами штрихкода</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="barcodeString"/><c> == null</c>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="barcodeString"/> не является допустимой строкой для штрихкода EAN13
        /// (см. <see cref="IsValid"/>)
        /// </exception>
        /// <seealso cref="IsValid"/>
        public BarcodeEan13([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            if (!IsValid(barcodeString))
                throw new ArgumentOutOfRangeException(nameof(barcodeString), barcodeString, "Invalid barcode string");

            this.barcodeString = barcodeString;
        }

        private BarcodeEan13(int secondNum, [NotNull] string barcodeData)
        {
            if (secondNum < 0 || secondNum > 9)
                throw new ArgumentOutOfRangeException(nameof(secondNum), secondNum, "secondNum must be value between 0 and 9 inclusive");

            Debug.Assert(barcodeData.Length == 10);
            Debug.Assert(barcodeData.All(char.IsDigit));

            barcodeString = InternalBarcodePrefix + secondNum + barcodeData;
            barcodeString += GetCheckSum(barcodeString);
        }
        #endregion

        #region Props
        public string BarcodeString => barcodeString;

        /// <summary>
        /// Является ли штрихкод внутренним штрихкодом
        /// (внутренние штрихкоды начинаются с «2»)
        /// </summary>
        public bool IsInternal => barcodeString[0] == InternalBarcodePrefix[0];

        /// <summary>
        /// Вторая цифра во внутреннем штрихкоде, число от 0 до 9
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Штрихкод не является внутренним (см. <see cref="IsInternal"/>)
        /// </exception>
        /// <seealso cref="IsInternal"/>
        public int SecondNum
        {
            get
            {
                if (!IsInternal)
                    throw new InvalidOperationException();

                return barcodeString[1] - ZeroCharCode;
            }
        }

        /// <summary>
        /// Данные внутреннего штрихкода (10 цифр, без префиксов и контрольной суммы)
        /// в виде числа
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Штрихкод не является внутренним (см. <see cref="IsInternal"/>)
        /// </exception>
        /// <seealso cref="IsInternal"/>
        public int? BarcodeDataHash
        {
            get
            {
                if (!IsInternal)
                    throw new InvalidOperationException();

                if (uint.TryParse(barcodeString.Substring(2, 10), out var result))
                    return (int)result;

                return null;
            }
        }

        /// <summary>
        /// Артикул товара во внутреннем штрихкоде (5 цифр начиная с третьей)
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Штрихкод не является внутренним (см. <see cref="IsInternal"/>)
        /// </exception>
        /// <seealso cref="IsInternal"/>
        public string ProductArticle
        {
            get
            {
                if (!IsInternal)
                    throw new InvalidOperationException();

                return barcodeString.Substring(2, ProductArticleMaxSize);
            }
        }

        /// <summary>
        /// Вес товара во внутреннем штрихкоде (5 цифр начиная с восьмой)
        /// в основных единицах измерения (не в тысячных долях)
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Штрихкод не является внутренним (см. <see cref="IsInternal"/>)
        /// </exception>
        /// <seealso cref="IsInternal"/>
        public decimal ProductWeight
        {
            get
            {
                if (!IsInternal)
                    throw new InvalidOperationException();

                return decimal.Parse(barcodeString.Substring(7, 5)) / 1000m;
            }
        }

        /// <summary>
        /// Количество товара во внутреннем штрихкоде (5 цифр начиная с восьмой)
        /// в штуках
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Штрихкод не является внутренним (см. <see cref="IsInternal"/>)
        /// </exception>
        /// <seealso cref="IsInternal"/>
        public decimal ProductAmount
        {
            get
            {
                if (!IsInternal)
                    throw new InvalidOperationException();

                return decimal.Parse(barcodeString.Substring(7, 5));
            }
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return barcodeString;
        }
        #endregion

        #region Equality Members
        public bool Equals(BarcodeEan13 other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(other.barcodeString, barcodeString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(BarcodeEan13))
                return false;
            return Equals((BarcodeEan13)obj);
        }

        public override int GetHashCode()
        {
            return barcodeString.GetHashCode();
        }

        public static bool operator ==(BarcodeEan13 left, BarcodeEan13 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BarcodeEan13 left, BarcodeEan13 right)
        {
            return !Equals(left, right);
        }
        #endregion

        #region Helpers
        private static char GetCheckSum([NotNull] string barcodeString)
        {
            Debug.Assert(barcodeString != null);
            Debug.Assert(barcodeString.Length >= 12);
            Debug.Assert(barcodeString.All(char.IsDigit));

            var checkSum = 0;

            for (var i = 0; i < 12; i++)
            {
                checkSum += (barcodeString[i] - ZeroCharCode) * (((i + 1) % 2 == 0) ? 3 : 1);
            }

            checkSum = (checkSum / 10 + (checkSum % 10 != 0 ? 1 : 0)) * 10 - checkSum;

            Debug.Assert(checkSum >= 0 && checkSum <= 9);
            return (char)(checkSum + ZeroCharCode);
        }

        /// <summary>
        /// Проверяет, является ли строка <paramref name="barcodeString"/> допустимым штрихкодом.
        /// </summary>
        /// <param name="barcodeString">Проверяемая строка</param>
        /// <returns>Результат проверки</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="barcodeString"/> <c> == null</c>
        /// </exception>
        public static bool IsValid([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));

            if (barcodeString.Length != 13)
                return false;

            for (var i = 0; i < 13; i++)    // Простой цикл гораздо быстрее LINQ и тем более регулярок
            {
                if (!char.IsDigit(barcodeString[i]))
                    return false;
            }

            return GetCheckSum(barcodeString) == barcodeString[12];
        }

        public static bool IsValidProductArticle([NotNull] string productArticle)
        {
            if (productArticle == null)
                throw new ArgumentNullException(nameof(productArticle));

            if (productArticle.Length != ProductArticleMaxSize)
                return false;

            for (var i = 0; i < ProductArticleMaxSize; i++)
            {
                if (!char.IsDigit(productArticle[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Создаёт и возвращает внутренний штрихкод с второй цифрой <paramref name="secondNum"/>
        /// и остальными цифрами (с 3 по 12), получаемыми из <paramref name="hash"/>.
        /// Контрольная сумма рассчитывается автоматически.
        /// </summary>
        /// <param name="secondNum">Вторая цифра штрихкода, от 0 до 9</param>
        /// <param name="hash">int который будет преобразован в штрих код</param>
        /// <returns>Созданный штрихкод</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="secondNum"/> меньше 0 или больше 9
        /// </exception>
        public static BarcodeEan13 CreateInternalBarcode(int secondNum, int hash)
        {
            return new BarcodeEan13(secondNum, ((uint)hash).ToString("0000000000"));
        }

        /// <summary>
        /// Создаёт и возвращает внутренний штрихкод с второй цифрой <paramref name="secondNum"/>,
        /// цифрами с 3 по 7, получаемыми из <paramref name="productArticle"/> и
        /// цифрами с 8 по 12, получаемыми из <paramref name="amount"/>.
        /// Контрольная сумма рассчитывается автоматически.
        /// </summary>
        /// <param name="secondNum">Вторая цифра штрихкода, от 0 до 9</param>
        /// <param name="productArticle">Артикул продукта, строка из 5 цифровых символов</param>
        /// <param name="amount">
        /// Количество продукта в основных единицах измерения, от 0,001 до 99,999.
        /// Округляется до 3 знаков после запятой и кодируется в штрихкоде в тысячных долях (умножается на 1000).
        /// </param>
        /// <returns>Созданный штрихкод</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="productArticle"/><c> == null</c>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="secondNum"/> меньше 0 или больше 9 или
        /// длина <paramref name="productArticle"/> не равна 5 или
        /// <paramref name="productArticle"/> содержит нецифровые символы или
        /// <paramref name="amount"/> выходит за интервал [0,001; 99,999]
        /// </exception>
        public static BarcodeEan13 CreateWeightBarcodeForProduct(int secondNum, [NotNull] string productArticle, decimal amount)
        {
            CheckArticleBeforeCreateBarcode(productArticle);

            // Генерация штрих-кода весового товара.
            var roundedAmount = Math.Round(amount, 3, MidpointRounding.AwayFromZero);
            if (roundedAmount <= ProductAmountMin || roundedAmount > ProductWeightAmountMax)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, $"amount value must be greater than {ProductAmountMin} and less or equal to {ProductWeightAmountMax}");

            return new BarcodeEan13(secondNum, productArticle + ((int)(roundedAmount * 1000m)).ToString("00000"));
        }

        /// <summary>
        /// Создаёт и возвращает внутренний штрихкод с второй цифрой <paramref name="secondNum"/>,
        /// цифрами с 3 по 7, получаемыми из <paramref name="productArticle"/> и
        /// цифрами с 8 по 12, получаемыми из <paramref name="amount"/>.
        /// Контрольная сумма рассчитывается автоматически.
        /// </summary>
        /// <param name="secondNum">Вторая цифра штрихкода, от 0 до 9</param>
        /// <param name="productArticle">Артикул продукта, строка из 5 цифровых символов</param>
        /// <param name="amount">
        /// Количество продукта в основных единицах измерения, от 1 до 999.
        /// </param>
        /// <returns>Созданный штрихкод</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="productArticle"/><c> == null</c>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="secondNum"/> меньше 0 или больше 9 или
        /// длина <paramref name="productArticle"/> не равна 5 или
        /// <paramref name="productArticle"/> содержит нецифровые символы или
        /// <paramref name="amount"/> выходит за интервал [1; 999]
        /// </exception>
        public static BarcodeEan13 CreatePieceBarcodeForProduct(int secondNum, [NotNull] string productArticle, int amount)
        {
            CheckArticleBeforeCreateBarcode(productArticle);

            if (amount <= ProductAmountMin || amount > ProductPieceAmountMax)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, $"amount value must be greater than {ProductAmountMin} and less or equal to {ProductPieceAmountMax}");

            // Генерация штрих-кода штучного товара.
            return new BarcodeEan13(secondNum, productArticle + amount.ToString("00000"));
        }

        private static void CheckArticleBeforeCreateBarcode([NotNull] string productArticle)
        {
            if (productArticle == null)
                throw new ArgumentNullException(nameof(productArticle));
            if (!IsValidProductArticle(productArticle))
                throw new ArgumentOutOfRangeException(nameof(productArticle), productArticle, $"productArticle must be digit string with {ProductArticleMaxSize} chars length");

        }
        #endregion
    }
}
