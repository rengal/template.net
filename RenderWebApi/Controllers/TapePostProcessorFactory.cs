using Resto.Data;
using Resto.Framework.Common.Print.VirtualTape.PostProcess;
using System.Diagnostics;

namespace RenderWebApi.Controllers
{
    internal class TapePostProcessorFactory
    {
        /// <summary>
        /// Создать постпроцессор для указанного драйвера принтера
        /// </summary>
        /// <param name="driver">Драйвер принтера</param>
        /// <returns>Ссылку на созданный постпроцессор</returns>
        public ITapePostProcessor CreatePostProcessor(PrinterDriver driver)
        {
            // создаем 
            var baseTapeProcessor = InternalCreatePostProcessor(driver);
            Debug.Assert(baseTapeProcessor != null);
            // Инициализируем основные значения по умолчанию
            InitBaseTapePostProcessor(baseTapeProcessor, driver);
            return baseTapeProcessor;
        }

        /// <summary>
        /// Вернуть обработкич по-умолчанию
        /// </summary>
        private BaseTapePostProcessor ReceiptProcessor => receiptProcessor ??= new DefaultTapePostProcessor();

        private BaseTapePostProcessor LabelProcessor => labelProcessor ??= new Epl2TapePostProcessor();

        /// <summary>
        /// Создает обработчик без инициализации
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        private BaseTapePostProcessor InternalCreatePostProcessor(PrinterDriver driver)
        {
            return driver switch
            {
                ZebraEplDriver => LabelProcessor,
                EpsonT88Driver => ReceiptProcessor,
                _ => ReceiptProcessor
            };
        }

        /// <summary>
        /// Инициализировать основные параметры обработчика 
        /// </summary>
        /// <param name="baseTapeProcessor">ПостОбработчик</param>
        /// <param name="driver">Драйвер принтера</param>
        private static void InitBaseTapePostProcessor(BaseTapePostProcessor baseTapeProcessor, PrinterDriver driver)
        {
            baseTapeProcessor.PagecutEsc = driver.PagecutEsc;
            baseTapeProcessor.StartEsc = driver.StartEsc;
            baseTapeProcessor.BellEsc = driver.BellEsc;

            baseTapeProcessor.EmptyLinesEsc = driver.EmptyLinesEsc;
            baseTapeProcessor.LinesOnPageParam = driver.LinesOnPageParam;
            baseTapeProcessor.EmptyLinesParam = driver.EmptyLinesParam;
            baseTapeProcessor.HasLinesParams = driver.HasLinesParams;

            if (baseTapeProcessor is Epl2TapePostProcessor epl2Handler)
            {
                var zebraEplDriver = driver as ZebraEplDriver;
                Debug.Assert(zebraEplDriver != null);
                epl2Handler.LabelWidth = zebraEplDriver.LabelWidth;
                epl2Handler.LabelHeight = zebraEplDriver.LabelHeight;
                epl2Handler.LabelMarginHor = zebraEplDriver.LabelMarginHor;
                epl2Handler.LabelMarginVert = zebraEplDriver.LabelMarginVert;
                epl2Handler.Font0Number = zebraEplDriver.Font0Number;
                epl2Handler.Font1Number = zebraEplDriver.Font1Number;
                epl2Handler.Font2Number = zebraEplDriver.Font2Number;
                epl2Handler.Font0Height = zebraEplDriver.Font0CharHeight;
                epl2Handler.Font1Height = zebraEplDriver.Font1CharHeight;
                epl2Handler.Font2Height = zebraEplDriver.Font2CharHeight;
                epl2Handler.BarcodeMargin = zebraEplDriver.BarcodeMarginVert;
                epl2Handler.PrinterEncoding = zebraEplDriver.Encoding;
                epl2Handler.Dpi = zebraEplDriver.Dpi;
            }
        }

        private BaseTapePostProcessor receiptProcessor;
        private BaseTapePostProcessor labelProcessor;
    }
}
