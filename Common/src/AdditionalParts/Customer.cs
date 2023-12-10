using System;
using System.Collections.Generic;
using System.Linq;
using EnumerableExtensions;
using Resto.Common;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;

// ReSharper disable once CheckNamespace
namespace Resto.Data
{
    public partial class Customer : IWithPdpInfo, IWithDependents, IComparable<Customer>, IComparable
    {
        /// <inheritdoc />
        public bool HasConsentPeriod => ConsentDateFrom.HasValue && ConsentDateTo.HasValue;

        /// <inheritdoc />
        public bool HasProcessingPeriod => ProcessingDateFrom.HasValue && ProcessingDateTo.HasValue;

        private CustomerEmail MainEmail
        {
            get { return ValidEmails.FirstOrDefault(e => e.IsMain); }
        }

        public CustomerEmail MainOrFirstEmail
        {
            get { return MainEmail ?? ValidEmails.FirstOrDefault(); }
        }

        private Address MainAddress
        {
            get { return ValidAddresses.FirstOrDefault(a => a.IsMainAddress); }
        }

        public Address MainOrFirstAddress
        {
            get { return MainAddress ?? ValidAddresses.FirstOrDefault(); }
        }

        private CustomerPhone MainPhone
        {
            get { return ValidPhones.FirstOrDefault(p => p.IsMain); }
        }

        public CustomerPhone MainOrFirstPhone
        {
            get { return MainPhone ?? ValidPhones.FirstOrDefault(); }
        }

        public bool IsIikoBizAccountBinded
        {
            get { return IikoBizInfo?.IikoNetId != null; }
        }


        public bool IsCustomer
        {
            get { return !CardNumber.IsNullOrEmpty() || !Phones.IsEmpty(); }
        }

        /// <summary>
        /// RMS-49217. В результате ошибок при формировании индексов
        /// в списке адресов могут быть null сущности.
        /// Для изменения коллекции использовать Addresses!!!
        /// </summary>
        [NotNull]
        public IReadOnlyList<Address> ValidAddresses
        {
            get { return addresses.Where(a => a.IsValid()).ToList(); }
        }

        /// <summary>
        /// RMS-49217. В результате ошибок при формировании индексов
        /// в списке телефонов могут быть null сущности.
        /// Для изменения коллекции использовать Phones!!!
        /// </summary>
        [NotNull]
        public IReadOnlyList<CustomerPhone> ValidPhones
        {
            get { return phones.Where(p => !(p?.PhoneNumber).IsNullOrWhiteSpace()).ToList(); }
        }

        /// <summary>
        /// RMS-49217. В результате ошибок при формировании индексов
        /// в списке email-ов могут быть null сущности.
        /// Для изменения коллекции использовать Emails!!!
        /// </summary>
        [NotNull]
        public IReadOnlyList<CustomerEmail> ValidEmails
        {
            get { return emails.Where(e => !(e?.Email).IsNullOrWhiteSpace()).ToList(); }
        }

        public string SurnameAndName
        {
            get { return string.Join(" ", new[] { Surname, Name }.Where(s => !string.IsNullOrEmpty(s))); }
        }

        public IDictionary<Guid, int> GetDependents()
        {
            var dps = ValidAddresses
                .Select(address => address.Street)
                .Cast<CachedEntity>()
                .Concat(ValidAddresses
                    .Select(address => address.Street.City));

            if (MarketingSource != null)
            {
                dps = dps.ContinueWith(MarketingSource);
            }
            return dps.Distinct().ToDictionary(e => e.Id, e => e.Revision);
        }

        public override string ToString()
        {
            return SurnameAndName;
        }

        public int CompareTo(Customer other)
        {
            if (other == null)
            {
                return 1;
            }

            int result = string.Compare(SurnameAndName, other.SurnameAndName, StringComparison.CurrentCulture);
            if (result == 0)
            {
                result = Id.CompareTo(other.Id);
            }

            return result;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Customer other))
            {
                return 1;
            }

            return CompareTo(other);
        }
    }
}
