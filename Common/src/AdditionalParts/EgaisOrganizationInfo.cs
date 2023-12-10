using System.Linq;
using System.Text.RegularExpressions;
using EnumerableExtensions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class EgaisOrganizationInfo
    {
        [CanBeNull]
        public User Сounteragent => GetCounteragent();

        public bool IsCorrect
        {
            get
            {
                return IsCorrectClientRegId(ClientRegId) && Address != null &&
                       EgaisAddress.IsCorrectCountry(Address.Country) &&
                       EgaisAddress.IsCorrectRegionCode(Address.RegionCode, EgaisOrganizationType) &&
                       EgaisAddress.IsCorrectAddressDescription(Address.Description) &&
                       IsCorrectInn(Inn, EgaisOrganizationType) && IsCorrectKpp(Kpp, EgaisOrganizationType) &&
                       IsCorrectFullName(FullName) && IsCorrectShortName(ShortName);
            }
        }

        #region Methods

        public static bool IsCorrectClientRegId(string id)
        {
            return !id.IsNullOrWhiteSpace() && Regex.IsMatch(id, EgaisConstraints.NOT_EMPTY_STRING_50.Pattern);
        }

        public static bool IsCorrectInn(string inn, EgaisOrganizationType organizationType)
        {
            return organizationType == EgaisOrganizationType.UL && !inn.IsNullOrWhiteSpace() && Regex.IsMatch(inn, EgaisConstraints.INN10.Pattern) ||
                   organizationType == EgaisOrganizationType.FL && !inn.IsNullOrWhiteSpace() && Regex.IsMatch(inn, EgaisConstraints.INN12.Pattern) ||
                   organizationType == EgaisOrganizationType.FO ||
                   organizationType == EgaisOrganizationType.TS && Regex.IsMatch(inn, EgaisConstraints.NOT_EMPTY_STRING_50.Pattern);
        }

        public static bool IsCorrectKpp(string kpp, EgaisOrganizationType organizationType)
        {
            return organizationType != EgaisOrganizationType.UL ||
                       !kpp.IsNullOrWhiteSpace() && Regex.IsMatch(kpp, EgaisConstraints.KPP.Pattern);
        }

        public static bool IsCorrectShortName(string shortName)
        {
            return !shortName.IsNullOrWhiteSpace() && Regex.IsMatch(shortName, EgaisConstraints.ANY_STRING_64.Pattern);
        }

        public static bool IsCorrectFullName(string fullName)
        {
            return !fullName.IsNullOrWhiteSpace() && Regex.IsMatch(fullName, EgaisConstraints.ANY_STRING_255.Pattern);
        }

        [CanBeNull]
        private User GetCounteragent()
        {
            var result = EntityManager.INSTANCE.GetAllNotDeleted<User>()
                .Where(user =>
                    !user.TaxpayerIdNumber.IsNullOrWhiteSpace() &&
                    user.TaxpayerIdNumber == Inn &&
                    !user.AccountingReasonCode.IsNullOrWhiteSpace() &&
                    user.AccountingReasonCode == Kpp)
                .ToArray();

            if (result.IsEmpty())
            {
                return null;
            }

            if (result.Length == 1)
            {
                return result.Single();
            }

            // Если нашлось несколько контрагентов - выбираем первого по алфавиту
            return result.OrderBy(user => user.NameLocal).First();
        }

        public bool Equals(EgaisOrganizationInfo organizationInfo)
        {
            if (organizationInfo == null)
            {
                return false;
            }

            return ClientRegId == organizationInfo.ClientRegId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EgaisOrganizationInfo);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            if (!ClientRegId.IsNullOrEmpty())
            {
                hashCode ^= ClientRegId.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            return Сounteragent != null ? Сounteragent.ToString() : ShortName;
        }

        #endregion
    }
}
