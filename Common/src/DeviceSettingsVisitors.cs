using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Properties;
using Resto.Common.UI.Controls;
using Resto.Configuration;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;
using AgentDevicesHelper = Resto.Common.src.Extensions.AgentDevicesHelper;

namespace Resto.Configuration
{
    #region IConfigPageCreator Interface

    public interface IConfigPageCreator
    {
        IValidator Create(PortWriterDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(OPOSPrinterDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(ShtrihDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PrimFRDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(AzimuthPrimDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(AzimuthFnPrimDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PilotFP410KDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(Maria301DriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(ArtSoftFiscalRegisterDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(SparkDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(SparkFnDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(DatecsDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(AtolDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(IcsDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(IksSpsDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PosnetDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(HrsDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PosnetThermalDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PiritDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PiritFnDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(BrioFisFm32DriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(AlposAf01DriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(EmpirijaDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(OPOSRegisterDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(UnisystemDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(CashServerDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(VideoDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(FV2029Settings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(OposCustomerDisplaySettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PlastekSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(VirtualPlastekSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PulsarSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(HoistSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(VirtualDeviceSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(MercuryDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(Mercury130DriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(IPCameraSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(ScaleDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(PowerDeviceDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(CashDrawerDriverSettings s, PreviousDeviceSettings previousDeviceSettings);
        IValidator Create(ExternalDeviceSettings s, PreviousDeviceSettings previousDeviceSettings);
    }

    #endregion IConfigPageCreator Interface
}

namespace Resto.Data
{
    #region DeviceSettingsVisitor Class

    public class DeviceSettingsVisitor
    {
        //
        // настройки драйвера записи в порт
        //
        public virtual void Store(PortWriterDriverSettings s)
        {
        }

        public virtual void Load(PortWriterDriverSettings s)
        {
        }

        //
        // настройки драйвера OPOS-принтера
        //
        public virtual void Store(OPOSPrinterDriverSettings s)
        {
        }

        public virtual void Load(OPOSPrinterDriverSettings s)
        {
        }


        //
        // настройки линейки ШТРИХ-М
        //
        public virtual void Store(ShtrihDriverSettings s)
        {
        }

        public virtual void Load(ShtrihDriverSettings s)
        {
        }

        //
        // настройки линейки ПРИМ-ФР
        //
        public virtual void Store(PrimFRDriverSettings s)
        {
        }

        public virtual void Load(PrimFRDriverSettings s)
        {
        }

        //
        // настройки линейки ПРИМ-ФР(Azimuth)
        //
        public virtual void Store(AzimuthPrimDriverSettings s)
        {
        }

        public virtual void Load(AzimuthPrimDriverSettings s)
        {
        }

        //
        // настройки линейки ПРИМ-ФР с ФН(Azimuth)
        //
        public virtual void Store(AzimuthFnPrimDriverSettings s)
        {
        }

        public virtual void Load(AzimuthFnPrimDriverSettings s)
        {
        }

        //
        // настройки линейки ПИЛОТ FP-410K
        //
        public virtual void Store(PilotFP410KDriverSettings s)
        {
        }

        public virtual void Load(PilotFP410KDriverSettings s)
        {
        }

        //
        // настройки линейки Мария-301 МТМ
        //
        public virtual void Store(Maria301DriverSettings s)
        {
        }

        public virtual void Load(Maria301DriverSettings s)
        {
        }

        //
        // настройки универсального драйвера украинских ФР от АртСофт
        //
        public virtual void Store(ArtSoftFiscalRegisterDriverSettings s)
        {
        }

        public virtual void Load(ArtSoftFiscalRegisterDriverSettings s)
        {
        }

        //
        // настройки линейки СПАРК
        //
        public virtual void Store(SparkDriverSettings s)
        {
        }

        public virtual void Load(SparkDriverSettings s)
        {
        }

        //
        // настройки линейки СПАРК с ФН
        //
        public virtual void Store(SparkFnDriverSettings s)
        {
        }

        public virtual void Load(SparkFnDriverSettings s)
        {
        }

        //
        // настройки линейки МЕРКУРИЙ MS-K
        //
        public virtual void Store(MercuryDriverSettings s)
        {
        }

        public virtual void Load(MercuryDriverSettings s)
        {
        }

        //
        // настройки линейки МЕРКУРИЙ 130
        //
        public virtual void Store(Mercury130DriverSettings s)
        {
        }

        public virtual void Load(Mercury130DriverSettings s)
        {
        }

        //
        // настройки линейки Datecs
        //
        public virtual void Store(DatecsDriverSettings s)
        {
        }

        public virtual void Load(DatecsDriverSettings s)
        {
        }

        //
        // настройки линейки АТОЛ
        //
        public virtual void Store(AtolDriverSettings s)
        {
        }

        public virtual void Load(AtolDriverSettings s)
        {
        }

        //
        // настройки линейки ICS (IKS-MARKET)
        //
        public virtual void Store(IcsDriverSettings s)
        {
        }

        public virtual void Load(IcsDriverSettings s)
        {
        }

        //
        // настройки линейки IKS (SPS)
        //
        public virtual void Store(IksSpsDriverSettings s)
        {
        }

        public virtual void Load(IksSpsDriverSettings s)
        {
        }

        //
        // настройки линейки Posnet
        //
        public virtual void Store(PosnetDriverSettings s)
        {
        }

        public virtual void Load(PosnetDriverSettings s)
        {
        }

        //
        // настройки линейки Hrs
        //
        public virtual void Store(HrsDriverSettings s)
        {
        }

        public virtual void Load(HrsDriverSettings s)
        {
        }

        //
        // настройки линейки PosnetThermal
        //
        public virtual void Store(PosnetThermalDriverSettings s)
        {
        }

        public virtual void Load(PosnetThermalDriverSettings s)
        {
        }

        //
        // настройки линейки BRIO Fis-FM32
        //
        public virtual void Store(BrioFisFm32DriverSettings s)
        {
        }

        public virtual void Load(BrioFisFm32DriverSettings s)
        {
        }

        //
        // настройки линейки ALPOS AF01
        //
        public virtual void Store(AlposAf01DriverSettings s)
        {
        }

        public virtual void Load(AlposAf01DriverSettings s)
        {
        }

        //
        //
        // настройки линейки Pirit
        //
        public virtual void Store(PiritDriverSettings s)
        {
        }

        public virtual void Load(PiritDriverSettings s)
        {
        }

        //
        //
        // настройки линейки Pirit с ФН
        //
        public virtual void Store(PiritFnDriverSettings s)
        {
        }

        public virtual void Load(PiritFnDriverSettings s)
        {
        }

        // настройки линейки Empirija
        //
        public virtual void Store(EmpirijaDriverSettings s)
        {
        }

        public virtual void Load(EmpirijaDriverSettings s)
        {
        }

        //
        // настройки линейки OPOSRegister
        //
        public virtual void Store(OPOSRegisterDriverSettings s)
        {
        }

        public virtual void Load(OPOSRegisterDriverSettings s)
        {
        }

        //
        // настройки линейки Юнисистем
        //
        public virtual void Store(UnisystemDriverSettings s)
        {
        }

        public virtual void Load(UnisystemDriverSettings s)
        {
        }

        //
        // настройки кассового сервера
        //
        public virtual void Store(CashServerDriverSettings s)
        {
        }

        public virtual void Load(CashServerDriverSettings s)
        {
        }

        //
        // настройки линейки Video
        //
        public virtual void Store(VideoDriverSettings s)
        {
        }

        public virtual void Load(VideoDriverSettings s)
        {
        }

        //
        // настройки линейки Video
        //
        public virtual void Store(IPCameraSettings s)
        {
        }

        public virtual void Load(IPCameraSettings s)
        {
        }

        //
        // настройки линейки Firich/Epson
        //
        public virtual void Store(FV2029Settings s)
        {
        }

        public virtual void Load(FV2029Settings s)
        {
        }

        //
        // настройки линейки OPOS (экран покупателя)
        //
        public virtual void Store(OposCustomerDisplaySettings s)
        {
        }

        public virtual void Load(OposCustomerDisplaySettings s)
        {
        }

        //
        // настройки линейки ПЛАС-ТЕК
        //
        public virtual void Store(PlastekSettings s)
        {
        }

        public virtual void Load(PlastekSettings s)
        {
        }

        //
        // настройки линейки ПЛАС-ТЕК (виртуальный)
        //
        public virtual void Store(VirtualPlastekSettings settings)
        {
        }

        public virtual void Load(VirtualPlastekSettings s)
        {
        }

        //
        // настройки линейки Pulsar
        //
        public virtual void Store(PulsarSettings s)
        {
        }

        public virtual void Load(PulsarSettings s)
        {
        }

        //
        // настройки системы Hoist
        //
        public virtual void Store(HoistSettings s)
        {
        }

        public virtual void Load(HoistSettings s)
        {
        }

        //
        // настройки виртуального устройства
        //
        public virtual void Store(VirtualDeviceSettings s)
        {
        }

        public virtual void Load(VirtualDeviceSettings s)
        {
        }

        //
        // настройки драйвера весы
        //
        public virtual void Store(ScaleDriverSettings s)
        {
        }

        public virtual void Load(ScaleDriverSettings s)
        {
        }

        //
        // настройки драйвера контроллер
        //
        public virtual void Store(PowerDeviceDriverSettings s)
        {
        }

        public virtual void Load(PowerDeviceDriverSettings s)
        {
        }

        //
        // настройки драйвера д/я
        //
        public virtual void Store(CashDrawerDriverSettings s)
        {
        }

        public virtual void Load(CashDrawerDriverSettings s)
        {
        }

        /// <summary>
        /// настройка внешнего устроства (плагин фронта)
        /// </summary>
        /// <param name="externalDeviceSettings"></param>
        public virtual void Store(ExternalDeviceSettings externalDeviceSettings)
        {
        }

        public virtual void Load(ExternalDeviceSettings s)
        {
        }
    }

    #endregion DeviceSettingsVisitor Class

    #region DeviceSettings Class

    public abstract partial class DeviceSettings
    {
        public abstract void Store(DeviceSettingsVisitor visitor);

        public abstract void Load(DeviceSettingsVisitor visitor);

        public abstract void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings);

        public virtual bool ValidateOfd(DeviceSettingsVisitor visitor, out string errMessage)
        {
            errMessage = string.Empty;
            return true;
        }

        public virtual bool ValidatePassword(string pass)
        {
            return true;
        }

        public abstract IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            [CanBeNull] PreviousDeviceSettings previousDeviceSettings);
    }

    #endregion DeviceSettings Class

    #region CustomerDisplaySettings Class

    public abstract partial class CustomerDisplaySettings
    {
    }

    #endregion CustomerDisplaySettings Class

    #region PortWriterDriverSettings Class

    public partial class PortWriterDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public static DeviceSettings CreateDefaultSettings()
        {
            return new PortWriterDriverSettings(
                DeviceType.PRINTER_DEVICE.DeviceTypeName,
                "LPT",
                1,
                115200,
                SerialPortFlowControl.NONE,
                PrintOrientation.DEFAULT, 
                "",
                "",
                0,
                0);
        }

        public static DeviceSettings CreateShtrih700Settings()
        {
            return new PortWriterDriverSettings(
                DeviceType.PRINTER_DEVICE.DeviceTypeName,
                "COM",
                1,
                115200,
                SerialPortFlowControl.NONE,
                PrintOrientation.DEFAULT, 
                "",
                "",
                0,
                0);
        }

        public static DeviceSettings CreateWindowsSettings()
        {
            return new PortWriterDriverSettings(
                DeviceType.PRINTER_DEVICE.DeviceTypeName,
                "WIN",
                1,
                115200,
                SerialPortFlowControl.NONE,
                PrintOrientation.DEFAULT, 
                "",
                "",
                0,
                0);
        }

        public static EscPosPrinterDriverSettings CreateEscPosSettings()
        {
            return new EscPosPrinterDriverSettings(
                DeviceType.PRINTER_DEVICE.DeviceTypeName,
                "COM",
                1,
                115200,
                SerialPortFlowControl.NONE,
                PrintOrientation.DEFAULT,
                "",
                "",
                0,
                0,
                0);
        }
    }

    #endregion PortWriterDriverSettings Class

    #region EscPosPrinterDriverSettings Class

    public partial class EscPosPrinterDriverSettings
    {
        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public static readonly IList<Pair<string, string>> PrintTextCommandDisplayNames = new List<Pair<string, string>>
        {
            Tuples.Pair("printerCodePage", Resources.UsePrinterCodepage),
            Tuples.Pair("GS v 0", Resources.Graphic_Gs_v_0),
            Tuples.Pair("GS ( L", Resources.Graphic_Gs_28_L)
        };

        public static readonly IList<Pair<string, string>> BarcodeCommandDisplayNames = new List<Pair<string, string>>
        {
            Tuples.Pair("GS k", "GS k"),
            Tuples.Pair("GS v 0", Resources.Graphic_Gs_v_0),
            Tuples.Pair("GS ( L", Resources.Graphic_Gs_28_L)
        };

        public static readonly IList<Pair<string, string>> QrCodeCommandDisplayNames = new List<Pair<string, string>>
        {
            Tuples.Pair("GS (", "GS ("),
            Tuples.Pair("GS ( k", "GS ( k"),
            Tuples.Pair("GS ( k 0", "GS ( k 0"),
            Tuples.Pair("GS v 0", Resources.Graphic_Gs_v_0),
            Tuples.Pair("GS ( L", Resources.Graphic_Gs_28_L)
        };

        public static readonly IList<Pair<string, string>> LogoCommandDisplayNames = new List<Pair<string, string>>
        {
            Tuples.Pair("FS p", "FS p"),
            Tuples.Pair("GS L", "GS L"),
            Tuples.Pair("GS ( L", "GS ( L"),
        };

        public static string GetDisplayNameByValue(IList<Pair<string, string>> source, string value)
        {
            var index = source.IndexOf(item => item.First == value);
            return index < 0 ? source.First().Second : source[index].Second;
        }

        public static string GetValueByDisplayName(IList<Pair<string, string>> source, string displayName)
        {
            var index = source.IndexOf(item => item.Second == displayName);
            return index < 0 ? source.First().First : source[index].First;
        }
    }

    #endregion EscPosPrinterDriverSettings Class

    #region OPOSPrinterDriverSettings Class

    public partial class OPOSPrinterDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public static DeviceSettings CreateDefaultSettings()
        {
            return new OPOSPrinterDriverSettings(DeviceType.PRINTER_DEVICE.DeviceTypeName, string.Empty, 19, 24, 48);
        }
    }

    #endregion OPOSPrinterDriverSettings Class

    #region ShtrihDriverSettings Class

    public partial class ShtrihDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
            previousDeviceSettings.HostAddress = HostAddress;
            previousDeviceSettings.HostPort = HostPort;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public override bool ValidatePassword(string pass)
        {
            return pass.Length > 0 && int.TryParse(pass, out _);
        }
    }

    #endregion ShtrihDriverSettings Class

    #region PrimFRDriverSettings Class

    public partial class PrimFRDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion PrimFRDriverSettings Class

    #region AzimuthPrimDriverSettings Class

    public partial class AzimuthPrimDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion AzimuthPrimDriverSettings Class

    #region AzimuthFnPrimDriverSettings Class

    public partial class AzimuthFnPrimDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion AzimuthFnPrimDriverSettings Class

    #region PilotFP410KDriverSettings Class

    public partial class PilotFP410KDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion PilotFP410KDriverSettings Class

    #region Maria301DriverSettings Class

    public partial class Maria301DriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion Maria301DriverSettings Class

    #region ArtSoftFiscalRegisterDriverSettings Class

    public partial class ArtSoftFiscalRegisterDriverSettings
    {
        private readonly List<string> artSoftModelNames = new List<string>
        {
            Resources.CashRegisterArtSoftDatecs,
            Resources.CashRegisterArtSoftKrypton,
            Resources.CashRegisterArtSoftMaria,
            Resources.CashRegisterArtSoftIkc,
            Resources.CashRegisterArtSoftMiniFP,
            Resources.CashRegisterArtSoftMiniFP2,
            Resources.CashRegisterArtSoftEthernet
        };

        public virtual List<string> GetModelNames()
        {
            return artSoftModelNames;
        }

        public const int DefaultProtocolType = 2;
        private string defaultModelName;

        public virtual string GetDefaultModelName()
        {
            if (string.IsNullOrEmpty(defaultModelName))
                defaultModelName = artSoftModelNames[DefaultProtocolType];
            return defaultModelName;
        }

        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
            previousDeviceSettings.HostAddress = HostAddress;
            previousDeviceSettings.HostPort = HostPort;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public virtual string GetModelNameOrDefault()
        {
            return (ProtocolType >= 0 && ProtocolType < artSoftModelNames.Count)
                ? artSoftModelNames[ProtocolType]
                : GetDefaultModelName();
        }
    }

    #endregion ArtSoftFiscalRegisterDriverSettings Class

    #region CheckboxDriverSettings Class

    public partial class CheckboxDriverSettings
    {
        private readonly List<string> checkboxModelNames = new List<string>
        {
            Resources.CashRegisterArtSoftMaria,
        };

        public override List<string> GetModelNames()
        {
            return checkboxModelNames;
        }

        public override string GetDefaultModelName()
        {
            return Resources.CashRegisterArtSoftMaria;
        }

        public override string GetModelNameOrDefault()
        {
            return Resources.CashRegisterArtSoftMaria;
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            base.Load(visitor);
            ProtocolType = DefaultProtocolType;
        }
    }

    #endregion CheckboxDriverSettings Class

    #region SparkDriverSettings Class

    public partial class SparkDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion SparkDriverSettings Class

    #region SparkFnDriverSettings Class

    public partial class SparkFnDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion SparkFnDriverSettings Class

    #region MercuryDriverSettings Class

    public partial class MercuryDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion MercuryDriverSettings Class

    #region Mercury130DriverSettings Class

    public partial class Mercury130DriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion Mercury130DriverSettings Class

    #region DatecsDriverSettings Class

    public partial class DatecsDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion DatecsDriverSettings Class

    #region AtolDriverSettings Class

    public partial class AtolDriverSettings
    {
        #region ModelInfo

        public class ModelInfo
        {
            public int ModelId { get; private set; }
            public string Name { get; private set; }
            public bool SupportsOfd { get; private set; }
            public int CharCount { get; private set; }

            public ModelInfo(int modelId, string name, bool supportsOfd, int charCount = 48)
            {
                ModelId = modelId;
                Name = name;
                SupportsOfd = supportsOfd;
                CharCount = charCount;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion

        [Transient]
        public readonly List<ModelInfo> AtolModels = new List<ModelInfo>
        {
            new ModelInfo(14, "АТОЛ: ФЕЛИКС-Р Ф", true, 38),
            new ModelInfo(15, "АТОЛ: ФЕЛИКС-02К / ЕНДВ", false, 20),
            new ModelInfo(24, "АТОЛ: ФЕЛИКС-РК / ЕНДВ", false, 38),
            new ModelInfo(27, "АТОЛ: ФЕЛИКС-ЗСК", false),
            new ModelInfo(23, "АТОЛ: ТОРНАДО-К (Меркурий MSK v.02)", false, 39),
            new ModelInfo(20, "АТОЛ: ТОРНАДО-Ф (МЕРКУРИЙ-114.1Ф)", true),
            new ModelInfo(13, "АТОЛ: Триум-Ф", true, 40),
            new ModelInfo(16, "АТОЛ: МЕРКУРИЙ-140Ф", true, 32),
            new ModelInfo(30, "АТОЛ: FPrint-02K / ЕНДВ", false, 56),
            new ModelInfo(31, "АТОЛ: FPrint-03K / ЕНДВ", false, 32),
            new ModelInfo(51, "АТОЛ: FPrint-11ПТК / К / ЕНДВ", false, 32),
            new ModelInfo(52, "АТОЛ: FPrint-22ПТК / К / ЕНДВ", false),
            new ModelInfo(73, "АТОЛ: FPrint-30", false, 32),
            new ModelInfo(47, "АТОЛ: FPrint-55ПТК / К / ЕНДВ", false, 36),
            new ModelInfo(53, "АТОЛ: FPrint-77ПТК / К / ЕНДВ", false, 57),
            new ModelInfo(32, "АТОЛ: FPrint-88К / ЕНДВ", false, 56),
            new ModelInfo(35, "АТОЛ: FPrint-5200К / ЕНДВ", false, 36),
            new ModelInfo(54, "АТОЛ: FPrintPay-01ПТК", false, 32),
            new ModelInfo(67, "АТОЛ 11Ф", true, 32),
            new ModelInfo(78, "АТОЛ 15Ф", true, 42),
            new ModelInfo(81, "АТОЛ 20Ф", true, 64),
            new ModelInfo(63, "АТОЛ 22Ф / FPrint-22ПТК", true),
            new ModelInfo(57, "АТОЛ 25Ф", true, 64),
            new ModelInfo(61, "АТОЛ 30Ф", true, 32),
            new ModelInfo(77, "АТОЛ 42ФС", true), //Интернет версия, без принтера
            new ModelInfo(80, "АТОЛ 50Ф", true, 42),
            new ModelInfo(64, "АТОЛ 52Ф", true, 32),
            new ModelInfo(62, "АТОЛ 55Ф", true, 36),
            new ModelInfo(75, "АТОЛ 60Ф", true, 42),
            new ModelInfo(69, "АТОЛ 77Ф", true),
            new ModelInfo(72, "АТОЛ 90Ф", true, 42),
            new ModelInfo(82, "АТОЛ 91Ф", true, 42),
            new ModelInfo(84, "АТОЛ 92Ф", true),
            new ModelInfo(76, "Казначей ФА", true), //Интернет версия, без принтера
            new ModelInfo(74, "Эвотор СТ2Ф", true, 16),
            new ModelInfo(79, "Эвотор СТ3Ф", true, 16),
            new ModelInfo(83, "Эвотор СТ5Ф", true, 16),
            new ModelInfo(33, "AТОЛ: BIXOLON-01K", false),
            new ModelInfo(41, "AТОЛ: PayVKP-80K", false, 42),
            new ModelInfo(45, "AТОЛ: PayPPU-700K", false, 42),
            new ModelInfo(46, "AТОЛ: PayCTS-2000K", false, 72),
            new ModelInfo(42, "AТОЛ: Аура-01ФР-KZ", false, 56),
            new ModelInfo(43, "AТОЛ: PayVKP-80KZ", false, 42),
            new ModelInfo(50, "AТОЛ: Wincor Nixdorf TH-230K", false),
            new ModelInfo(19, "Штрих-М: ЭЛВЕС-МИНИ-ФР-Ф 02", false, 24),
            new ModelInfo(18, "Штрих-М: ШТРИХ-ФР-Ф 03,04", false, 50),
            new ModelInfo(25, "Штрих-М: ШТРИХ-ФР-К / ПТК", false, 36),
            new ModelInfo(118, "Штрих-М: ШТРИХ-ФР-Ф / (БЕЛАРУСЬ)", false, 50),
            new ModelInfo(26, "Штрих-М: ШТРИХ-ФР-К", false, 36),
            new ModelInfo(28, "Штрих-М: ШТРИХ-МИНИ-ФР-К / ПТК", false, 50),
            new ModelInfo(107, "Штрих-М: ШТРИХ-КОМБО-ФР-К", false),
            new ModelInfo(110, "Штрих-М: ШТРИХ-М-ФР-К / ПТК", false),
            new ModelInfo(125, "Штрих-М: ШТРИХ-М-ФР-KZ", false),
            new ModelInfo(113, "Штрих-М: ШТРИХ-LIGHT-ФР-К / ПТК", false, 32),
            new ModelInfo(126, "Штрих-М: ПТК RR-01K, 02K, 04K", false),
            new ModelInfo(127, "Штрих-М: ПТК Retail-01K", false, 56),
            new ModelInfo(0, "Штрих-М: ЭЛВЕС-МИКРО-Ф (1.6)", false, 24),
            new ModelInfo(122, "Штрих-М: ЭЛВЕС-МИКРО-Ф (2.x)", false, 24),
            new ModelInfo(105, "Искра: ПРИМ-08ТК", false, 40),
            new ModelInfo(104, "Искра: ПРИМ-88ТК", false, 40),
            new ModelInfo(108, "Искра: ПРИМ-07К", false, 40),
            new ModelInfo(114, "КристаллСервис: ПИРИТ ФР01К", false, 56),
            new ModelInfo(128, "КристаллСервис: Pirit K", false, 56),
            new ModelInfo(106, "СЕРВИС ПЛЮС: СП101ФР-К/СП402ФР-К", false, 40),
            new ModelInfo(101, "ПИЛОТ: POSPrint FP410K", false),
            new ModelInfo(117, "Newton: ПОРТ FP-300/FP-550/FP-1000", false),
            new ModelInfo(120, "Newton: ПОРТ FP-60", false, 42),
            new ModelInfo(119, "Datecs: FP3530T", false),
            new ModelInfo(102, "Мультисофт: MSTAR-Ф-3", false, 64),
            new ModelInfo(111, "Мультисофт: MSTAR-ТК.1", false, 64),
            new ModelInfo(17, "Инкотекс: МЕРКУРИЙ-114.1Ф", false, 38),
            new ModelInfo(103, "РЕЗОНАНС: Мария-301МТМ", false, 43),
            new ModelInfo(109, "Юнисистем: МИНИ-ФП6", false, 42),
            new ModelInfo(116, "IKC-Техно: IKC-E260T/РФ 2160", false, 36),
            new ModelInfo(115, "NCR: NCR-001K", false, 44),
            new ModelInfo(121, "Мебиус 2К, 3К", false, 40),
            new ModelInfo(124, "Spark-801T/115K", false, 42),
            new ModelInfo(123, "Мебиус-3K ТГФР35", false, 32),
        };

        public const int DefaulModelId = 52;

        [Transient]
        private ModelInfo defaultModel;

        private ModelInfo DefaultModel
        {
            get
            {
                return defaultModel ??
                       (defaultModel = AtolModels.Single(mi => mi.ModelId == DefaulModelId));
            }
        }

        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
            previousDeviceSettings.HostAddress = HostAddress;
            previousDeviceSettings.HostPort = HostPort;
        }

        public override bool ValidateOfd(DeviceSettingsVisitor visitor, out string info)
        {
            var modelInfo = GetModelOrDefault();
            bool supportsOfd = !string.IsNullOrEmpty(ofdProtocolVersion);
            var ofdValid = modelInfo.SupportsOfd == supportsOfd;

            if (ofdValid)
            {
                info = string.Empty;
            }
            else
            {
                info = supportsOfd ? Resources.ModelDoesNotSupportOfd : Resources.ModelMustSupportOfd;
            }
            return ofdValid;
        }

        public override bool ValidatePassword(string pass)
        {
            return (pass.IsNullOrWhiteSpace() || (pass.Length <= 8));
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public ModelInfo GetModelOrDefault()
        {
            ModelInfo modelInfo = AtolModels.FirstOrDefault(mi => mi.ModelId == uModel) ?? DefaultModel;
            return modelInfo;
        }
    }

    #endregion AtolDriverSettings Class

    #region IcsDriverSettings Class

    public partial class IcsDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion IcsDriverSettings Class

    #region IksSpsDriverSettings Class

    public partial class IksSpsDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public override bool ValidatePassword(string pass)
        {
            return pass.Length > 0 && int.TryParse(pass, out _);
        }
    }

    #endregion IksSpsDriverSettings Class

    #region PosnetDriverSettings Class

    public partial class PosnetDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
            previousDeviceSettings.HostAddress = HostAddress;
            previousDeviceSettings.HostPort = HostPort;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion PosnetDriverSettings Class

    #region HrsDriverSettings Class

    /// <summary>
    /// RMS-48487 Front.Plugins: Перевести плагин HRS на API.v6
    /// Плагин устарел, и более не актуален, пользователи этого c++-устройства должны вручную перенастроиться на использование плагина Resto.Front.Api.Hrs
    /// </summary>
    [Obsolete]
    public partial class HrsDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.HostAddress = HostAddress;
            previousDeviceSettings.HostPort = HostPort;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion HrsDriverSettings Class

    #region PosnetThermalDriverSettings Class

    public partial class PosnetThermalDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }
  
        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion PosnetThermalDriverSettings Class

    #region BrioFisFm32DriverSettings Class

    public partial class BrioFisFm32DriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion BrioFisFm32DriverSettings Class

    #region AlposAf01DriverSettings Class

    public partial class AlposAf01DriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion AlposAf01DriverSettings Class

    #region PiritDriverSettings Class

    public partial class PiritDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion

    #region PiritFnDriverSettings Class

    public partial class PiritFnDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
            previousDeviceSettings.HostAddress = HostAddress;
            previousDeviceSettings.HostPort = HostPort;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion

    #region EmpirijaDriverSettings Class

    public partial class EmpirijaDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
            previousDeviceSettings.BaudRate = BaudRate;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion EmpirijaDriverSettings Class

    #region OPOSRegisterDriverSettings Class

    public partial class OPOSRegisterDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion OPOSRegisterDriverSettings Class

    #region UnisystemDriverSettings Class

    public partial class UnisystemDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            previousDeviceSettings.PortNumber = PortNumber;
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion UnisystemDriverSettings Class

    #region CashServerDriverSettings Class

    public partial class CashServerDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion CashServerDriverSettings Class

    #region VirtualDeviceSettings Class

    public partial class VirtualDeviceSettings : DeviceSettings
    {
        #region DeviceSettings Members

        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        #endregion DeviceSettings Members
    }

    #endregion VirtualDeviceSettings Class

    #region VideoDriverSettings Class

    public partial class VideoDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion VideoDriverSettings Class

    #region IPCameraSettings Class

    public partial class IPCameraSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion IPCameraSettings Class

    #region FV2029Settings Class

    public partial class FV2029Settings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion FV2029Settings Class

    #region OposCustomerDisplaySettings Class

    public partial class OposCustomerDisplaySettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion OposCustomerDisplaySettings Class

    #region PlastekSettings Class

    public partial class PlastekSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion PlastekSettings Class

    #region VirtualPlastekSettings Class

    public partial class VirtualPlastekSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion VirtualPlastekSettings Class

    #region PulsarSettings Class

    public partial class PulsarSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion

    #region HoistSettings Class

    public partial class HoistSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }
    }

    #endregion

    #region ScaleDriverSettings Class

    public partial class ScaleDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public static DeviceSettings CreateCasDefaultSettings()
        {
            return new ScaleDriverSettings(
                DeviceType.SCALE_DEVICE.DeviceTypeName,
                "COM",
                "CAS",
                1,
                19200,
                "cp866",
                "",
                0,
                0,
                0,
                string.Empty,
                5000, // ставится 0 по умолчанию, т.к. с 1 и 2 новый драйвер не работает
                1000,
                28,
                "0.00",
                0);
        }

        public static DeviceSettings CreateDigiDefaultSettings()
        {
            return new ScaleDriverSettings(
                DeviceType.SCALE_DEVICE.DeviceTypeName,
                "TCP",
                "DIGI",
                1,
                19200,
                "cp866",
                "",
                0,
                0,
                0,
                string.Empty,
                5000,
                1000,
                28,
                "0.00",
                0);
        }

        public static DeviceSettings CreateMassaDefaultSettings()
        {
            return new ScaleDriverSettings(
                DeviceType.SCALE_DEVICE.DeviceTypeName,
                "TCP",
                "Massa_Scales",
                1,
                19200,
                "cp866",
                "",
                5001,
                5001,
                5001,
                string.Empty,
                10000,
                1000,
                28,
                "0.00",
                0);
        }

        public static DeviceSettings CreateShtrihPrintDefaultSettings()
        {
            return new ScaleDriverSettings(
                DeviceType.SCALE_DEVICE.DeviceTypeName,
                "COM",
                "ShtrihPrint_Scales",
                1,
                19200,
                "cp866",
                "",
                0,
                2005,
                1111,
                "30",
                5000,
                1000,
                28,
                "0.00",
                0);
        }

        public static DeviceSettings CreateNciEcrDefaultSettings()
        {
            return new ScaleDriverSettings(
                DeviceType.SCALE_DEVICE.DeviceTypeName,
                "COM",
                "NciEcr_Scales",
                1,
                19200,
                "cp866",
                "",
                0,
                0,
                0,
                string.Empty,
                1000,
                1000,
                28,
                "0.00",
                0);
        }

        public static DeviceSettings CreateShtrihDefaultSettings()
        {
            return new ScaleDriverSettings(
                DeviceType.SCALE_DEVICE.DeviceTypeName,
                "COM",
                "Shtrih_Scales",
                1,
                9600,
                "cp866",
                "",
                0,
                0,
                0,
                string.Empty,
                1000,
                1000,
                28,
                "0.00",
                0);
        }
    }

    #endregion ScaleDriverSettings Class

    #region PowerDeviceDriverSettings Class

    public partial class PowerDeviceDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public static DeviceSettings CreateDefaultSettings()
        {
            return new PowerDeviceDriverSettings(DeviceType.POWER_DEVICE.DeviceTypeName, 1, 38400, 0);
        }
    }

    #endregion PowerDeviceDriverSettings Class

    #region CashDrawerDriverSettings Class

    public partial class CashDrawerDriverSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
            throw new NotImplementedException();
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public static DeviceSettings CreateDefaultSettings()
        {
            return new CashDrawerDriverSettings(
                DeviceType.CASH_DRAWER_DEVICE.DeviceTypeName,
                "COM",
                string.Empty,
                1,
                1);
        }

        public static DeviceSettings CreateOPOSDefaultSettings()
        {
            return new CashDrawerDriverSettings(
                DeviceType.CASH_DRAWER_DEVICE.DeviceTypeName,
                "OPOS",
                string.Empty,
                1,
                1);
        }
    }

    #endregion CashDrawerDriverSettings Class

    public partial class ExternalDeviceSettings
    {
        public override void Store(DeviceSettingsVisitor visitor)
        {
            visitor.Store(this);
        }

        public override void Load(DeviceSettingsVisitor visitor)
        {
            visitor.Load(this);
        }

        public override void FillPreviousSettings(PreviousDeviceSettings previousDeviceSettings)
        {
        }

        public override IValidator CreateConfigPage(IConfigPageCreator configPageCreator,
            PreviousDeviceSettings previousDeviceSettings)
        {
            return configPageCreator.Create(this, previousDeviceSettings);
        }

        public virtual void MergeWithDefaultSettings([NotNull] ExternalDeviceSettings defaultSettings)
        {
            if (defaultSettings.Settings != null)
                Settings = AgentDevicesHelper.MergeSettings(Settings, defaultSettings.Settings);
        }
    }

    public partial class ExternalCashRegisterSettings
    {
        public override void MergeWithDefaultSettings([NotNull] ExternalDeviceSettings defaultSettings)
        {
            base.MergeWithDefaultSettings(defaultSettings);
            if (defaultSettings is ExternalCashRegisterSettings defaultCashRegisterSettings)
            {
                if (defaultCashRegisterSettings.OfdProtocolVersion != null)
                    ofdProtocolVersion =
                        defaultCashRegisterSettings.OfdProtocolVersion.TryMergeWith(ofdProtocolVersion) as
                            ExternalDeviceCustomEnumSetting;
                if (defaultCashRegisterSettings.Font0Width != null)
                    font0Width =
                        defaultCashRegisterSettings.Font0Width.TryMergeWith(font0Width) as ExternalDeviceNumberSetting;
            }
        }
    }

    public partial class InternalCustomerDisplaySettings
    {
        public static InternalCustomerDisplaySettings CreateDefaultSettings()
        {
            return new InternalCustomerDisplaySettings
            {
                isEnabled = false,
                chars = 20,
                rows = 2,
                idleText = Resources.DeviceHierarchyBuildHierarchyWelcome,
            };
        }
    }
}