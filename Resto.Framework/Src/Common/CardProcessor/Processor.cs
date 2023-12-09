using System;

namespace Resto.Framework.Common.CardProcessor
{
    ///<summary>
    /// Интерфейс, описывающий процессор, сообщающий о прокартке карты
    ///</summary>
    public abstract class Processor
    {
        public virtual bool Enabled { get; set; }

        public abstract void Attach();

        public event EventHandler CardRollStarted;
        public event EventHandler CardRollCompleted;
        public event EventHandler<EventArgs<MagnetTrackData>> CardRollSucceeded;
        public event EventHandler<EventArgs<object>> CardRollFailed;

        protected void OnCardRollStarted(object sender, EventArgs e)
        {
            if (CardRollStarted != null)
            {
                CardRollStarted(sender, e);
            }
        }

        protected void OnCardRollCompleted(object sender, EventArgs e)
        {
            if (CardRollCompleted != null)
            {
                CardRollCompleted(sender, e);
            }
        }

        protected void OnCardRollSucceeded(object sender, EventArgs<MagnetTrackData> e)
        {
            if (CardRollSucceeded != null)
            {
                CardRollSucceeded(sender, e);
            }
        }

        protected void OnCardRollFailed(object sender, EventArgs<object> e)
        {
            if (CardRollFailed != null)
            {
                CardRollFailed(sender, e);
            }
        }

        public virtual void Configurate(ICardReaderConfig cardReaderConfig, IReaderDevicesConfig readerDevicesConfig)
        {
        }

        public abstract bool CheckIsActive(ICardReaderConfig cardReaderConfig, IReaderDevicesConfig readerDevicesConfig);

        public abstract void Detach();
    }
}