using System;
using System.Collections.Generic;
using System.Globalization;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    public static class DeviceHierarchy
    {
        #region Constants

        #endregion

        #region Constructors

        static DeviceHierarchy()
        {
            DeviceTypes = BuildHierarchy();
        }

        #endregion

        #region Fields

        private static Dictionary<string, CreateDriverDelegate> agentDrivers = new Dictionary<string, CreateDriverDelegate>();

        #endregion

        #region Properties

        public static DeviceTypeBase[] DeviceTypes { get; private set; }

        public static IReadOnlyDictionary<string, CreateDriverDelegate> AgentDrivers
        {
            get { return agentDrivers; }
        }

        #endregion

        #region Implementation

        private static DeviceTypeBase[] BuildHierarchy()
        {
            InitAgentDrivers();

            DeviceTypeBase[] deviceTypes = new DeviceTypeBase[] {

                #region PrinterDeviceType
                new PrinterDeviceType(
                    new []{
                        //
                        // принтеры Posiflex
                        //
                        new DriverSettings(
                            "PosiflexDriver",
                            agentDrivers["PosiflexDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры Comstar
                        //
                        new DriverSettings(
                            "ComstarDriver",
                            agentDrivers["ComstarDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры SP298
                        //
                        new DriverSettings(
                            "SP298Driver",
                            agentDrivers["SP298Driver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры STAR SPxxx
                        //
                        new DriverSettings(
                            "SPxxxDriver",
                            agentDrivers["SPxxxDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры STAR TSPxxx
                        //
                        new DriverSettings(
                            "TSPxxxDriver",
                            agentDrivers["TSPxxxDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры STAR SMxxx
                        //
                        new DriverSettings(
                            "SMxxxDriver",
                            agentDrivers["SMxxxDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры ESC/POS
                        //
                        new DriverSettings(
                            "EscPosPrinterDriver",
                            agentDrivers["EscPosPrinterDriver"],
                            PortWriterDriverSettings.CreateEscPosSettings),
                        //
                        // принтеры Epson TM-T88
                        //
                        new DriverSettings(
                            "EpsonT88Driver",
                            agentDrivers["EpsonT88Driver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры Epson TM-U220
                        //
                        new DriverSettings(
                            "EpsonTMU220Driver",
                            agentDrivers["EpsonTMU220Driver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры Epson TM-U295
                        //
                        new DriverSettings(
                            "EpsonTMU295Driver",
                            agentDrivers["EpsonTMU295Driver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры Btp-R580II
                        //
                        new DriverSettings(
                            "BtpR580IIDriver",
                            agentDrivers["BtpR580IIDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры Checkway
                        //
                        new DriverSettings(
                            "CheckwayDriver",
                            agentDrivers["CheckwayDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры АТОЛ
                        //
                        new DriverSettings(
                            "AtolPrinterDriver",
                            agentDrivers["AtolPrinterDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры Citizen CT-Sxxx
                        //
                        new DriverSettings(
                            "CitizenCTSxxxDriver",
                            agentDrivers["CitizenCTSxxxDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры SPrint TM200
                        //
                        new DriverSettings(
                            "SPrintTM200Driver",
                            agentDrivers["SPrintTM200Driver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры SPrint TM200 Min
                        //
                        new DriverSettings(
                            "SPrintTM200SimpleDriver",
                            agentDrivers["SPrintTM200SimpleDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры Штрих-600
                        //
                        new DriverSettings(
                            "Shtrih600Driver",
                            agentDrivers["Shtrih600Driver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // принтеры ШТРИХ-700
                        //
                        new DriverSettings(
                            "Shtrih700Driver",
                            agentDrivers["Shtrih700Driver"],
                            PortWriterDriverSettings.CreateShtrih700Settings),
                        //
                        // OPOS принтеры
                        //
                        new DriverSettings(
                            "OPOSPrinterDriver",
                            agentDrivers["OPOSPrinterDriver"],
                            OPOSPrinterDriverSettings.CreateDefaultSettings),
                        //
                        // Windows принтер
                        //
                        new DriverSettings(
                            "WindowsPrinterDriver",
                            agentDrivers["WindowsPrinterDriver"],
                            PortWriterDriverSettings.CreateWindowsSettings),
                        //
                        // Zebra Epl принтеры этикеток 
                        //
                        new DriverSettings(
                            "ZebraEplDriver",
                            agentDrivers["ZebraEplDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // АТОЛ BP21 принтер этикеток 
                        //
                        new DriverSettings(
                            "BP21PrinterDriver",
                            agentDrivers["BP21PrinterDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // АТОЛ BP41 принтер этикеток 
                        //
                        new DriverSettings(
                            "BP41PrinterDriver",
                            agentDrivers["BP41PrinterDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // TSPL принтер этикеток 
                        //
                        new DriverSettings(
                            "TsplPrinterDriver",
                            agentDrivers["TsplPrinterDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // Принтер Poscenter 
                        //
                        new DriverSettings(
                            "PoscenterDriver",
                            agentDrivers["PoscenterDriver"],
                            PortWriterDriverSettings.CreateDefaultSettings),
                        //
                        // PrinterNullDriver
                        //
                        new DriverSettings(
                            "PrinterNullDriver",
                            agentDrivers["PrinterNullDriver"],
                            () => null),
                    }
                ),
                #endregion

                #region CashRegisterDeviceType
                new CashRegisterDeviceType(
                    new []{
                        //
                        // серия ШТРИХ-ФР
                        //
                        new DriverSettings(
                            "ShtrihDriver",
                            agentDrivers["ShtrihDriver"],
                            (() => new ShtrihDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, ConnectionInterface.COM, 1, 19200, 7778, string.Empty))),
                        //
                        // серия ШТРИХ-ФР-Ф
                        //
                        new DriverSettings(
                            "ShtrihDriverFRF",
                            agentDrivers["ShtrihDriverFRF"],
                            () => new ShtrihDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, ConnectionInterface.COM, 1, 4800, 7778, string.Empty)),
                        //
                        // серия ПРИМ-ФР
                        //
                        new DriverSettings(
                            "PrimFRDriver",
                            agentDrivers["PrimFRDriver"],
                            () => new PrimFRDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600)),

                        //
                        // серия ПРИМ-ФР (Azimuth)
                        //
                        new DriverSettings(
                            "AzimuthPrimDriver",
                            agentDrivers["AzimuthPrimDriver"],
                            () => new AzimuthPrimDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600)),

                        //
                        // серия ПРИМ-ФР с ФН (Azimuth)
                        //
                        new DriverSettings(
                            "AzimuthFnPrimDriver",
                            agentDrivers["AzimuthFnPrimDriver"],
                            () => new AzimuthFnPrimDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600, TaxationSystem.COMMON, string.Empty)),

                        //
                        // Пилот FP-410K
                        //
                        new DriverSettings(
                            "PilotFP410KDriver",
                            agentDrivers["PilotFP410KDriver"],
                            () => new PilotFP410KDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1)),

                        //
                        // Мария-301 МТМ
                        //
                        new DriverSettings(
                            "Maria301Driver",
                            agentDrivers["Maria301Driver"],
                            () => new Maria301DriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 115200,"1111111111")),

                        //
                        // Универсальный драйвер украинских ФР от АртСофт
                        //
                        new DriverSettings(
                            "ArtSoftFiscalRegisterDriver",
                            agentDrivers["ArtSoftFiscalRegisterDriver"],
                            () => new ArtSoftFiscalRegisterDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, ConnectionInterface.COM, 1, 115200,
                                string.Empty, 80, 2, InternalCustomerDisplaySettings.CreateDefaultSettings())),

                        //
                        // Универсальный драйвер украинских ФР Checkbox
                        //
                        new DriverSettings(
                            "CheckboxDriver",
                            agentDrivers["CheckboxDriver"],
                            () => new CheckboxDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, ConnectionInterface.COM, 1, 115200,
                                string.Empty, 80, 2, InternalCustomerDisplaySettings.CreateDefaultSettings())),

                        //
                        // серия СПАРК
                        //
                        new DriverSettings(
                            "SparkDriver",
                            agentDrivers["SparkDriver"],
                            () => new SparkDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600, 1, "000000", RestaurantPrintOption.TAPE_AND_CR, CommonConstants.DefaultPaymentRegisterId)),

                        //
                        // серия СПАРК с ФН
                        //
                        new DriverSettings(
                            "SparkFnDriver",
                            agentDrivers["SparkFnDriver"],
                            () => new SparkFnDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, "111111", "1.0")),

                        //
                        // серия МЕРКУРИЙ MS-K
                        //
                        new DriverSettings(
                            "MercuryDriver",
                            agentDrivers["MercuryDriver"],
                            () => new MercuryDriverSettings(
                                      DeviceType.CASH_REGISTER.DeviceTypeName,
                                      1,
                                      57600,
                                      1,
                                      "0000",
                                      2 + (50 << 8) + (50 << 16) + (1 << 24))),
                        //
                        // серия MSTAR-TK
                        //
                        new DriverSettings(
                            "MstarTKDriver",
                            agentDrivers["MstarTKDriver"],
                            () => new MercuryDriverSettings(
                                      DeviceType.CASH_REGISTER.DeviceTypeName,
                                      1,
                                      9600,
                                      1,
                                      "0000",
                                      2 + (50 << 8) + (50 << 16) + (1 << 24))),
                        //
                        // серия МЕРКУРИЙ 130
                        //
                        new DriverSettings(
                            "Mercury130Driver",
                            agentDrivers["Mercury130Driver"],
                            () => new Mercury130DriverSettings(
                                      DeviceType.CASH_REGISTER.DeviceTypeName,
                                      1,
                                      9600,
                                      800,    // TODO
                                      "0000",
                                      1,
                                      0,
                                      1)),
                        //
                        // серия АТОЛ
                        //
                        new DriverSettings(
                            "AtolDriver",
                            agentDrivers["AtolDriver"],
                            () => new AtolDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, ConnectionInterface.COM, 1, 115200, String.Empty, 5555, string.Empty)),
                        //
                        // серия ICS (IKS-MARKET)
                        //
                        new DriverSettings(
                            "IcsDriver",
                            agentDrivers["IcsDriver"],
                            () => new IcsDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600)),
                        //
                        // серия Pirit 
                        //
                        new DriverSettings(
                            "PiritDriver",
                            agentDrivers["PiritDriver"],
                            () => new PiritDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 57600)),
                        //
                        // серия Pirit с ФН
                        //
                        new DriverSettings(
                            "PiritFnDriver",
                            agentDrivers["PiritFnDriver"],
                            () => new PiritFnDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, ConnectionInterface.COM, 1, 57600, TaxationSystem.COMMON, "1.0")),

                        //
                        // серия IKS (STRAUJOS PREKYBOS SISTEMOS)
                        //
                        new DriverSettings(
                            "IksSpsDriver",
                            agentDrivers["IksSpsDriver"],
                            () => new IksSpsDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600, 0, InternalCustomerDisplaySettings.CreateDefaultSettings())),
                        //
                        // серия Posnet
                        //
                        new DriverSettings(
                            "PosnetDriver",
                            agentDrivers["PosnetDriver"],
                            () => new PosnetDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, ConnectionInterface.COM, 1, 9600, "", 0,
                                InternalCustomerDisplaySettings.CreateDefaultSettings()) {CodePage = "1250"}),
                            //
                        // серия PosnetThermal
                        //
                        new DriverSettings(
                            "PosnetThermalDriver",
                            agentDrivers["PosnetThermalDriver"],
                            () => new PosnetThermalDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600)),
                        //
                        // серия BRIO Fis-FM
                        //
                        new DriverSettings(
                            "BrioFisFm32Driver",
                            agentDrivers["BrioFisFm32Driver"],
                            () => new BrioFisFm32DriverSettings()),
                            //
                        // серия ALPOS AF01
                        //
                        new DriverSettings(
                            "AlposAf01Driver",
                            agentDrivers["AlposAf01Driver"],
                            () => new AlposAf01DriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, InternalCustomerDisplaySettings.CreateDefaultSettings())),
                        //
                        // серия Empirija
                        //
                        new DriverSettings(
                            "EmpirijaDriver",
                            agentDrivers["EmpirijaDriver"],
                            () => new EmpirijaDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600)),
                        

                        //
                        // серия OPOSRegister
                        //
                        new DriverSettings(
                            "OPOSRegisterDriver",
                            agentDrivers["OPOSRegisterDriver"],
                            () => new OPOSRegisterDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, string.Empty, "0", "1")),
                        //
                        // серия Datecs
                        //
                        new DriverSettings(
                            "DatecsDriver",
                            agentDrivers["DatecsDriver"],
                            () => new DatecsDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, 9600)),
                        //
                        // серия Юнисистем
                        //
                        new DriverSettings(
                            "UnisystemDriver",
                            agentDrivers["UnisystemDriver"],
                            () => new UnisystemDriverSettings(DeviceType.CASH_REGISTER.DeviceTypeName, 1, "12345")),
                        
                        //
                        // виртуальная касса
                        //
                        new DriverSettings(
                            String.Empty,
                            () =>
                            {
                                var stub = new ChequePrinterStub(null);
                                stub.UpdateType(AgentDriverType.Front);
                                return stub;
                            },
                            () => null),
                        //
                        // внешняя виртуальная касса
                        //
                        new DriverSettings(
                            String.Empty,
                            () =>
                            {
                                var stub = new ChequePrinterStub(null);
                                stub.UpdateType(AgentDriverType.Virtual);
                                return stub;
                            },
                            () => null),
                        //
                        // касса ChequePrinterNullDriver
                        //
                        new DriverSettings(
                            "ChequePrinterNullDriver",
                            agentDrivers["ChequePrinterNullDriver"],
                            () => null),
                    }
                ),
                #endregion

                #region CameraDeviceType
                new CameraDeviceType(
                    new []{
                        new DriverSettings(
                            "VideoDriver",
                            agentDrivers["VideoDriver"],
                            () => new VideoDriverSettings()),
                        new DriverSettings(
                            "IPCamera",
                            agentDrivers["IPCamera"],
                            () => new IPCameraSettings())
                    }
                ),
                #endregion

                #region CustomerDisplayDeviceType
                new CustomerDisplayDeviceType(
                    new []{
                        //
                        // модель Firich/Epson
                        //
                        new DriverSettings(
                            "FV2029Driver",
                            agentDrivers["FV2029Driver"],
                            () => new FV2029Settings(
                                      DeviceType.CUSTOMER_DISPLAY.DeviceTypeName,
                                      10,
                                      60000,
                                      Resources.DeviceHierarchyBuildHierarchyWelcome,
                                      "COM",
                                      1,
                                      9600,
                                      "Firich",
                                      CultureInfo.CurrentCulture.TextInfo.OEMCodePage.ToString(CultureInfo.InvariantCulture),
                                      true)
                        ),
                        //
                        // OPOS экран покупателя
                        //
                        new DriverSettings(
                            "OposCustomerDisplay",
                            agentDrivers["OposCustomerDisplay"],
                            () => new OposCustomerDisplaySettings(
                                    DeviceType.CUSTOMER_DISPLAY.DeviceTypeName,
                                    10,
                                    60000,
                                    Resources.DeviceHierarchyBuildHierarchyWelcome,
                                    "CustomerDisplay"))
                    }
                ),
                #endregion

                #region CardProcessingDeviceType
                new CardProcessingDeviceType(
                    new []{
                        //
                        // ПЛАС-ТЕК
                        //
                        new DriverSettings(
                            "PlastekDriver",
                            agentDrivers["PlastekDriver"],
                            (() => new PlastekSettings(
                                       DeviceType.CARD_PROCESSING.DeviceTypeName,
                                       "https://localhost/",
                                       1,
                                       1,
                                       1,
                                       "",
                                       ""))),
                        //
                        // Виртуальный ПЛАС-ТЕК
                        //
                        new DriverSettings(
                            "VirtualPlastekDriver",
                            agentDrivers["VirtualPlastekDriver"],
                            (() => null)),
                        //
                        // Pulsar
                        //
                        new DriverSettings(
                            "PulsarDriver",
                            agentDrivers["PulsarDriver"],
                            () => new PulsarSettings(
                                      DeviceType.CARD_PROCESSING.DeviceTypeName,
                                      "643", // код 810 убран т.к. ЭкспоБанк использует старые коды валют
                                      60,
                                      180,
                                      "127.0.0.1",
                                      3030)
                        ),
                        //
                        // Клубные карты
                        //
                        new DriverSettings(
                            String.Empty,
                            () => new DiscountCardDriver(),
                            () => null
                            ),
                        //
                        // Лаки Тикет
                        //
                        new DriverSettings(
                            String.Empty,
                            () => new LuckyTicketDriver(),
                            () => null
                            ),
                        //
                        // TRPOS
                        //
                        new DriverSettings(
                            String.Empty,
                            () => new TrposDriver(),
                            () => null
                            ),
                        //
                        // Smart Sale
                        //
                        new DriverSettings(
                            String.Empty,
                            () => new SmartSaleDriver
                            {
                                Settings = new PulsarSettings(javaDevice:DeviceType.CARD_PROCESSING.DeviceTypeName,
                                    currencyCode: "643",
                                    operationTimeout: 60,
                                    serverAddress: "127.0.0.1",
                                    serverPort: 1025,
                                    verifyTimeout: 180),
                                TerminalSettings = new PulsarTerminalSettings(comPort: 1,
                                    baudRate: 115200,
                                    controlAddress: "127.0.0.1",
                                    controlPort: 1025,
                                    controlTimeout: 20,
                                    terminalId: "00000000")
                            },
                            () => null),
                        //
                        // Эдельвейс
                        //
                        new DriverSettings(
                            String.Empty,
                            () => new EdelweissDriver(),
                            () => null
                            ),
                        //
                        // Epitome
                        //
                        new DriverSettings(
                            String.Empty,
                            () => new EpitomeDriver(),
                            () => null
                            ),
                        //
                        // Hoist
                        //
                        new DriverSettings(
                            String.Empty,
                            () => new HoistDriver(),
                            () => null
                            )
                    }
                ),
                #endregion

                #region ScaleDeviceType

                new ScaleDeviceType(
                    new []{
                        //
                        // весы линейки CAS 
                        //
                        new DriverSettings(
                            "CASScaleDriver",
                            agentDrivers["CASScaleDriver"],
                            ScaleDriverSettings.CreateCasDefaultSettings),
                        //
                        // весы линейки Масса
                        //
                        new DriverSettings(
                            "MassaScaleDriver",
                            agentDrivers["MassaScaleDriver"],
                            ScaleDriverSettings.CreateMassaDefaultSettings),
                        //
                        // весы линейки Штрих-Принт
                        //
                        new DriverSettings(
                            "ShtrihPrintScaleDriver",
                            agentDrivers["ShtrihPrintScaleDriver"],
                            ScaleDriverSettings.CreateShtrihPrintDefaultSettings),
                        //
                        // Протокол NCI-ECR
                        //
                        new DriverSettings(
                            "NciEcrScaleDriver",
                            agentDrivers["NciEcrScaleDriver"],
                            ScaleDriverSettings.CreateNciEcrDefaultSettings),
                        //
                        // весы линейки Shtrih_Scales 
                        //
                        new DriverSettings(
                            "ShtrihScaleDriver",
                            agentDrivers["ShtrihScaleDriver"],
                            ScaleDriverSettings.CreateShtrihDefaultSettings),
                        //
                        // весы линейки DIGI
                        //
                        new DriverSettings(
                            "DigiScaleDriver",
                            agentDrivers["DigiScaleDriver"],
                            ScaleDriverSettings.CreateDigiDefaultSettings),
                        //
                        // весы линейки ScaleNullDriver
                        //
                        new DriverSettings(
                            "ScaleNullDriver",
                            agentDrivers["ScaleNullDriver"],
                            () => null),
                    }
                ),

                #endregion

                #region PowerDeviceType
                new PowerDeviceType(
                    new []{
                        new DriverSettings(
                            "PowerDeviceDriver",
                            agentDrivers["PowerDeviceDriver"],
                            (() => new PowerDeviceDriverSettings(DeviceType.POWER_DEVICE.DeviceTypeName, 1, 38400, 0)))
                    }
                ),
                #endregion

                #region CashDrawerDeviceType
                new CashDrawerDeviceType(
                    new []{
                        //
                        // OPOS
                        //
                        new DriverSettings(
                            "OPOSCashDrawerDriver",
                            agentDrivers["OPOSCashDrawerDriver"],
                            CashDrawerDriverSettings.CreateOPOSDefaultSettings)
                    }
                ),
                #endregion
            };

            return deviceTypes;
        }

        private static void InitAgentDrivers()
        {
            agentDrivers = new Dictionary<string, CreateDriverDelegate>
            {
                #region PrinterDeviceType
                {"PosiflexDriver", (() => new PosiflexDriver())},
                {"ComstarDriver", (() => new ComstarDriver())},
                {"SP298Driver", (() => new SP298Driver())},
                {"SPxxxDriver", (() => new SPxxxDriver())},
                {"TSPxxxDriver", (() => new TSPxxxDriver())},
                {"SMxxxDriver", (() => new SMxxxDriver())},
                {"EscPosPrinterDriver", (() => new EscPosPrinterDriver())},
                {"EpsonT88Driver", (() => new EpsonT88Driver())},
                {"EpsonTMU220Driver", (() => new EpsonTMU220Driver())},
                {"EpsonTMU295Driver", (() => new EpsonTMU295Driver())},
                {"BtpR580IIDriver", (() => new BtpR580IIDriver())},
                {"CheckwayDriver", (() => new CheckwayDriver())},
                {"AtolPrinterDriver", (() => new AtolPrinterDriver())},
                {"CitizenCTSxxxDriver", (() => new CitizenCTSxxxDriver())},
                {"SPrintTM200Driver", (() => new SPrintTM200Driver())},
                {"SPrintTM200SimpleDriver", (() => new SPrintTM200SimpleDriver())},
                {"Shtrih600Driver", (() => new Shtrih600Driver())},
                {"Shtrih700Driver", (() => new Shtrih700Driver())},
                {"OPOSPrinterDriver", (() => new OPOSPrinterDriver())},
                {"WindowsPrinterDriver", (() => new WindowsPrinterDriver())},
                {"ZebraEplDriver", (() => new ZebraEplDriver())},
                {"BP21PrinterDriver", () => new BP21PrinterDriver()},
                {"BP41PrinterDriver", () => new BP41PrinterDriver()},
                {"TsplPrinterDriver", () => new TsplPrinterDriver()},
                {"PoscenterDriver", () => new PoscenterDriver()},
                #endregion

                #region CashRegisterDeviceType
                {"ShtrihDriver", (() => new ShtrihDriver())},
                {"ShtrihDriverFRF", (() => new ShtrihFRFDriver())},
                {"PrimFRDriver", (() => new PrimFRDriver())},
                {"AzimuthPrimDriver", (() => new AzimuthPrimDriver())},
                {"AzimuthFnPrimDriver", (() => new AzimuthFnPrimDriver())},
                {"PilotFP410KDriver", (() => new PilotFP410KDriver())},
                {"Maria301Driver", (() => new Maria301Driver())},
                {"ArtSoftFiscalRegisterDriver", (() => new ArtSoftFiscalRegisterDriver())},
                {"CheckboxDriver", () => new CheckboxDriver()},
                {"SparkDriver", (() => new SparkDriver())},
                {"SparkFnDriver", (() => new SparkFnDriver())},
                {"MercuryDriver", (() => new MercuryDriver())},
                {"MstarTKDriver", (() => new MstarTKDriver())},
                {"Mercury130Driver", (() => new Mercury130Driver())},
                {"AtolDriver", (() => new AtolDriver())},
                {"IcsDriver", (() => new IcsDriver())},
                {"PiritDriver", (() => new PiritDriver())},
                {"PiritFnDriver", (() => new PiritFnDriver())},
                {"IksSpsDriver", (() => new IksSpsDriver())},
                {"PosnetDriver", (() => new PosnetDriver())},
                {"PosnetThermalDriver", (() => new PosnetThermalDriver())},
                {"BrioFisFm32Driver", (() => new BrioFisFm32Driver())},
                {"AlposAf01Driver", (() => new AlposAf01Driver())},
                {"EmpirijaDriver", (() => new EmpirijaDriver())},
                {"OPOSRegisterDriver", (() => new OPOSRegisterDriver())},
                {"DatecsDriver", (() => new DatecsDriver())},
                {"UnisystemDriver", (() => new UnisystemDriver())},
                #endregion

                #region CameraDeviceType
                {"VideoDriver", (() => new CameraDriver())},
                {"IPCamera", (() => new IPCameraDriver())},
                #endregion

                #region CustomerDisplayDeviceType
                {"FV2029Driver", (() => new FV2029Driver())},
                {"OposCustomerDisplay", (() => new OposCustomerDisplayDriver())},
                #endregion

                #region CardProcessingDeviceType
                {"PlastekDriver", (() => new PlastekDriver())},
                {"VirtualPlastekDriver", (() => new VirtualPlastekDriver())},
                {"PulsarDriver", (() => new PulsarDriver())},
                #endregion

                #region ScaleDeviceType
                {"CASScaleDriver", (() => new CASScaleDriver())},
                {"MassaScaleDriver", (() => new MassaScaleDriver())},
                {"ShtrihPrintScaleDriver", (() => new ShtrihPrintScaleDriver())},
                {"NciEcrScaleDriver", (() => new NciEcrScaleDriver())},
                {"ShtrihScaleDriver", (() => new ShtrihScaleDriver())},
                {"DigiScaleDriver", (() => new DigiScaleDriver())},
                #endregion

                #region PowerDeviceType
                {"PowerDeviceDriver", (() => new PowerDeviceDriver())},
                #endregion

                #region CashDrawerDeviceType
                {"OPOSCashDrawerDriver", (() => new OPOSCashDrawerDriver())},
                #endregion

                #region NullDriverType
		        {"ChequePrinterNullDriver", (() => new ChequePrinterNullDriver())},
                {"PrinterNullDriver", (() => new PrinterNullDriver())},
                {"ScaleNullDriver", (() => new ScaleNullDriver())}, 
	            #endregion
            };
        }

        #endregion
    }
}
