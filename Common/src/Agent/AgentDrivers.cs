using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using log4net;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Common.Helpers;
using Resto.Framework.Data;
using Resto.Framework.Text;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using System.Linq;
using System.Globalization;

namespace Resto.Data
{
    /// <summary>
    /// Метод обратного вызова, сигнализирующий о том, что агент завершил выполнение задания
    /// </summary>
    /// <param name="agentResult">Результат выполнения задания</param>
    public delegate void AgentCallBack(AgentPostResult agentResult);

    /// <summary>
    /// Тип драйвера устройства.
    /// </summary>
    [Flags]
    public enum AgentDriverType
    {
        /// <summary>
        /// Виртуальное устройство.
        /// Не привязано к агенту.
        /// </summary>
        Virtual,
        /// <summary>
        /// Агентское устройство.
        /// Привязано к агенту. Создается и работает на агенте.
        /// Может быть запущено и остановлено.
        /// </summary>
        Agent,
        /// <summary>
        /// Фронтовое устройство.
        /// Аналог виртуального устройства, но привязано к агенту.
        /// В Агенте не создается и не обрабатывается.
        /// </summary>
        Front
    }

    #region AgentDriver Interface

    public partial interface AgentDriver
    {
        #region Properties

        /// <summary>
        /// Gets driver name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets driver description string.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets agent model
        /// </summary>
        string AgentModel { get; }

        /// <summary>
        /// Gets driver type.
        /// </summary>
        AgentDriverType DriverType { get; }

        /// <summary>
        /// Gets driver obsolete flag
        /// </summary>
        bool IsObsolete { get; }

        #endregion
    }

    public static class AgentDriverExtensions
    {
        public static bool IsNullDriver(this AgentDriver agentDriver)
        {
            return agentDriver is ChequePrinterNullDriver || agentDriver is PrinterNullDriver ||
                   agentDriver is ScaleNullDriver;
        }
    }

    public static class AgentDriverHelper
    {
        private static readonly LogWrapper logWrapper = new LogWrapper(typeof(AgentDriverHelper));

        /// <summary>
        /// Виртуальное устройство без привязки к Фронту.
        /// </summary>
        public static bool IsPureVirtual(this AgentDriver driver)
        {
            return driver.DriverType == AgentDriverType.Virtual;
        }

        public static bool IsVirtual(this AgentDriver driver)
        {
            return driver.DriverType != AgentDriverType.Agent;
        }

        public static bool IsStartable(this AgentDriver driver)
        {
            return driver.DriverType == AgentDriverType.Agent;
        }

        public static int GetWideFontWidth(int font0Width)
        {
            return font0Width == 1 ? 1 : font0Width / 2;
        }

        public static byte GetBarcodeHeightOrDefault(string barcode, string heightRatioStr)
        {
            if (!double.TryParse(heightRatioStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var heightRatio))
                heightRatio = 0.3;
            var widthInDots = ImageHelper.CalcEan13BarCodeWidth(barcode, 3);
            var heightValue = (int)widthInDots * heightRatio;

            if (heightValue > byte.MaxValue)
            {
                logWrapper.Log.Warn($"Barcode height of {heightValue} exceededs the maximum value of {byte.MaxValue}. " +
                    $"The height has been trimmed to {byte.MaxValue}");
                return byte.MaxValue;
            }
            return (byte)heightValue;
        }

    }

    #endregion AgentDriver Interface

    #region ICharCount Interface

    public interface ICharCount
    {
        int CharCount { get; }
    }

    #endregion ICharCount Interface

    #region TerminalDriver Class

    public partial class TerminalDriver
    {
        public string Name => string.Empty;

        public string Description => string.Empty;

        public string AgentModel => "TerminalDriver";

        public AgentDriverType DriverType => AgentDriverType.Front;

        public bool IsObsolete => false;
    }

    #endregion TerminalDriver Class

    #region CameraDriver Class

    public partial class CameraDriver
    {
        public string Name => string.Empty;

        public string Description => Resources.CameraDriverDescription;

        public string AgentModel => "VideoDriver";

        public AgentDriverType DriverType => AgentDriverType.Agent;

        public bool IsObsolete => false;
    }

    #endregion CameraDriver Class

    #region CardProcessingDriver Class

    public partial class CardProcessingDriver
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string AgentModel { get; }

        public abstract AgentDriverType DriverType { get; }

