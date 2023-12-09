using System;
using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.CardProcessor
{
    public sealed class PollErrorEvent : IWaiterLockEvent
    {
        #region Static Members
        public static readonly IWaiterLockEvent Instance = new PollErrorEvent();
        #endregion

        #region Ctor
        private PollErrorEvent()
        {}
        #endregion

        #region Props
        public bool ErrorEvent
        {
            get { return true; }
        }

        public bool KeyInserted
        {
            get { return false; }
        }

        public string KeyNumber
        {
            get { throw new NotSupportedException(); }
        }
        #endregion
    }

    public sealed class KeyExtractedEvent : IWaiterLockEvent
    {
        #region Static Members
        public static readonly IWaiterLockEvent Instance = new KeyExtractedEvent();
        #endregion

        #region Ctor
        private KeyExtractedEvent()
        {}
        #endregion

        #region Props
        public bool ErrorEvent
        {
            get { return false; }
        }

        public bool KeyInserted
        {
            get { return false; }
        }

        public string KeyNumber
        {
            get { throw new NotSupportedException(); }
        }
        #endregion
    }

    public sealed class KeyInsertedEvent : IWaiterLockEvent
    {
        #region Static Members
        private static readonly Cache<string, IWaiterLockEvent> Cache =
            new Cache<string, IWaiterLockEvent>(keyNumber => new KeyInsertedEvent(keyNumber));

        [NotNull]
        public static IWaiterLockEvent GetFor([NotNull] string keyNumber)
        {
            return Cache[keyNumber];
        }
        #endregion

        #region Fields
        [NotNull]
        private readonly string keyNumber;
        #endregion

        #region Ctor
        private KeyInsertedEvent([NotNull] string keyNumber)
        {
            Debug.Assert(keyNumber != null);

            this.keyNumber = keyNumber;
        }
        #endregion

        #region Props
        public bool ErrorEvent
        {
            get { return false; }
        }

        public bool KeyInserted
        {
            get { return true; }
        }

        public string KeyNumber
        {
            get { return keyNumber; }
        }
        #endregion

        #region Equality
        private bool Equals(KeyInsertedEvent other)
        {
            return string.Equals(keyNumber, other.keyNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj is KeyInsertedEvent && Equals((KeyInsertedEvent)obj);
        }

        public override int GetHashCode()
        {
            return keyNumber.GetHashCode();
        }
        #endregion
    }
}