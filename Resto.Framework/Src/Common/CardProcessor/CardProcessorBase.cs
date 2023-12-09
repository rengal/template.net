using System;
using System.Collections.Generic;
using log4net;

namespace Resto.Framework.Common.CardProcessor
{
    using CardRollFailedHandler = EventHandler<EventArgs<object>>;

    public abstract class CardProcessorBase : ICardProcessor
    {
        public const int PopupPriority = 10;
        public const int BaseProirity = 100;
        public const int LowPriority = 1000;

        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(CardProcessorBase));
        private readonly List<Processor> processors = new List<Processor>();

        /// <summary>
        ///  Загружает из плагина все доступные процессоры,
        ///  проверяет включены ли они в конфигурации и если да - подключает
        /// </summary>
        /// <param name="externalProcessors"></param>
        /// <param name="cardReaderConfig">настройки считывателей, кроме клавиатурных и POS</param>
        /// <param name="readerDevicesConfig">настройки клавиатурны] и POS считывателей</param>
        public virtual void CreateKeyProcessor(IEnumerable<Processor> externalProcessors, ICardReaderConfig cardReaderConfig,
            IReaderDevicesConfig readerDevicesConfig)
        {
            Log.DebugFormat("CreateKeyProcessor called. Configured=[{0}]", cardReaderConfig.Configured);

            foreach (var processor in processors)
            {
                Log.DebugFormat("Unsubscribing processor [{0}]", processor.GetType());
                processor.CardRollSucceeded -= CardRolledHandler;
                processor.CardRollFailed -= CardRollFailedHandler;
                processor.CardRollStarted -= CardRollStartedHandler;
                processor.CardRollCompleted -= CardRollCompletedHandler;
            }

            processors.Clear();

            foreach (var processor in externalProcessors)
            {
                var active = processor.CheckIsActive(cardReaderConfig, readerDevicesConfig);
                Log.DebugFormat("New processor [{0}] is active: [{1}]", processor.GetType(), active);
                if (active)
                {
                    processors.Add(processor);

                    Log.DebugFormat("Subscribing processor [{0}]", processor.GetType());
                    try
                    {
                        processor.Configurate(cardReaderConfig, readerDevicesConfig);
                        processor.CardRollSucceeded += CardRolledHandler;
                        processor.CardRollFailed += CardRollFailedHandler;
                        processor.CardRollStarted += CardRollStartedHandler;
                        processor.CardRollCompleted += CardRollCompletedHandler;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Failed to configure processor", ex);
                    }
                }
            }
        }

        protected abstract void CardRolledHandler(object sender, EventArgs<MagnetTrackData> e);

        protected virtual void CardRollCompletedHandler(object sender, EventArgs e) { }
        protected virtual void CardRollStartedHandler(object sender, EventArgs e) { }
        protected virtual void CardRollFailedHandler(object sender, EventArgs<object> e) { }

        public abstract void AddCardRolledHandler(EventHandler<CardRolledEventArgs> cardRolledHandler, string type);
        public abstract void AddCardRolledHandler(EventHandler<CardRolledEventArgs> cardRolledHandler, CardRollFailedHandler errorHandler, string type);
        public abstract void AddCardRolledHandler(EventHandler<CardRolledEventArgs> cardRolledHandler, CardRollFailedHandler failedHandler, string type, int priority);
        public abstract void RemoveCardRolledHandler(EventHandler<CardRolledEventArgs> cardRolledHandler);

        public void Attach()
        {
            foreach (var processor in processors)
            {
                Log.DebugFormat("Attaching processor [{0}]", processor.GetType());
                try
                {
                    processor.Attach();
                    processor.Enabled = true;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error in attaching processor[{processor.GetType()}]", ex);
                }
            }
        }

        public void Detach()
        {
            foreach (var processor in processors)
            {
                Log.DebugFormat("Detaching processor [{0}]", processor.GetType());
                try
                {
                    processor.Detach();
                    processor.Enabled = false;
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Error in detaching processor[{0}]", processor.GetType()), ex);
                }
            }
        }

        public abstract IDisposable CreateProcessorPriorityThresholdHolder(int priorityThreshold);
    }
}