        public bool IsObsolete => false;
    }

    #endregion CardProcessingDriver Class

    #region PlastekDriver Class

    public partial class PlastekDriver
    {
        public override string Name => Resources.PlastekDriverName;

        public override string Description => Resources.PlastekDriverDescription;

        public override string AgentModel => "PlastekDriver";

        public override AgentDriverType DriverType => AgentDriverType.Agent;
    }

    public partial class VirtualPlastekDriver
    {
        public override AgentDriverType DriverType => AgentDriverType.Virtual;
    }

    #endregion PlastekDriver Class

    #region PulsarDriver Class

    public partial class PulsarDriver
    {
        public override string Name => Resources.PulsarDriverName;

        public override string Description => Resources.PulsarDriverDescription;

        public override string AgentModel => "PulsarDriver";

        public override AgentDriverType DriverType => AgentDriverType.Agent;
    }

    #endregion PulsarDriver Class

    #region HoistDriver Class

    public partial class HoistDriver
    {
        public override string Name => Resources.HoistDriverName;

        public override string Description => Resources.HoistDriverName;

        public override string AgentModel => "HoistDriver";

        public override AgentDriverType DriverType => AgentDriverType.Virtual;
    }

    #endregion HoistDriver Class

    #region EdelweissDriver Class

    public partial class EdelweissDriver
    {
        public override string Name => Resources.EdelweissDriverName;

        public override string Description => Resources.EdelweissDriverName;

        public override string AgentModel => "EdelweissDriver";

        public override AgentDriverType DriverType => AgentDriverType.Virtual;
    }

    #endregion HoistDriver Class

    #region EpitomeDriver Class

    public partial class EpitomeDriver
    {
        public override string Name => Resources.EpitomeDriverName;

        public override string Description => Resources.EpitomeDriverName;

        public override string AgentModel => "EpitomeDriver";

        public override AgentDriverType DriverType => AgentDriverType.Virtual;
    }

    #endregion HoistDriver Class

    #region TrposDriver Class

    public partial class TrposDriver
    {
        public override string Name => Resources.TrposDriverName;

        public override string Description => Resources.TrposDriverName;

        public override string AgentModel => "TrposDriver";

        public override AgentDriverType DriverType => AgentDriverType.Virtual;
    }

    #endregion TrposDriver Class

    #region SmartSaleDriver Class

    public partial class SmartSaleDriver
    {
        public const string SmartSalePaymentSystemName = "smartsale";

        public override string Name => Resources.SmartSaleDriverName;

        public override string Description => Resources.SmartSaleDriverName;

        public override string AgentModel => "SmartSaleDriver";

        public override AgentDriverType DriverType => AgentDriverType.Front;
    }

    #endregion SmartSaleDriver Class

    #region CustomerDisplayDriver Class

    public partial class CustomerDisplayDriver
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string AgentModel { get; }

        public AgentDriverType DriverType => AgentDriverType.Agent;

        public bool IsObsolete => false;
    }

    #endregion CustomerDisplayDriver Class

    #region FV2029Driver Class

    public partial class FV2029Driver
    {
        public override string Name => "Firich/Epson";

        public override string AgentModel => "FV2029Driver";

        public override string Description => Resources.FV2029DriverDescription;
    }

    #endregion FV2029Driver Class

    #region OposCustomerDisplayDriver Class

    public partial class OposCustomerDisplayDriver
    {
        public override string Name => "OPOS Display";

        public override string AgentModel => "OposCustomerDisplay";

        public override string Description => Resources.OposCustomerDisplayDriverDescription;
    }

    #endregion OposCustomerDisplayDriver Class

    #region PrinterDriver Class

    public abstract partial class PrinterDriver
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string AgentModel { get; }

        public virtual AgentDriverType DriverType => AgentDriverType.Agent;

        public virtual bool IsObsolete => false;

        public virtual bool CanPrintBarCode => false;

        public virtual bool CanPrintLogo => false;

        public virtual bool CanPrintQRCode => false;

        public virtual bool CanPrintImages => false;

        public virtual string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return string.Empty;
        }
        
        public virtual string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return string.Empty;
        }

        public virtual string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            return string.Empty;
        }

        public virtual string ConvertToPulse()
        {
            return string.Empty;
        }

        public virtual string GetInitializationEsc(Encoding encoding)
        {
            return string.Empty;
        }

        /// <summary>
        /// Заменить символы в строке, не поддерживаемые принтером, на их аналоги
        /// </summary>
        /// <param name="st">Исходная строка</param>
        /// <returns>Преобразованная строка</returns>
        public virtual string ReplaceUnsupportedChars(string st)
        {
            //Заменить неразрывный пробел на обычный пробел
            return st.Replace("\u00A0", " ");
        }

        public virtual string ConvertToLogo(string logoId)
        {
            return CanPrintLogo ? Convert.ToChar(Convert.ToInt32(logoId)) + LogoEndEsc : string.Empty;
        }

        protected string ConvertToBarcodeXml(string text, string align, string heightRatio, string hri)
        {
            var alignAttribute = string.IsNullOrEmpty(align) ? string.Empty : $" align=\"{SecurityElement.Escape(align)}\"";
            var heightAttribute = string.IsNullOrEmpty(heightRatio) ? string.Empty : $" heightRatio=\"{SecurityElement.Escape(heightRatio)}\"";
            var hriAttribute = string.IsNullOrEmpty(hri) ? string.Empty : $" hri=\"{SecurityElement.Escape(hri)}\"";

            return $"<barcode{alignAttribute}{heightAttribute}{hriAttribute}>{SecurityElement.Escape(text)}</barcode>";
        }

        protected string ConvertToQRCodeXml(string text, string align, string size, string correction)
        {
            var alignAttribute = string.IsNullOrEmpty(align) ? string.Empty : $" align=\"{SecurityElement.Escape(align)}\"";
            var sizeAttribute = string.IsNullOrEmpty(size) ? string.Empty : $" size=\"{SecurityElement.Escape(size)}\"";
            var correctionAttribute = string.IsNullOrEmpty(correction) ? string.Empty : $" correction=\"{SecurityElement.Escape(correction)}\"";
            return $"<qrcode{alignAttribute}{sizeAttribute}{correctionAttribute}>{SecurityElement.Escape(text)}</qrcode>";
        }

        protected string ConvertToImageXml(string base64Image, string align, string resizeMode)
        {
            var alignAttribute = string.IsNullOrEmpty(align) ? string.Empty : $" align=\"{SecurityElement.Escape(align)}\"";
            var resizeModeAttribute = string.IsNullOrEmpty(resizeMode) ? string.Empty : $" resizeMode=\"{SecurityElement.Escape(resizeMode)}\"";

            return $"<image{alignAttribute}{resizeModeAttribute}>{SecurityElement.Escape(base64Image)}</image>";
        }


        #region Поддержка печати в C#
        // не удалять и не переносить в ресурсы, пока не будет сделан RMS-22237
        protected const string Papercut = "\n-------------разрез принтера----------\n";

        [Transient]
        protected int font0Width;
        [Transient]
        protected int font1Width;
        [Transient]
        protected int font2Width;
        [Transient]
        protected string font0Esc = string.Empty;
        [Transient]
        protected string font1Esc = string.Empty;
        [Transient]
        protected string font2Esc = string.Empty;
        [Transient]
        protected string pagecutEsc = string.Empty;
        [Transient]
        protected string startEsc = string.Empty;
        [Transient]
        protected string bellEsc = string.Empty;
        [Transient]
        protected string emptyLinesEsc = string.Empty;
        [Transient]
        protected string logoBeginEsc = string.Empty;
        [Transient]
        protected string logoEndEsc = string.Empty;
        [Transient]
        protected string boldBeginEsc = string.Empty;
        [Transient]
        protected string boldEndEsc = string.Empty;
        [Transient]
        protected string italicBeginEsc = string.Empty;
        [Transient]
        protected string italicEndEsc = string.Empty;
        [Transient]
        protected string reverseBeginEsc = string.Empty;
        [Transient]
        protected string reverseEndEsc = string.Empty;
        [Transient]
        protected string underlineBeginEsc = string.Empty;
        [Transient]
        protected string underlineEndEsc = string.Empty;

        private static readonly LogWrapper logWrapper = new LogWrapper(typeof(PrinterDriver));

        protected static ILog LOG => logWrapper.Log;

        public virtual int Font0Width => font0Width;

        public virtual int Font1Width => font1Width;

        public virtual int Font2Width => font2Width;

        public virtual string Font0Esc => font0Esc;

        public virtual string Font1Esc => font1Esc;

        public virtual string Font2Esc => font2Esc;

        public virtual string PagecutEsc => pagecutEsc;

        public virtual string StartEsc => startEsc;

        public virtual string BellEsc => bellEsc;

        public virtual string EmptyLinesEsc => emptyLinesEsc;

        public virtual int LinesOnPageParam => -1;

        public virtual int EmptyLinesParam => -1;

        public bool HasLinesParams => LinesOnPageParam > 0 && EmptyLinesParam >= 0;

        public virtual string LogoBeginEsc => logoBeginEsc;

        public virtual string LogoEndEsc => logoEndEsc;

        public abstract void InitPrintParams(int? baseWidth);

        public virtual string BoldBeginEsc => boldBeginEsc;

        public virtual string BoldEndEsc => boldEndEsc;

        public virtual string ItalicBeginEsc => italicBeginEsc;

        public virtual string ItalicEndEsc => italicEndEsc;

        public virtual string ReverseBeginEsc => reverseBeginEsc;

        public virtual string ReverseEndEsc => reverseEndEsc;

        public virtual string UnderlineBeginEsc => underlineBeginEsc;

        public virtual string UnderlineEndEsc => underlineEndEsc;
        #endregion
    }

    #endregion PrinterDriver Class

    #region PortWriterDriver Class

    public partial class PortWriterDriver
    {
        public override string AgentModel => "PortWriterDriver";

        #region Поддержка печати в C#

        public static readonly Encoding BASE64_CHARSET_ENCODING = Encoding.GetEncoding("cp866");

        public string CodePageSafe => string.IsNullOrEmpty(codePage) ? "cp866" : codePage;

        public string CodePagePrimary
        {
            get
            {
                var pos = CodePageSafe.IndexOf(':');
                return pos > 0 ? CodePageSafe.Substring(0, pos) : CodePageSafe;
            }
        }

        public string CodePageSecondary
        {
            get
            {
                var pos = CodePage.IsNullOrEmpty() ? -1 : CodePage.IndexOf(':');
                return pos > 0 ? CodePage.Substring(pos + 1, CodePage.Length - pos - 1) : CodePage;
            }
        }

        public virtual Encoding Encoding => EncodingsManager.GetEncoding(CodePagePrimary, BASE64_CHARSET_ENCODING);

        public virtual Encoding EncodingForFormatting => EncodingsManager.GetEncoding(CodePageSecondary, BASE64_CHARSET_ENCODING);

        #endregion
    }

    #endregion

    #region AtolDriver Class

    public partial class AtolDriver : ICharCount
    {
        public override string Name => Resources.AtolDriverName;

        public override string AgentModel => "AtolDriver";

        public override bool CanUseFontSizes => true;

        public override string Description => Resources.AtolDriverDescriptionFiscalRegistrar;

        public override bool CanPrint => true;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintQRCode => true;

        public int CharCount => 30;

        #region Поддержка печати в C#

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override bool ZeroCashOnClose => true;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = width;//вообще-то должно зависеть от модели ФР, но автоперенос поддерживается
                font2Width = width;
            }
            else
            {
                font0Width = 30;
                font1Width = 30;
                font2Width = 30;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion AtolDriver Class

    #region IcsDriver Class

    public partial class IcsDriver : ICharCount
    {
        public override string Name => Resources.IcsDriverName;

        public override string AgentModel => "IcsDriver";

        public override bool CanUseFontSizes => false;

        public override string Description => Resources.IcsDriverDescriptionFiscalRegistrar;

        public override bool CanPrint => true;

        public int CharCount => 28;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => true;

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 28;
            font1Width = 28;
            font2Width = 28;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion IcsDriver Class

    #region IksSpsDriver Class

    public partial class IksSpsDriver : ICharCount
    {
        public override string Name => Resources.IksSpsDriverName;

        public override string AgentModel => "IksSpsDriver";

        public override bool CanUseFontSizes => false;

        public override string Description => Resources.IksSpsDriverDescriptionFiscalRegister;

        public override bool CanPrint => true;

        public int CharCount => 39;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => true;

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 39;
            font1Width = 39;
            font2Width = 39;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion IksSpsDriver Class

    #region PosnetDriver Class

    public partial class PosnetDriver : ICharCount
    {
        public override string Name => Resources.PosnetDriverName;

        public override string AgentModel => "PosnetDriver";

        public override bool CanUseFontSizes => true;

        public override string Description => Resources.PosnetDriverDescriptionFiscalRegister;

        public override bool CanPrint => true;

        public override bool CanPrintQRCode => true;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override string ConvertToLogo(string logoId)
        {
            return $"<logo>{SecurityElement.Escape(logoId)}</logo>";
        }

        public int CharCount => 50;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = width / 2;
                font2Width = width / 2;
            }
            else
            {
                font0Width = 42;
                font1Width = 21;
                font2Width = 21;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion PosnetDriver Class

    #region PosnetThermalDriver Class

    public partial class PosnetThermalDriver : ICharCount
    {
        public override string Name => Resources.PosnetThermalDriverName;

        public override string AgentModel => "PosnetThermalDriver";

        public override bool CanUseFontSizes => false;

        public override string Description => Resources.PosnetThermalDriverDescriptionFiscalRegister;

        public override bool CanPrint => false;

        public int CharCount => 50;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion PosnetThermalDriver Class

    #region ExternalCashRegisterDriver Class

    /// <summary>
    /// Заменяется <seealso cref="Resto.CashServer.Data.Entities.CashServerEntities.CashServerExternalCashRegisterDriver"/>. 
    /// </summary>
    public partial class ExternalCashRegisterDriver
    {
        public override string Name => Resources.ExternalCashRegisterDriverName;

        public override string AgentModel => "ExternalCashRegisterDriver";

        public override string Description => Resources.ExternalCashRegisterDriverName;

        public override bool CanPrint => true;

        public override bool CanPrintLogo => true;

        public override void InitPrintParams(int? baseWidth)
        { }

        public override string ConvertToLogo(string logoId)
        {
            return $"<logo>{SecurityElement.Escape(logoId)}</logo>";
        }

    }

    #endregion ExternalCashRegisterDriver Class

    #region HrsDriver Class

    /// <summary>
    /// RMS-48487 Front.Plugins: Перевести плагин HRS на API.v6
    /// Плагин устарел, и более не актуален, пользователи этого c++-устройства должны вручную перенастроиться на использование плагина Resto.Front.Api.Hrs
    /// </summary>
    [Obsolete]
    public partial class HrsDriver : ICharCount
    {
        public override string Name => Resources.HrsDriverName;

        public override string AgentModel => "HrsDriver";

        public override bool CanUseFontSizes => false;

        public override string Description => Resources.HrsDriverDescriptionFiscalRegister;

        public override bool CanPrint => true;

        public int CharCount => font0Width;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => true;

        public override void InitPrintParams(int? baseWidth)
        {
        }

        #endregion
    }

    #endregion HrsDriver Class

    #region BrioFisFm32Driver Class

    public partial class BrioFisFm32Driver : ICharCount
    {
        public override string Name => Resources.BrioFisFm32DriverName;

        public override string AgentModel => "BrioFisFm32Driver";

        public override bool CanUseFontSizes => true;

        public override string Description => Resources.BrioFisFm32DriverDescriptionFiscalRegister;

        public override bool CanPrint => true;

        public int CharCount => 42;

        public override bool IsBillTaskSupported => true;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = AgentDriverHelper.GetWideFontWidth(width);
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = 42;
                font1Width = 21;
                font2Width = 21;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
        }

        #endregion
    }

    #endregion BrioFisFm32Driver Class

    #region AlposAf01Driver Class

    public partial class AlposAf01Driver : ICharCount
    {
        public override string Name => Resources.AlposAf01DriverName;

        public override string AgentModel => "AlposAf01Driver";

        public override bool CanUseFontSizes => true;

        public override string Description => Resources.AlposAf01DriverDescriptionFiscalRegister;

        public override bool CanPrint => true;

        public int CharCount => 42;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = AgentDriverHelper.GetWideFontWidth(width);
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = 42;
                font1Width = 21;
                font2Width = 21;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
        }

        public override bool IsBillTaskSupported => true;

        public override bool IsRegisterStatusSupported => true;

        #endregion
    }

    #endregion AlposAf01Driver Class

    #region PiritDriver Class

    public partial class PiritDriver : ICharCount
    {
        public override string AgentModel => "PiritDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.PiritDriverName;

        public override string Description => Resources.PiritDriverDescriptionFiscalRegister;

        public override bool CanUseFontSizes => true;

        public int CharCount => 44;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintQRCode => true;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = AgentDriverHelper.GetWideFontWidth(width);
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = CharCount;
                font1Width = AgentDriverHelper.GetWideFontWidth(CharCount);
                font2Width = AgentDriverHelper.GetWideFontWidth(CharCount);
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        #endregion
    }

    #endregion PiritDriver Class

    #region PiritFnDriver Class

    public partial class PiritFnDriver : ICharCount
    {
        public override string AgentModel => "PiritFnDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.PiritFnDriverName;

        public override string Description => Resources.PiritFnDriverDescriptionFiscalRegister;

        public override bool CanUseFontSizes => true;

        public int CharCount => 44;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintQRCode => true;

        #region C# Printing support

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = AgentDriverHelper.GetWideFontWidth(width);
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = CharCount;
                font1Width = AgentDriverHelper.GetWideFontWidth(CharCount);
                font2Width = AgentDriverHelper.GetWideFontWidth(CharCount);
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        #endregion
    }

    #endregion PiritFnDriver Class

    #region EmpirijaDriver Class

    public partial class EmpirijaDriver : ICharCount
    {
        public override string AgentModel => "EmpirijaDriver";

        public override string Name => Resources.EmpirijaDriverName;

        public override bool CanUseFontSizes => true;

        public override string Description => Resources.EmpirijaDriverDescriptionFiscalRegistrar;

        public override bool CanPrint => true;

        public int CharCount => 50;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => true;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = width;
                font2Width = width;
            }
            else
            {
                font0Width = 50;
                font1Width = 50;
                font2Width = 50;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion EmpirijaDriver Class

    #region OPOSRegisterDriver Class

    public partial class OPOSRegisterDriver : ICharCount
    {
        public override string AgentModel => "OPOSRegisterDriver";

        public override string Name => Resources.OPOSRegisterDriverName;

        public override bool CanUseFontSizes => true;

        public override string Description => Resources.OPOSRegisterDriverDescriptionFiscalRegistrar;

        public override bool CanPrint => true;

        public int CharCount => 50;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = width;
                font2Width = width;
            }
            else
            {
                font0Width = 50;
                font1Width = 50;
                font2Width = 50;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion OPOSRegisterDriver Class

    #region UnisystemDriver Class

    public partial class UnisystemDriver : ICharCount
    {
        public override string Name => Resources.UnisystemDriverNameMIHI;

        public override string AgentModel => "UnisystemDriver";

        public override string Description => Resources.UnisystemDriverDescriptionFiscalRegistrarMIHI;

        public override bool CanPrint => true;

        public int CharCount => 30;

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 30;
            font1Width = 30;
            font2Width = 30;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion UnisystemDriver Class

    public interface IVirtualCashRegisterDriver
    {
        IPrinterDevice PrinterDevice { get; }
    }

    #region ChequePrinterStub Class

    /// <summary>
    /// Класс для поддержки виртуальной кассы.
    /// </summary>
    /// <remarks>
    /// Используется для работы с 2 ФР-ностью если тип <see cref="AgentDriverType.Front"/>. 
    /// Используется для работы с внешней кассой если тип <see cref="AgentDriverType.Virtual"/>. 
    /// </remarks>
    public partial class ChequePrinterStub : IVirtualCashRegisterDriver
    {
        public override string Name => Description;

        public override string AgentModel => "ChequePrinterStub";

        public override string Description => isVirtual ?
            Resources.ChequePrinterStubExternalVirtualCashNonFiscal :
            Resources.ChequePrinterStubVirtualCashNonFiscal;

        public override AgentDriverType DriverType => isVirtual ? AgentDriverType.Virtual : AgentDriverType.Front;

        public override bool CanPrint => false;

        /// <summary>
        /// Обновляет тип и имя.
        /// </summary>
        public void UpdateType(AgentDriverType type)
        {
            isVirtual = type == AgentDriverType.Virtual;
        }

        #region Поддежка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
        }

        #endregion
    }

    #endregion ChequePrinterStub Class

    #region ChequePrinterDriver Class

    public partial class ChequePrinterDriver
    {
        public virtual bool CanPrint => false;

        public virtual bool ZeroCashOnClose => false;

        public override bool CanPrintBarCode => false;

        public virtual bool CanUseFontSizes => false;

        public virtual bool IsCancellationSupported => false;

        public virtual bool IsBillTaskSupported => false;

        public virtual bool NeedToCheckBillNumber => false;

        public virtual bool IsRegisterStatusSupported => false;

        public virtual bool PrintNonFiscalPrepayCheque => false;

        public virtual bool PrintDatailedChequeWithPrepay => false;

        public virtual bool IsCustomDuplicateCheckSupported => false;
    }

    #endregion ChequePrinterDriver Class

    #region ComstarDriver Class

    public partial class ComstarDriver
    {
        public override string Name => "Comstar";

        public override string Description => Resources.ComstarDriverDescription;

        public override bool CanPrintLogo => true;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 25;
            font1Width = 15;
            font2Width = 15;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 27, 87, 48, 27, 104, 48 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 27, 87, 49, 27, 104, 48 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 27, 87, 49, 27, 104, 49 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 27, 100, 51 });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 0x11 });
            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1C, 0x70 });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0 });
        }

        #endregion
    }

    #endregion ComstarDriver Class

    #region EpsonT88Driver Class

    public partial class EpsonT88Driver
    {
        public override string Name => "Epson TM-T88";

        public override string Description => Resources.EpsonT88DriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override bool CanPrintQRCode => true;

        public override bool CanPrintImages => true;

        protected virtual string ConvertToQRCode(string text, string align, byte nsize, string correction)
        {
            byte alignValue = align switch
            {
                "left" => 0,
                "center" => 1,
                "right" => 2,
                _ => 1
            };
            byte ncorrection = 49;
            if (string.Equals(correction, "low", StringComparison.OrdinalIgnoreCase))
                ncorrection = 48;
            else if (string.Equals(correction, "medium", StringComparison.OrdinalIgnoreCase))
                ncorrection = 49;
            else if (string.Equals(correction, "high", StringComparison.OrdinalIgnoreCase))
                ncorrection = 50;
            else if (string.Equals(correction, "ultra", StringComparison.OrdinalIgnoreCase))
                ncorrection = 51;

            var qrCodeEsc =
                BASE64_CHARSET_ENCODING.GetString(new byte[]
                    {
                        0x1b, 0x61, alignValue, // set alignment
                        29, 40, 107, 3, 0, 49, 65, 50, 0, //model 2
                        29, 40, 107, 3, 0, 49, 67, nsize, //size of code symbol //GS ( k (cn=49)
                        29, 40, 107, 3, 0, 49, 69, ncorrection, //error correction level
                        29, 40, 107, (byte)(text.Length + 3), (byte)((text.Length - 1)/256), 49, 80, 48
                    }); //store text data
            qrCodeEsc += text;
            qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[] { 29, 40, 107, 3, 0, 49, 81, 48 }); //print data

            if (alignValue != 0)
                qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x61, 0 }); // Set alignment (left)

            return qrCodeEsc;
        }

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            byte alignValue = align switch
            {
                "left" => 0,
                "center" => 1,
                "right" => 2,
                _ => 1
            };
            byte hriValue = hri switch
            {
                "on" => 2,
                "off" => 0,
                _ => 2
            };

            var heightValue = AgentDriverHelper.GetBarcodeHeightOrDefault(text, heightRatio);

            var beginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[]
            {
                0x1b, 0x21, 0, // Set Font A
                0x1b, 0x61, alignValue, // Set alignment
                0x1d, 0x68, heightValue, //Set barcode height
                0x1d, 0x77, 3, //Set narrow width
                0x1d, 0x48, hriValue, //Set hri
                0x1d, 0x6b, 67, 13
            });

            var endEsc = alignValue != 0
                ? BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x61, 0 }) // Set alignment (left)
                : string.Empty;

            return beginEsc + text + endEsc;
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            byte moduleSize = size switch
            {
                "tiny" => 2,
                "small" => 3,
                "normal" => 5,
                "large" => 7,
                "extralarge" => 8,
                _ => 5
            };

            return ConvertToQRCode(text, align, moduleSize, correction);
        }

        public override string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            var (bitmap, _) = ImageHelper.CreateBitmapFromBase64(base64Image, WidthInDots, null, align, resizeMode, true);
            var (imageData, widthInBytes) = ImageHelper.ImageToByteArray(bitmap, true);

            byte m = 0;

            //xL + xH*256 = ширина изображения в байтах
            var xL = (byte)(widthInBytes % 256);
            var xH = (byte)(widthInBytes / 256);

            //yL + yH*256 = ширина изображения в пикселях
            var yL = (byte)(bitmap.Height % 256);
            var yH = (byte)(bitmap.Height / 256);

            var cmd = new List<byte>
                {
                    0x1d, 0x76, 0x30, //GS v 0 - Print raster image
                    m, xL, xH, yL, yH
                }
                .Concat(imageData)
                .ToArray();

            return BASE64_CHARSET_ENCODING.GetString(cmd);
        }

        public override string ConvertToPulse()
        {
            if (string.IsNullOrEmpty(CodePage) || !CodePage.StartsWith("unicode", StringComparison.OrdinalIgnoreCase))
            {
                return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, (byte)PulsePort, 0x7F, 0x7F });
            }
            return string.Empty;
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 42;
            font1Width = 28;
            font2Width = 21;
            {
                font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0 });
                font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 49 });
                font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 48 });
                pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1d, 0x56, 66, 0 });
            }
            boldBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x45, 0x01 });
            boldEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x45, 0 });

            // Epson T-88 не поддерживает курсив
            italicBeginEsc = string.Empty;
            italicEndEsc = string.Empty;

            reverseBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1d, 0x42, 0x01 });
            reverseEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1d, 0x42, 0 });

            underlineBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x2d, 0x01 });
            underlineEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x2d, 0 });

            if (string.IsNullOrEmpty(CodePage) || !CodePage.StartsWith("unicode", StringComparison.OrdinalIgnoreCase))
                bellEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, 0, 127, 127 });
            else
                bellEsc = "";

            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1C, 0x70 });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0 });

            var codePageId = Encoding.CodePage;

            if (string.IsNullOrEmpty(CodePage) || codePageId == 866)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 17 });
            else if (codePageId == 857)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 18 }); //есть все символы алфавита кроме двух
            else if (codePageId == 852)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 18 });
            else if (codePageId == 1252)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 16 });
            else if (codePageId == 858)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 2 });
            else if (codePageId == 860)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 3 });
            else if (codePageId == 863)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 4 });
            else if (codePageId == 865)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 5 });
            else
                startEsc = "";
        }


        #endregion
    }

    #endregion

    #region EscPosPrinterDriver Class

    public partial class EscPosPrinterDriver
    {
        public override string Name => Resources.EscPosPrinterDriverDescription;

        public override string Description => Resources.EscPosPrinterDriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override bool CanPrintQRCode => true;

        public override bool CanPrintImages => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override string ConvertToLogo(string logoId)
        {
            return $"<logo>{SecurityElement.Escape(logoId)}</logo>";
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 42;
            font1Width = 42;
            font2Width = 21;
            pagecutEsc = "<pagecut/>";
        }

        public override string ConvertToPulse()
        {
            return "<pulse/>\n";
        }

        public override string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            return ConvertToImageXml(base64Image, align, resizeMode);
        }

        #endregion

    }

    #endregion

    #region EpsonTMU220Driver Class

    public partial class EpsonTMU220Driver
    {
        public override string Name => "Epson TM-U220";

        public override string Description => Resources.EpsonTMU220DriverDescription;

        public override bool CanPrintLogo => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[]
                { 0x1b, 0x21, 0, 0x1b, 0x28, 0x42, 18, 0, 0, 2, 0, 127, 0, 4 });
            return beginEsc + text;
        }

        public override string ConvertToPulse()
        {
            return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, (byte)PulsePort, 0x7F, 0x7F });
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            var isGb2312 = string.Equals(CodePageSafe, "gb2312", StringComparison.OrdinalIgnoreCase);

            // Workaround для китайского принтера GP-5890XIII а рамках RMS-16752 (Добавить настройку кодовой страницы для принтеров)
            // Данный принтер схож по характеристикам с принтером Epson TM-U220 у которого примерно такая же ширина чека.
            if (isGb2312)
            {
                // Количество символов в строке подсчитано исходя из того что ширина китайских иероглифов в 2 раза больше 
                // латинских символов. Учитывая что текст в строке может содержать и китайские и латинские символы форматируем 
                // относительно латинских
                font0Width = 2 * 16;
                font1Width = 2 * 16;
                font2Width = 2 * 8;
                // Стартовая последовательность без установки кодировки Кирилица (cp866)
                // Китайские символы печатаются в кодировке gb2312 которая поддерживается по умолчанию. Латинские 
                // символы являются подмножеством данной кодировки.
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0 });
                // В конце чека прокатываем бумагу на 0x3 строк. Обрезчика у принтера нет
                pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x64, 0x3 });
            }
            else
            {
                font0Width = 30;
                font1Width = 30;
                font2Width = 15;
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 17 });
                pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1d, 0x56, 66, 0 });
            }

            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 16 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 48 });
            bellEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, 0, 127, 127 });

            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1C, 0x70 });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0 });
        }

        #endregion
    }

    #endregion EpsonTMU220Driver Class

    #region EpsonTMU295Driver Class

    public partial class EpsonTMU295Driver
    {
        public override string Name => "Epson TM-U295";

        public override string Description => Resources.EpsonTMU295DriverDescription;

        public override bool CanPrintBarCode => false;

        public override bool CanPrintLogo => false;

        public override bool CanPrintQRCode => false;

        public override string ConvertToPulse()
        {
            return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, (byte)PulsePort, 0x7F, 0x7F });
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 35;
            font1Width = 21;
            font2Width = 14;
            {
                font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0 });
                font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 49 });
                font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 48 });
                pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1d, 0x56, 66, 0 });
                bellEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, 0, 127, 127 });
                emptyLinesEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x64 });
            }

            var codePageId = Encoding.CodePage;

            if (string.IsNullOrEmpty(CodePage) || codePageId == 866)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x63, 0x34, 0x30, 0x1b, 0x74, 17 });
            else if (codePageId == 857)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x63, 0x34, 0x30, 0x1b, 0x74, 18 }); //есть все символы алфавита кроме двух
            else if (codePageId == 852)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x63, 0x34, 0x30, 0x1b, 0x74, 18 });
            else if (codePageId == 1252)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x63, 0x34, 0x30, 0x1b, 0x74, 16 });
            else if (codePageId == 858)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x63, 0x34, 0x30, 0x1b, 0x74, 2 });
            else if (codePageId == 860)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x63, 0x34, 0x30, 0x1b, 0x74, 3 });
            else if (codePageId == 863)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x63, 0x34, 0x30, 0x1b, 0x74, 4 });
            else if (codePageId == 865)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x63, 0x34, 0x30, 0x1b, 0x74, 5 });
            else
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x63, 0x34, 0x30 });
        }

        public override int LinesOnPageParam => LinesOnPage;

        public override int EmptyLinesParam => EmptyLines;

        #endregion
    }

    #endregion EpsonTMU295Driver Class

    #region BtpR580IIDriver Class

    [Obsolete("Use CheckwayDriver instead")]
    public partial class BtpR580IIDriver
    {
        public override string Name => "BTP-R580II";

        public override string Description => Resources.BtpR580IIDriverDescription;

        public override bool IsObsolete => true;

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return CheckwayDriver.ConvertToQRCodePrint3000(text, align, size, correction);
        }
    }

    #endregion BtpR580IIDriver Class

    #region CheckwayProduct Class

    public partial class CheckwayProduct
    {
        public static CheckwayProduct ParseOrDefault(string value)
        {
            try
            {
                return Parse(value);
            }
            catch (ArgumentException)
            {
                return PRINT1000;
            }
        }

        public static string GetValueOrDefault(object value)
        {
            return value is CheckwayProduct checkwayProduct ? checkwayProduct._Value : PRINT1000._Value;
        }

        public static string GetValueOrDefault(CheckwayProduct checkwayProduct)
        {
            return checkwayProduct != null ? checkwayProduct._Value : PRINT1000._Value;
        }

        public static string GetValueOrDefault(string value)
        {
            return ParseOrDefault(value)._Value;
        }
    }

    #endregion CheckwayProduct Class

    #region CheckwayDriver Class

    public partial class CheckwayDriver
    {
        public override string Name => "Checkway";

        public override string Description => Resources.CheckwayDriverDescription;

        private static string ConvertToQRCodePrint1000(string text, string align, string size, string correction)
        {
            byte alignValue = align switch
            {
                "left" => 0,
                "center" => 1,
                "right" => 2,
                _ => 1
            };
            byte unitSize = 10;
            byte correctionLevel = 48;
            if (string.Equals(size, "tiny", StringComparison.OrdinalIgnoreCase))
                unitSize = 5;
            else if (string.Equals(size, "small", StringComparison.OrdinalIgnoreCase))
                unitSize = 6;
            else if (string.Equals(size, "normal", StringComparison.OrdinalIgnoreCase))
                unitSize = 8;
            else if (string.Equals(size, "large", StringComparison.OrdinalIgnoreCase))
                unitSize = 10;
            else if (string.Equals(size, "extralarge", StringComparison.OrdinalIgnoreCase))
                unitSize = 11;

            if (string.Equals(correction, "low", StringComparison.OrdinalIgnoreCase))
                correctionLevel = 48;
            else if (string.Equals(correction, "medium", StringComparison.OrdinalIgnoreCase))
                correctionLevel = 49;
            else if (string.Equals(correction, "high", StringComparison.OrdinalIgnoreCase))
                correctionLevel = 50;
            else if (string.Equals(correction, "ultra", StringComparison.OrdinalIgnoreCase))
                correctionLevel = 51;


            var qrCodeEsc =

                // Command 0x61 - Set alignment
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1B, 0x61, alignValue }) +

                //Command 0x67 - Set unit size
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1D, 0x28, 0x6B, 0x30, 0x67, unitSize }) + //GS ( k 0

                //Command 0x69 - Set up error correction level
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1D, 0x28, 0x6B, 0x30, 0x69, correctionLevel }) +

                //Command 0x80 - Data transmission to coding cache
                BASE64_CHARSET_ENCODING.GetString(new byte[]
                {
                    0x1D, 0x28, 0x6B, 0x30, 0x80,
                    (byte)(text.Length & 0xFF), (byte)((text.Length >> 8) & 0xFF)
                }) + text +

                //Command 0x81 - Print data in coding cache
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1D, 0x28, 0x6B, 0x30, 0x81 });

            // Command 0x61 - Set alignment (left)
            if (alignValue != 0)
                qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x61, 0 });

            return qrCodeEsc;
        }

        public static string ConvertToQRCodePrint3000(string text, string align, string size, string correction)
        {
            byte alignValue = align switch
            {
                "left" => 0,
                "center" => 1,
                "right" => 2,
                _ => 1
            };

            byte nsize = 9;
            var ncorrection = 'L';

            if (string.Equals(size, "tiny", StringComparison.OrdinalIgnoreCase))
                nsize = 5;
            else if (string.Equals(size, "small", StringComparison.OrdinalIgnoreCase))
                nsize = 6;
            else if (string.Equals(size, "normal", StringComparison.OrdinalIgnoreCase))
                nsize = 8;
            else if (string.Equals(size, "large", StringComparison.OrdinalIgnoreCase))
                nsize = 10;
            else if (string.Equals(size, "extralarge", StringComparison.OrdinalIgnoreCase))
                nsize = 11;

            if (string.Equals(correction, "low", StringComparison.OrdinalIgnoreCase))
                ncorrection = 'L';
            else if (string.Equals(correction, "medium", StringComparison.OrdinalIgnoreCase))
                ncorrection = 'M';
            else if (string.Equals(correction, "high", StringComparison.OrdinalIgnoreCase))
                ncorrection = 'Q';
            else if (string.Equals(correction, "ultra", StringComparison.OrdinalIgnoreCase))
                ncorrection = 'H';

            var qrCodeEsc =
                // Command 0x61 - Set alignment (center)
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1B, 0x61, 0x01 }) +

                //Command 0x6F - Change size of basic element
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1D, 0x6F, 0x00, nsize, 0x00, 0x02 }) +

                //Command 0x6B - Print (type 0x0B - QRCode)
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1D, 0x6B, 0x0B }) + //GS k

                //CorrectionLevel
                ncorrection +

                //Character mode: A - Mixed by alphabet and numbers
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x41, 0x2C }) +

                //QR Code text
                text +

                //End of QR code (NULL-end)
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x00 });

            // Command 0x61 - Set alignment (left)
            if (alignValue != 0)
                qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x61, 0 });

            return qrCodeEsc;
        }

        private string ConvertToLogoPrint1000(string logoId)
        {
            return //Command ESC 0x61 - Set Alignment (center)
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1B, 0x61, 0x01 }) +

                    //Command GS 0x54 - Print Image
                    BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1D, 0x54, 0x1C, 0x70, 0x01, 0x30 }) +

                    //Command ESC 0x61 - Set Alignment (left)
                    BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1B, 0x61, 0x00 });
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            if (CheckwayProduct.PRINT1000.CompareTo(ProductId) == 0)
                return ConvertToQRCodePrint1000(text, align, size, correction);
            if (CheckwayProduct.PRINT3000.CompareTo(ProductId) == 0)
                return ConvertToQRCodePrint3000(text, align, size, correction);
            return base.ConvertToQRCode(text, align, size, correction);
        }

        public override string ConvertToLogo(string logoId)
        {
            if (CheckwayProduct.PRINT1000.CompareTo(ProductId) == 0)
                return ConvertToLogoPrint1000(logoId);
            return base.ConvertToLogo(logoId);
        }
    }

    #endregion CheckwayDriver Class

    #region AtolPrinterDriver Class

    public partial class AtolPrinterDriver
    {
        public override string Name => Resources.AtolPrinterName;

        public override string Description => Resources.AtolPrinterDriverDescription;

        public override void InitPrintParams(int? baseWidth)
        {
            base.InitPrintParams(baseWidth);
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[]
                {
                    0x1b, 0x40, //Initialize printer
                    0x1b, 0x21, 0 //Set Font A(12x24 dots)
                });
        }

        protected override string ConvertToQRCode(string text, string align, byte nsize, string correction)
        {
            //workaround for Atol RP-326 to enlarge images
            return base.ConvertToQRCode(text, align, (byte)(nsize + 4), correction);
        }
    }

    #endregion AtolPrinterDriver Class

    #region CitizenCTSxxxDriver Class

    public partial class CitizenCTSxxxDriver
    {
        public override string Name => Resources.CitizenCTSxxxPrinterName;

        public override string Description => Resources.CitizenCTSxxxPrinterDriverDescription;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            base.InitPrintParams(baseWidth);
            bellEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x1e });
            logoBeginEsc =
                BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1D, 0x38, 0x4C, 0x06, 0x00, 0x00, 0x00, 0x30, 0x45, 0x4C });

            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x01, 0x01 });
        }

        public override string ConvertToLogo(string logoId)
        {
            int logoIndex;
            if (!Int32.TryParse(logoId, out logoIndex) || logoIndex < 0 || logoIndex > 9)
                return string.Empty;

            return logoId + LogoEndEsc;
        }

        #endregion
    }

    #endregion CitizenCTSxxxPrinterDriver Class

    #region SPrintTM200Driver Class

    public partial class SPrintTM200Driver
    {
        public override string Name => "SPrint TM200";

        public override string Description => Resources.SPrintTM200DriverDescription;

        public override bool CanPrintBarCode => false;

        public override bool CanPrintLogo => false;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[]
                { 0x1d, 0x68, 162, 0x1d, 0x77, 3, 0x1d, 0x48, 2, 0x1d, 0x6b, 67, 12 });
            return beginEsc + text;
        }

        public override string ConvertToPulse()
        {
            return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, (byte)PulsePort, 0x7F, 0x7F });
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 72;
            font1Width = 36;
            font2Width = 36;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 1 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 49 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 57 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1d, 0x56, 66, 0 });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 17 });
            bellEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, 0, 127, 127 });
            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1C, 0x70 });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0 });
        }

        #endregion
    }

    #endregion SPrintTM200Driver Class

    #region SPrintTM200SimpleDriver Class

    public partial class SPrintTM200SimpleDriver
    {
        public override string Name => "SPrint TM200 Min";

        public override string Description => Resources.SPrintTM200SimpleDriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[]
                { 0x1b, 0x21, 0x00, 0x1d, 0x68, 162, 0x1d, 0x77, 3, 0x1d, 0x48, 2, 0x1d, 0x6b, 67, 13 });
            return beginEsc + text;
        }

        public override string ConvertToPulse()
        {
            return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, (byte)PulsePort, 0x7F, 0x7F });
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 48;
            font1Width = 48;
            font2Width = 24;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0x00 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0x10 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0x30 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1d, 0x56, 65, 3 });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40 });
            bellEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, 0, 127, 127 });
            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1C, 0x70 });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0 });
        }

        #endregion
    }

    #endregion SPrintTM200SimpleDriver Class

    #region Shtrih600Driver Class

    public partial class Shtrih600Driver
    {
        public override string Name => Resources.Shtrih600DriverName;

        public override string Description => Resources.Shtrih600DriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[]
                { 0x1b, 0x50, 0x1b, 0x28, 0x42, 18, 0, 0, 2, 0, 0x7d, 0, 4 });
            return beginEsc + text;
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 30;
            font1Width = 24;
            font2Width = 15;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x50 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x58, 45, 0, 0 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x58, 72, 0, 0 });
            //pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1d });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x21, 0, 0x1b, 0x74, 2 });
            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1C, 0x70 });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0 });
        }

        #endregion
    }

    #endregion Shtrih600Driver Class

    #region Shtrih700Driver Class

    public partial class Shtrih700Driver
    {
        public Shtrih700Driver()
        {
        }

        public Shtrih700Driver(string codePage)
        {
            CodePage = codePage;
        }

        public override string Name => Resources.Shtrih700DriverName;

        public override string Description => Resources.Shtrih700DriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintQRCode => false;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc =
                BASE64_CHARSET_ENCODING.GetString(new byte[]
                    { 0x1b, 0x28, 0x42, 0x13, 0x00, 0x00, 0x02, 0x00, 0x20, 0x00, 0x10 });
            var endEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x0d });
            return beginEsc + text + endEsc;
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 48;
            font1Width = 23;
            font2Width = 23;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x46 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x45 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x45 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x0c });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40 });
        }

        #endregion
    }

    #endregion Shtrih700Driver Class

    #region PosiflexDriver Class

    public partial class PosiflexDriver
    {
        public override string Name => "Posiflex";

        public override string Description => Resources.PosiflexDriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[]
                { 27, 33, 50, 0x1d, 0x68, 162, 0x1d, 0x77, 3, 0x1d, 0x48, 2, 0x1d, 0x6b, 67, 13 });
            return beginEsc + text;
        }

        public override string ConvertToPulse()
        {
            if (string.IsNullOrEmpty(CodePage) || !CodePage.ToLower().StartsWith("unicode"))
                return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, (byte)PulsePort, 0x19, 0x7F });
            return string.Empty;
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 42;
            font1Width = 28;
            font2Width = 21;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 27, 33, 50 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 27, 33, 49 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 27, 33, 48 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 29, 86, 49 });
            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1C, 0x70 });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0 });
            var codePageId = Encoding.CodePage;

            if (string.IsNullOrEmpty(CodePage) || codePageId == 866)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 0x11 });
            else if (codePageId == 1251) //Windows Cyrillic 
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 28 });
            else if (codePageId == 855) //Cyrillic 
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 36 });
            else if (codePageId == 857) //Turkish 
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 37 });
            else if (codePageId == 852) //Latin 2
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 18 });
            else if (codePageId == 1252) //Windows European
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 16 });
            else if (codePageId == 850) //Multilingual (LatinI) 
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 2 });
            else if (codePageId == 860) //Portuguese
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 3 });
            else if (codePageId == 863) //Canadian-French
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 4 });
            else if (codePageId == 865) //Nordic
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 5 });
            else if (codePageId == 858) //Multilingual (Latin + Euro)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 19 });
            else if (codePageId == 862) //Hebrew
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 21 });
            else if (codePageId == 864) //Arabic
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 22 });
            else if (codePageId == 1253) //Windows Greek 
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 24 });
            else if (codePageId == 1254) //Windows Turkish
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 25 });
            else if (codePageId == 1257) //Windows Baltic
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 26 });
            else if (codePageId == 1256) //Windows Arabic (Farsi)
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 27 });
            else if (codePageId == 737) //Greek 
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 29 });
            else if (codePageId == 1255) //Hebrew New code
                startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, 33 });
            else
                startEsc = "";

            if (string.IsNullOrEmpty(CodePage) || !CodePage.ToLower().StartsWith("unicode"))
                bellEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x70, 0x00, 0x19, 0x7F });
            else
                bellEsc = "";
        }

        public override bool CanPrintQRCode => true;

        public override string ConvertToQRCode(string text, string size, string correction, string align)
        {
            byte alignValue = align switch
            {
                "left" => 0,
                "center" => 1,
                "right" => 2,
                _ => 1
            };

            byte sizeValue = size switch
            {
                "tiny" => 1,
                "small" => 3,
                "normal" => 5,
                "large" => 6,
                "extralarge" => 7,
                _ => 5
            };

            byte correctionValue = size switch
            {
                "low" => 48,
                "medium" => 49,
                "high" => 50,
                "ultra" => 51,
                _ => 49
            };

            var qrCodeEsc =
                BASE64_CHARSET_ENCODING.GetString(new byte[]
                    {
                        0x1b, 0x61, alignValue, // Set alignment
                        29, 40, 107, 3, 0, 49, 65, 50, 0, //model 2
                        29, 40, 107, 3, 0, 49, 67, sizeValue, //size of code symbol
                        29, 40, 107, 3, 0, 49, 69, correctionValue, //error correction level
                        29, 40, 107, (byte)(text.Length + 3), (byte)((text.Length - 1)/256), 49, 80, 48
                    }); //store text data
            qrCodeEsc += text;
            qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[] { 29, 40, 107, 3, 0, 49, 81, 48 }); //print data

            if (alignValue != 0)
                qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x61, 0 }); // Set alignment (left)

            return qrCodeEsc;
        }

        #endregion
    }

    #endregion PosiflexDriver Class

    #region WindowsPrinterDriver Class

    public partial class WindowsPrinterDriver
    {
        public override string Name => "Windows Printer";

        public override string Description => Resources.WindowsPrinterDriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override bool CanPrintQRCode => true;

        public override bool CanPrintImages => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            return ConvertToImageXml(base64Image, align, resizeMode);
        }

        public override string ConvertToLogo(string logoId)
        {
            return $"<logo>{SecurityElement.Escape(logoId)}</logo>";
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 42;
            font1Width = 40;
            font2Width = 20;
            pagecutEsc = "<pagecut/>";
        }

        #endregion
    }

    #endregion WindowsPrinterDriver Class

    #region PrimFRDriver Class

    public partial class PrimFRDriver : ICharCount
    {
        public override string AgentModel => "PrimFRDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.PrimFRDriverName;

        public override string Description => Resources.PrimFRDriverDescriptionFiscalRegistrar;

        public override bool IsObsolete => true;

        public int CharCount => 42;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 42;
            font1Width = 42;
            font2Width = 42;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion PrimFRDriver Class

    #region AzimuthPrimDriver Class
    public partial class AzimuthPrimDriver : ICharCount
    {
        public override string AgentModel => "AzimuthPrimDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.AzimuthPrimDriverName;

        public override string Description => Resources.AzimuthPrimDriverDescriptionFiscalRegistrar;

        public override bool CanUseFontSizes => true;

        public int CharCount => 42;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = width;
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = 42;
                font1Width = 21;
                font2Width = 21;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion AzimuthPrimDriver Class

    #region AzimuthFnPrimDriver Class
    public partial class AzimuthFnPrimDriver : ICharCount
    {
        public override string AgentModel => "AzimuthFnPrimDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.AzimuthFnPrimDriverName;

        public override string Description => Resources.AzimuthFnPrimDriverDescriptionFiscalRegister;

        public override bool CanUseFontSizes => true;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintQRCode => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public int CharCount => 42;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = width;
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = 42;
                font1Width = 21;
                font2Width = 21;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion AzimuthFnPrimDriver Class

    #region PilotFP410KDriver Class
    public partial class PilotFP410KDriver : ICharCount
    {
        public override string AgentModel => "PilotFP410KDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.PilotFP410KDriverName;

        public override string Description => Resources.PilotFP410KDriverDescriptionFiscalRegistrar;

        public int CharCount => 48;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = CharCount;
            font1Width = font0Width;
            font2Width = font0Width;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion PilotFP410KDriver Class

    #region Maria301Driver Class
    public partial class Maria301Driver : ICharCount
    {
        public override string AgentModel => "Maria301Driver";

        public override bool CanPrint => true;

        public override string Name => Resources.Maria301DriverName;

        public override string Description => Resources.Maria301DriverDescriptionFiscalRegistrar;

        public int CharCount => 43;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 43;
            font1Width = 43;
            font2Width = 43;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion Maria301Driver Class

    #region ArtSoftFiscalRegisterDriver Class
    public partial class ArtSoftFiscalRegisterDriver : ICharCount
    {
        public override string AgentModel => "ArtSoftFiscalRegisterDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.ArtSoftFiscalRegisterDriverName;

        public override string Description => Resources.ArtSoftFiscalRegisterDriverDescription;

        public int CharCount => 43;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 43;
            font1Width = 43;
            font2Width = 43;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion ArtSoftFiscalRegisterDriver Class

    #region CheckboxDriver Class
    public partial class CheckboxDriver : ICharCount
    {
        public override bool CanPrint => true;

        public override string Name => Resources.CheckboxDriverName;

        public override string Description => Resources.CheckboxDriverDescription;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 43;
            font1Width = 43;
            font2Width = 43;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion CheckboxDriver Class

    #region SP298Driver Class

    public partial class SP298Driver
    {
        public override string Name => Resources.SP298DriverName;

        public override string Description => Resources.SP298DriverDescription;

        public override bool CanPrintQRCode => true;

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            var qrString = string.Empty;
            var xSize = 140;
            if (string.Equals(size, "tiny", StringComparison.OrdinalIgnoreCase))
                xSize = 100;
            else if (string.Equals(size, "small", StringComparison.OrdinalIgnoreCase))
                xSize = 120;
            else if (string.Equals(size, "normal", StringComparison.OrdinalIgnoreCase))
                xSize = 140;
            else if (string.Equals(size, "large", StringComparison.OrdinalIgnoreCase))
                xSize = 150;
            else if (string.Equals(size, "extralarge", StringComparison.OrdinalIgnoreCase))
                xSize = 160;

            var ySize = xSize;

            var writer = new ZXing.BarcodeWriter<int> { Format = BarcodeFormat.QR_CODE };

            var options = new QrCodeEncodingOptions
            {
                Width = xSize,
                Height = ySize
            };
            if (string.Equals(correction, "low", StringComparison.OrdinalIgnoreCase))
                options.ErrorCorrection = ErrorCorrectionLevel.L;
            else if (string.Equals(correction, "medium", StringComparison.OrdinalIgnoreCase))
                options.ErrorCorrection = ErrorCorrectionLevel.M;
            else if (string.Equals(correction, "high", StringComparison.OrdinalIgnoreCase))
                options.ErrorCorrection = ErrorCorrectionLevel.Q;
            else if (string.Equals(correction, "ultra", StringComparison.OrdinalIgnoreCase))
                options.ErrorCorrection = ErrorCorrectionLevel.H;
            else
                options.ErrorCorrection = ErrorCorrectionLevel.M;

            writer.Options = options;
            var bitMatrix = writer.Encode(text);

            const int dotsPerLine = 8;

            var data = new byte[xSize + 4];

            //command 0x4A - Vertial Feed dotsPerLine Dots
            var cmdFeed = new byte[] { 0x1B, 0x4A, dotsPerLine };

            for (var yStep = 0; yStep <= ySize / dotsPerLine; yStep++)
            {
                var yBase = yStep * dotsPerLine;
                data[0] = 0x1B;
                data[1] = 0x4B;
                data[2] = (byte)((xSize) % 256);
                data[3] = (byte)((xSize) / 256);

                for (var x = 0; x < xSize; x++)
                {
                    data[4 + x] = 0x55;
                    var value = 0;
                    for (var yPos = 0; yPos < 8; yPos++)
                    {
                        if (yBase + yPos < ySize)
                        {
                            if (bitMatrix[x, yBase + yPos])
                                value += 1 << (7 - yPos);
                        }
                    }
                    data[4 + x] = (byte)value;
                }
                qrString += BASE64_CHARSET_ENCODING.GetString(data);
                qrString += BASE64_CHARSET_ENCODING.GetString(cmdFeed);
            }
            return qrString;
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 42;
            font1Width = 34;
            font2Width = 20;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x4d, 0x14 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x3a, 0x14 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x50, 0x0e });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1e, 0x1e, 0x1e, 0x1b, 0x0c, 0x03 });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40 });
            emptyLinesEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x61 });
        }

        public override int LinesOnPageParam => LinesOnPage;

        public override int EmptyLinesParam => EmptyLines;

        #endregion
    }

    #endregion SP298Driver Class

    #region SPxxxDriver Class

    public partial class SPxxxDriver
    {
        public override string Name => Resources.SPxxxDriverName;

        public override string Description => Resources.SPxxxDriverDescription;

        public override bool CanPrintLogo => true;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 42;
            font1Width = 23;
            font2Width = 17;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x4d, 0x14 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x3a, 0x14 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x50, 0x0e });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x4a, 0x40, 0x1b, 0x64, 1 });
            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x0A, 0x1B, 0x1C });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0 });
        }

        #endregion
    }

    #endregion SPxxxDriver Class

    #region ShtrihDriver Class

    public partial class ShtrihDriver : ICharCount
    {
        public override string AgentModel => "ShtrihDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.ShtrihMDriverName;

        public override string Description => Resources.ShtrihMDriverDescriptionFiscalRegistrar;

        public override bool IsObsolete => true;

        public override bool CanUseFontSizes => true;


        public int CharCount => 48;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintQRCode => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = AgentDriverHelper.GetWideFontWidth(width);
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = 48;
                font1Width = 24;
                font2Width = 24;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        #endregion
    }

    #endregion ShtrihDriver Class

    #region ShtrihFRFDriver Class

    public partial class ShtrihFRFDriver : ICharCount
    {
        public override bool CanPrint => true;

        public override string Name => Resources.ShtrihFRFDriverName;

        public override string Description => Resources.ShtrihFRFDriverDescriptionFiscalRegistrar;

        public override bool IsObsolete => false;

        public override bool CanUseFontSizes => true;

        public new int CharCount => 36;

        public override bool CanPrintQRCode => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        #region Поддержка печати в C#

        public override bool ZeroCashOnClose => false;

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = AgentDriverHelper.GetWideFontWidth(width);
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = 36;
                font1Width = 18;
                font2Width = 18;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion ShtrihFRFDriver Class

    #region SparkDriver Class

    public partial class SparkDriver : ICharCount
    {
        public override string AgentModel => "SparkDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.SparkDriverName;

        public override string Description => Resources.SparkDriverDescriptionFiscalRegistrar;

        public override bool CanUseFontSizes => true;

        public override bool CanPrintQRCode => true;

        public int CharCount => 48;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            if (baseWidth.HasValue)
            {
                int width = baseWidth.Value;
                font0Width = width;
                font1Width = AgentDriverHelper.GetWideFontWidth(width);
                font2Width = AgentDriverHelper.GetWideFontWidth(width);
            }
            else
            {
                font0Width = 48;
                font1Width = 24;
                font2Width = 24;
            }
            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";

            pagecutEsc = Papercut;
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override bool ZeroCashOnClose => true;

        public override bool IsCancellationSupported => true;

        public override bool IsBillTaskSupported => true;

        public override bool NeedToCheckBillNumber => true;

        public override bool IsRegisterStatusSupported => true;

        #endregion
    }

    #endregion SparkDriver Class

    #region SparkFnDriver Class

    public partial class SparkFnDriver : ICharCount
    {
        public override string AgentModel => "SparkFnDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.SparkFnDriverName;

        public override string Description => Resources.SparkFnDriverDescriptionFiscalRegistrar;

        public override bool CanUseFontSizes => true;

        public override bool CanPrintQRCode => true;

        public int CharCount => font0Width;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            const int defaultWidth = 42;
            const int minWidth = 7;
            var width = (baseWidth.HasValue && baseWidth > minWidth) ? baseWidth.Value : defaultWidth;

            font0Width = width;
            font1Width = AgentDriverHelper.GetWideFontWidth(width);
            font2Width = AgentDriverHelper.GetWideFontWidth(width);

            font0Esc = "<f0>";
            font1Esc = "<f1>";
            font2Esc = "<f2>";

            pagecutEsc = Papercut;
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override bool ZeroCashOnClose => true;

        #endregion
    }

    #endregion SparkDriver Class

    #region TSPxxxDriver Class

    public partial class TSPxxxDriver
    {
        public override string Name => Resources.TSPxxxDriverName;

        public override string Description => Resources.TSPxxxDriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override bool CanPrintQRCode => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[]
                { 0x1b, 0x4d, 0x1b, 0x69, 0, 0, 0x1b, 0x62, 3, 2, 2, 127 });
            var endEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1e });
            return beginEsc + text + endEsc;
        }

        public override string ConvertToPulse()
        {
            return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x07, 20, 20, 0x07 });
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 48;
            font1Width = 38;
            font2Width = 19;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x4d, 0x1b, 0x69, 0, 0 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x50, 0x1b, 0x69, 1, 1, 0x1b, 0x67, 0x1b, 0x69, 1, 0 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x50, 0x1b, 0x69, 1, 1 });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40, 0x1b, 0x1d, 0x74, 10 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x4a, 0x40, 0x1b, 0x64, 1 });
            bellEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x07, 20, 20, 0x07 });
        }

        public override string ConvertToLogo(string logoId)
        {
            return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1B, 0x1C, 0x70, Convert.ToByte(logoId), 0x00 });
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            byte alignValue = align switch
            {
                "left" => 0,
                "center" => 1,
                "right" => 2,
                _ => 1
            };

            byte nsize = 7;
            byte ncorrection = 49;

            if (string.Equals(size, "tiny", StringComparison.OrdinalIgnoreCase))
                nsize = 1;
            else if (string.Equals(size, "small", StringComparison.OrdinalIgnoreCase))
                nsize = 3;
            else if (string.Equals(size, "normal", StringComparison.OrdinalIgnoreCase))
                nsize = 5;
            else if (string.Equals(size, "large", StringComparison.OrdinalIgnoreCase))
                nsize = 7;
            else if (string.Equals(size, "extralarge", StringComparison.OrdinalIgnoreCase))
                nsize = 8;


            if (string.Equals(correction, "low", StringComparison.OrdinalIgnoreCase))
                ncorrection = 0;
            else if (string.Equals(correction, "medium", StringComparison.OrdinalIgnoreCase))
                ncorrection = 1;
            else if (string.Equals(correction, "high", StringComparison.OrdinalIgnoreCase))
                ncorrection = 2;
            else if (string.Equals(correction, "ultra", StringComparison.OrdinalIgnoreCase))
                ncorrection = 3;

            var qrCodeEsc =
                BASE64_CHARSET_ENCODING.GetString(new byte[]
                    {
                        0x1B, 0x61, alignValue, //align
                        27, 29, 121, 83, 48, 2, //model 2
                        27, 29, 121, 83, 50, nsize, //size of code symbol
                        27, 29, 121, 83, 49, ncorrection, //error correction level
                        27, 29, 121, 68, 49, 0, (byte)(text.Length), (byte)((text.Length - 1)/256)
                    }); //store text data
            qrCodeEsc += text;
            qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[]
                {
                    27, 29, 121, 73, //check expansion
                    27, 29, 121, 80, //print data to buffer
                    27, 73, 10, 0
                }); //actual printing

            // Command 0x61 - Set alignment (left)
            if (alignValue != 0)
                qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x61, 0 });

            return qrCodeEsc;
        }

        #endregion
    }

    #endregion TSPxxxDriver Class

    #region SMxxxDriver Class

    public partial class SMxxxDriver
    {
        public override string Name => Resources.SMxxxDriverName;

        public override string Description => Resources.SMxxxDriverDescription;

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override bool CanPrintQRCode => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc =
                BASE64_CHARSET_ENCODING.GetString(new byte[]
                {
                    0x1d, 0x68, 0x60, //Set barcode height
                    0x1d, 0x77, 0x03, //Set barcode width
                    0x1d, 0x6b, 0x02
                });
            var endEsc  = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x00 });
            return beginEsc + text + endEsc;
        }

        #region Поддержка печати в C#

        private static readonly Dictionary<int, byte> CodePageMap = new Dictionary<int, byte>
            {
                {437, 0}, //PC437 (USA, Standard Europe)
                {20290, 1}, //Japanese katakana
                {858, 2}, //Multilingual PC858
                {860, 3}, //Portuguese PC860
                {863, 4}, //Canadian-French PC863
                {865, 5}, //Nordic PC865
                {852, 6}, // Slavic (Latin-2) PC852
                {861, 7}, //Icelandic PC861
                {866, 8}, //Cyrillic Russian PC866
                {855, 9}, //Cyrillic PC855
                {857, 10}, //Turkish (Latin #5) PC857
                {862, 11}, //Israel (Hebrew) PC862
                {864, 12}, //Arabic PC864
                {737, 13}, //Greek PC737
                {772, 14}, //Lithuanian PC772
                {774, 15}, //Lithuanian PC774
                {874, 16}, //Thailand PC874
                {1252, 17}, //Windows Latin-1 PC1252
            };

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 48;
            font1Width = 48;
            font2Width = 24;
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0x00 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0x10 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x21, 0x38 });
            startEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x40 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x0a });

            if (!string.IsNullOrEmpty(CodePage) && CodePageMap.ContainsKey(Encoding.CodePage))
                startEsc +=
                    BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x74, CodePageMap[Encoding.CodePage] });

        }

        public override string ConvertToLogo(string logoId)
        {
            return BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1B, 0x66, Convert.ToByte(logoId), 0x0C });
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            byte alignValue = align switch
            {
                "left" => 0,
                "center" => 1,
                "right" => 2,
                _ => 1
            };

            byte nsize = 7;
            byte ncorrection = 0x4D;

            if (string.Equals(size, "tiny", StringComparison.OrdinalIgnoreCase))
                nsize = 4;
            else if (string.Equals(size, "small", StringComparison.OrdinalIgnoreCase))
                nsize = 5;
            else if (string.Equals(size, "normal", StringComparison.OrdinalIgnoreCase))
                nsize = 6;
            else if (string.Equals(size, "large", StringComparison.OrdinalIgnoreCase))
                nsize = 7;
            else if (string.Equals(size, "extralarge", StringComparison.OrdinalIgnoreCase))
                nsize = 8;

            if (string.Equals(correction, "low", StringComparison.OrdinalIgnoreCase))
                ncorrection = 0x4C;
            else if (string.Equals(correction, "medium", StringComparison.OrdinalIgnoreCase))
                ncorrection = 0x4D;
            else if (string.Equals(correction, "high", StringComparison.OrdinalIgnoreCase))
                ncorrection = 0x51;
            else if (string.Equals(correction, "ultra", StringComparison.OrdinalIgnoreCase))
                ncorrection = 0x48;

            var length = Convert.ToUInt16(text.Length);
            var qrCodeEsc =
                BASE64_CHARSET_ENCODING.GetString(new byte[]
                    {
                        0x1D, 0x5A, 0x02, //barcode type - QR-code
                        0x1B, 0x5A, 0x00, ncorrection, nsize, (byte)(length & 0xff), (byte)(length >> 8),
                        //print QR-code
                    });

            qrCodeEsc += text;

            // Command 0x61 - Set alignment (left)
            if (alignValue != 0)
                qrCodeEsc += BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x61, 0 });

            return qrCodeEsc;
        }

        #endregion
    }

    #endregion TSPxxxDriver Class

    #region MercuryDriver Class

    public partial class MercuryDriver : ICharCount
    {
        public override string AgentModel => "MercuryDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.MercuryDriverName;

        public override string Description => Resources.MercuryDriverDescriptionFiscalRegistrar;

        public virtual int CharCount => 40;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 40;
            font1Width = 40;
            font2Width = 40;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion MercuryDriver Class

    #region MstarTKDriver Class

    public partial class MstarTKDriver
    {
        public override bool CanPrint => true;

        public override string Name => Resources.MstarTKDriverName;

        public override string Description => Resources.MstarTKDriverDescriptionFiscalRegistrar;

        public override int CharCount => 40;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 40;
            font1Width = 40;
            font2Width = 40;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion MstarTKDriver Class

    #region Mercury130Driver Class

    public partial class Mercury130Driver : ICharCount
    {
        public override string AgentModel => "Mercury130Driver";

        public override bool CanPrint => false;

        public override string Name => Resources.Mercury130DriverName;

        public override string Description => Resources.Mercury130DriverDescriptionFiscalRegistrar;

        public int CharCount => 30;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 40;
            font1Width = 40;
            font2Width = 40;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion Mercury130Driver Class

    #region DatecsDriver Class

    public partial class DatecsDriver : ICharCount
    {
        public override string AgentModel => "DatecsDriver";

        public override bool CanPrint => true;

        public override string Name => Resources.DatecsDriverName;

        public override string Description => Resources.DatecsDriverDescriptionFiscalRegistrar;

        public int CharCount => 35;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = 35;
            font1Width = 35;
            font2Width = 35;
            pagecutEsc = Papercut;
        }

        #endregion
    }

    #endregion DatecsDriver Class

    #region CashServerDriver Class

    public partial class CashServerDriver : ICharCount, IVirtualCashRegisterDriver
    {
        public override string AgentModel => "CashServerDriver";

        public override bool CanPrint => false;

        public override string Name => Resources.CashServerDriverName;

        public override string Description => Resources.CashServerDriverName;

        public override AgentDriverType DriverType => AgentDriverType.Front;

        public int CharCount => 48;

        public override bool ZeroCashOnClose => true;

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
        }

        #endregion
    }

    #endregion CashServerDriver Class

    #region OPOSPrinterDriver Class

    public partial class OPOSPrinterDriver
    {
        public override string AgentModel => "OPOSPrinterDriver";

        public static readonly Encoding BASE64_CHARSET_ENCODING = Encoding.GetEncoding("cp866");

        public override string Name => Resources.OPOSPrinterDriverOPOSPrinter;

        public override string Description => Resources.OPOSPrinterDriverOPOSPrinter;

        public virtual Encoding Encoding => EncodingsManager.GetEncoding(CodePage, BASE64_CHARSET_ENCODING);

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            var beginEsc = "<barcode data=\"";
            var endEsc = "\"/>";
            return beginEsc + text + endEsc;
        }

        public override string ConvertToPulse()
        {
            return $"<pulse data=\"{pulsePort}\"/>\n";
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x7c, 0x31, 0x43 });
            font1Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x7c, 0x34, 0x43 });
            font2Esc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x7c, 0x33, 0x68, 0x43, 0x1b, 0x7c, 0x32, 0x76, 0x43 });
            pagecutEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1b, 0x7c, 0x30, 0x66, 0x50 });
            bellEsc = "<bell data=\"\"/>"; // можно задавать параметры для звонка (поэтому есть атрибут data)
            logoBeginEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x1C, 0x70 });
            logoEndEsc = BASE64_CHARSET_ENCODING.GetString(new byte[] { 0x30 });
        }

        public override int Font0Width => OposFont2Width;

        public override int Font1Width => OposFont1Width;

        public override int Font2Width => OposFont0Width;

        #endregion

        public override bool CanPrintBarCode => true;

        public override bool CanPrintLogo => true;

        public override bool CanPrintQRCode => true;

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            string qrCodeEsc = "<qrcode data=\"";
            qrCodeEsc += text;
            qrCodeEsc += "\"/>";
            return qrCodeEsc;
        }
    }

    #endregion OPOSPrinterDriver Class

    #region ScaleDeviceDriver Class

    public partial class ScaleDeviceDriver
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public virtual string AgentModel => "ScaleDeviceDriver";

        public abstract AgentDriverType DriverType { get; }

        public bool IsObsolete => false;

        /// <summary>
        /// Имя конкретного типа весы, необходимо агенту при создании драйвера.
        /// </summary>
        public abstract string ScaleName { get; }
    }

    #endregion ScaleDeviceDriver Class

    #region CASScaleDriver Class

    public partial class CASScaleDriver
    {
        public override string Name => "CAS";

        public override string Description => Resources.CASScaleDriverDescription;

        public override AgentDriverType DriverType => AgentDriverType.Agent;

        public override string ScaleName => "CAS";
    }

    #endregion CASScaleDriver Class

    #region DigiScaleDriver Class

    public partial class DigiScaleDriver
    {
        public override string Name => "DIGI";

        public override string Description => Resources.DigiScaleDriverDescription;

        public override AgentDriverType DriverType => AgentDriverType.Agent;

        public override string ScaleName => "DIGI";
    }

    #endregion DigiScaleDriver Class

    #region ExternalScaleDriver Class

    public partial class ExternalScaleDriver
    {
        public override string Name => Resources.ExternalScaleDriverName;
        public override string AgentModel => "ExternalScaleDriver";

        public override string Description => Resources.ExternalScaleDriverName;

        public override AgentDriverType DriverType => AgentDriverType.Agent;

        public override string ScaleName => Resources.ExternalScaleDriverName;
    }

    #endregion DigiScaleDriver Class

    #region MassaScaleDriver Class

    public partial class MassaScaleDriver
    {
        public override string Name => Resources.MassaScaleDriverName;

        public override string Description => Resources.MassaScaleDriverDescription;

        public override AgentDriverType DriverType => AgentDriverType.Agent;

        public override string ScaleName => "Massa_Scales";
    }

    #endregion MassaScaleDriver Class

    #region ShtrihPrintScaleDriver Class

    public partial class ShtrihPrintScaleDriver
    {
        public override string Name => Resources.ShtrihPrintScaleDriverName;

        public override string Description => Resources.ShtrihPrintScaleDriverDescription;

        public override AgentDriverType DriverType => AgentDriverType.Agent;

        public override string ScaleName => "ShtrihPrint_Scales";
    }

    #endregion ShtrihPrintScaleDriver Class

    #region NciEcrScaleDriver Class

    public partial class NciEcrScaleDriver
    {
        public override string Name => Resources.NciEcrScaleDriverName;

        public override string Description => Resources.NciEcrScaleDriverDescription;

        public override AgentDriverType DriverType => AgentDriverType.Agent;

        public override string ScaleName => "NciEcr_Scales";
    }

    #endregion NciEcrScaleDriver Class

    #region ShtrihScaleDriver Class

    public partial class ShtrihScaleDriver
    {
        public override string Name => Resources.ShtrihScaleDriverName;

        public override string Description => Resources.ShtrihScaleDriverDescription;

        public override AgentDriverType DriverType => AgentDriverType.Agent;

        public override string ScaleName => "Shtrih_Scales";
    }

    #endregion ShtrihScaleDriver Class

    #region DiscountDriver Class

    public abstract partial class DiscountDriver
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string AgentModel { get; }

        public virtual AgentDriverType DriverType => AgentDriverType.Virtual;

        public bool IsObsolete => false;
    }

    #endregion DiscountDriver Class

    #region DiscountCardDriver Class

    public partial class DiscountCardDriver
    {
        public override string Name => Resources.DiscountCardDriverName;

        public override string AgentModel => "DiscountCard";

        public override string Description => Resources.DiscountCardDriverName;
    }

    #endregion LuckyTicketDriver Class

    #region LuckyTicketDriver Class

    public partial class LuckyTicketDriver
    {
        public override string Name => Resources.LuckyTicketDriverName;

        public override string AgentModel => "LuckyTicket";

        public override string Description => Resources.LuckyTicketDriverName;
    }

    #endregion LuckyTicketDriver Class

    #region PowerDeviceDriver Class

    public partial class PowerDeviceDriver
    {
        public string Name => Resources.PowerDeviceDriverName;

        public string Description => Resources.PowerDeviceDriverDescription;

        public string AgentModel => "PowerDeviceDriver";

        public AgentDriverType DriverType => AgentDriverType.Agent;

        public bool IsObsolete => false;
    }

    #endregion PowerDeviceDriver Class

    #region CashDrawerDeviceDriver Class

    public partial class CashDrawerDeviceDriver
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public string AgentModel => "CashDrawerDeviceDriver";

        public AgentDriverType DriverType => AgentDriverType.Agent;

        public bool IsObsolete => false;

        /// <summary>
        /// Имя конкретного типа д/я, необходимо агенту при создании драйвера.
        /// </summary>
        public abstract string CashDrawerName { get; }
    }

    #endregion CashDrawerDeviceDriver Class

    public partial class CashDrawerHolder
    {
        public override string ToString()
        {
            return cashDrawerDevice == null ? Resources.InternalCashDrawerName : cashDrawerDevice.NameLocal;
        }
    }

    #region OPOSCashDrawerDriver Class

    public partial class OPOSCashDrawerDriver
    {
        public override string Name => Resources.OPOSCashDrawerDriverName;

        public override string Description => Resources.OPOSCashDrawerDriverDescription;

        public override string CashDrawerName => string.Empty;
    }

    #endregion OPOSCashDrawerDriver Class

    #region ZebraEplDriver Class

    public partial class ZebraEplDriver
    {
        public override string Name => Resources.ZebraEplDriverName;
        public override string Description => Resources.ZebraEplDriverDescription;
        // первая тройка для 203 dpi, вторая для 300 dpi
        private static readonly int[,] FontWidths = { { 10, 12, 14 }, { 16, 20, 24 } };
        private static readonly int[,] FontHeights = { { 16, 20, 24 }, { 28, 36, 44 } };
        public override bool CanPrintBarCode => true;

        public int Font0Number => 2;
        public int Font1Number => 3;
        public int Font2Number => 4;
        public int Font0CharHeight => FontHeights[Dpi == 203 ? 0 : 1, 0];
        public int Font1CharHeight => FontHeights[Dpi == 203 ? 0 : 1, 1];
        public int Font2CharHeight => FontHeights[Dpi == 203 ? 0 : 1, 2];
        public int BarcodeMarginVert => 3;
        public override bool CanPrintQRCode => true;
        public override bool CanPrintImages => true;

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            return ConvertToImageXml(base64Image, align, resizeMode);
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            int index = Dpi == 203 ? 0 : 1;
            const int interCharSpace = 2;
            font0Width = LabelWidth / (FontWidths[index, 0] + interCharSpace);
            font1Width = LabelWidth / (FontWidths[index, 1] + interCharSpace);
            font2Width = LabelWidth / (FontWidths[index, 2] + interCharSpace);
        }

        #endregion
    }

    #endregion ZebraEplDriver Class

    #region PoscenterDriver Class

    public partial class PoscenterDriver
    {
        public override string Name => Resources.PoscenterDriverName;
        public override string Description => Resources.PoscenterDriverDescription;

        private static readonly int[] FontWidths = { 9, 12, 16 };
        private static readonly int[] FontHeights = { 15, 20, 25 };
        public override bool CanPrintBarCode => true;
        public int Font0Number => 0;
        public int Font1Number => 1;
        public int Font2Number => 2;
        public int Font0CharHeight => FontHeights[0];
        public int Font1CharHeight => FontHeights[1];
        public int Font2CharHeight => FontHeights[2];

        public override bool CanPrintQRCode => true;
        public override bool CanPrintImages => true;

        public int BarcodeMarginVert => 3;

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            return ConvertToImageXml(base64Image, align, resizeMode);
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = LabelWidth / FontWidths[0];
            font1Width = LabelWidth / FontWidths[1];
            font2Width = LabelWidth / FontWidths[2];
        }

        #endregion
    }

    #endregion PoscenterDriver Class

    #region TsplPrinterDriver

    public partial class TsplPrinterDriver
    {
        public override string Name => Resources.TsplDriverName;
        public override string Description => Resources.TsplDriverDescription;

        private static readonly int[] FontWidths = { 8, 12, 16 };
        private static readonly int[] FontHeights = { 12, 20, 24 };
        public int Font0Number => 1;
        public int Font1Number => 2;
        public int Font2Number => 3;
        public int Font0CharWidth => FontWidths[0];
        public int Font1CharWidth => FontWidths[1];
        public int Font2CharWidth => FontWidths[2];
        public int Font0CharHeight => FontHeights[0];
        public int Font1CharHeight => FontHeights[1];
        public int Font2CharHeight => FontHeights[2];
        public int BarcodeMarginVert => 3;
        public override bool CanPrintBarCode => true;
        public override bool CanPrintQRCode => true;
        public override bool CanPrintImages => true;

        //Промежуточная кодировка для передачи бинарных данных в процессор
        private static Encoding DataTransferEncoding => Encoding.GetEncoding("windows-1251");

        public TsplPrinterDriver()
        {
            CodePage = DataTransferEncoding.HeaderName;
        }

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            return ConvertToImageXml(base64Image, align, resizeMode);
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            const int interCharSpace = 2;
            font0Width = LabelWidth / (FontWidths[0] + interCharSpace);
            font1Width = LabelWidth / (FontWidths[1] + interCharSpace);
            font2Width = LabelWidth / (FontWidths[2] + interCharSpace);
        }

        #endregion
    }

    #endregion

    #region BP21Driver Class

    public partial class BP21PrinterDriver
    {
        public override string Name => Resources.BP21DriverName;
        public override string Description => Resources.BP21DriverDescription;

        private static readonly int[] FontWidths = { 8, 12, 16 };
        private static readonly int[] FontHeights = { 12, 20, 24 };
        public int Font0Number => 1;
        public int Font1Number => 2;
        public int Font2Number => 3;
        public int Font0CharWidth => FontWidths[0];
        public int Font1CharWidth => FontWidths[1];
        public int Font2CharWidth => FontWidths[2];
        public int Font0CharHeight => FontHeights[0];
        public int Font1CharHeight => FontHeights[1];
        public int Font2CharHeight => FontHeights[2];
        public int BarcodeMarginVert => 3;
        public override bool CanPrintBarCode => true;
        public override bool CanPrintQRCode => true;
        public override bool CanPrintImages => true;

        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            return ConvertToImageXml(base64Image, align, resizeMode);
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = LabelWidth / FontWidths[0];
            font1Width = LabelWidth / FontWidths[1];
            font2Width = LabelWidth / FontWidths[2];
        }

        #endregion
    }

    #endregion ZebraEplDriver Class

    #region BP41PrinterDriver

    public partial class BP41PrinterDriver
    {
        public override string Name => Resources.BP41DriverName;
        public override string Description => Resources.BP41DriverDescription;
        private static readonly int[] FontWidths = { 8, 12, 16 };
        private static readonly int[] FontHeights = { 12, 20, 24 };
        public int Font0Number => 1;
        public int Font1Number => 2;
        public int Font2Number => 3;
        public int Font0CharWidth => FontWidths[0];
        public int Font1CharWidth => FontWidths[1];
        public int Font2CharWidth => FontWidths[2];
        public int Font0CharHeight => FontHeights[0];
        public int Font1CharHeight => FontHeights[1];
        public int Font2CharHeight => FontHeights[2];
        public int BarcodeMarginVert => 3;
        public override bool CanPrintBarCode => true;
        public override bool CanPrintQRCode => true;
        public override bool CanPrintImages => true;

        public BP41PrinterDriver()
        {
            CodePage = Encoding.GetEncoding("windows-1251").HeaderName;
        }
        public override string ConvertToBarcode(string text, string align, string heightRatio, string hri)
        {
            return ConvertToBarcodeXml(text, align, heightRatio, hri);
        }

        public override string ConvertToQRCode(string text, string align, string size, string correction)
        {
            return ConvertToQRCodeXml(text, align, size, correction);
        }

        public override string ConvertToImage(string base64Image, string align, string resizeMode)
        {
            return ConvertToImageXml(base64Image, align, resizeMode);
        }

        #region Поддержка печати в C#

        public override void InitPrintParams(int? baseWidth)
        {
            font0Width = LabelWidth / FontWidths[0];
            font1Width = LabelWidth / FontWidths[1];
            font2Width = LabelWidth / FontWidths[2];
        }

        #endregion
    }

    #endregion

    #region Null device drivers
    public partial class ChequePrinterNullDriver : ICharCount
    {
        public override string Name => "ChequePrinterNullDriver";

        public override string AgentModel => "ChequePrinterNullDriver";        

        public override string Description => Resources.NullDriverDescription;

        public override void InitPrintParams(int? baseWidth) { }

        public override AgentDriverType DriverType => AgentDriverType.Front;

        public int CharCount => 1;

        public override bool CanPrint => false;
    }

    public partial class PrinterNullDriver
    {
        public override string Name => "PrinterNullDriver";
        public override string AgentModel => "PrinterNullDriver";
        public override string Description => Resources.NullDriverDescription;
        public override void InitPrintParams(int? baseWidth) { }

        public override AgentDriverType DriverType => AgentDriverType.Front;
    }

    public partial class ScaleNullDriver
    {
        public override string Name => "ScaleNullDriver";

        public override string Description => Resources.NullDriverDescription;

        public override string ScaleName => "ScaleNullDriver";

        public override AgentDriverType DriverType => AgentDriverType.Front;
    }
    #endregion
}
