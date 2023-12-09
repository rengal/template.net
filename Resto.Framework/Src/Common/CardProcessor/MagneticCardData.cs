using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.CardProcessor
{
    public sealed class MagnetTrackData
    {
        [CanBeNull] private readonly ReaderDevice readerDevice;
        [CanBeNull] private readonly string rawData;
        [CanBeNull] private readonly string track1;
        [CanBeNull] private readonly string track2;
        [CanBeNull] private readonly string track3;
        [CanBeNull] private readonly string secret;

        #region Ctors

        [UsedImplicitly]
        private MagnetTrackData()
        {
        }

        public MagnetTrackData(string track1, string track2, string track3)
            : this(null, track1, track2, track3)
        {
        }

        public MagnetTrackData(string rawData, string track1, string track2, string track3)
            : this(null, rawData, track1, track2, track3, GetDefaultSecret(track1, track2, track3))
        {
        }

        public MagnetTrackData(ReaderDevice readerDevice, string rawData, string track1, string track2, string track3)
            : this(readerDevice, rawData, track1, track2, track3, GetDefaultSecret(track1, track2, track3))
        {
        }

        public MagnetTrackData(ReaderDevice readerDevice, string rawData, string track1, string track2, string track3, string secret)
        {
            this.readerDevice = readerDevice;
            this.rawData = rawData;
            this.track1 = track1;
            this.track2 = track2;
            this.track3 = track3;
            this.secret = secret;
        }
        #endregion

        #region Props

        [CanBeNull]
        public ReaderDevice ReaderDevice
        {
            get { return readerDevice; }
        }

        [CanBeNull]
        public string RawData
        {
            get { return rawData; }
        }

        [CanBeNull]
        public string Track1
        {
            get { return track1; }
        }

        [CanBeNull]
        public string Track2
        {
            get { return track2; }
        }

        [CanBeNull]
        public string Track3
        {
            get { return track3; }
        }

        [CanBeNull]
        public string Secret
        {
            get { return secret; }
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return
                $"1:{CardUtils.MaskCardTrack(Track1)}, 2:{CardUtils.MaskCardTrack(Track2)}, 3:{CardUtils.MaskCardTrack(Track3)}";
        }

        private static string GetDefaultSecret(string track1, string track2, string track3)
        {
            return $"{track1};{track2}?{track3}";
        }
        #endregion
    }
}