using System.Reflection;
using System.Collections.Generic;

[assembly: AssemblyCompany(BrandInfo.Brand)]
[assembly: AssemblyCopyright("Copyright © " + BrandInfo.Brand)]

/// <summary>
/// Данный класс автоматически сгенерирован из файла Resources/Brand/build/BrandInfo.resx.
/// Чтобы внести изменения, поменяйте вышеуказанный файл ресурсов и запустите ClassConverter или ResxToJavaConverter.
/// </summary>
internal static partial class BrandInfo
{
    public const string ServerInstance = "iikoServer";
    public const string OfficeAppWin = "iikoOffice";
    public const string Brand = "iiko";
    public const string ChainAppWin = "iikoChain";
    public const string OfficeAppWeb = "iikoWeb";
    public const string LoyaltyApp = "iikoCard";
    public const string Card5 = "iikoCard (old)";
    public const string LoyaltyAppCallCenter = "iikoCard5CallCenter";
    public const string CloudApi = "iikoTransport";
    public const string DriverApp = "iikoDeliveryMan";
    public const string WaiterApp = "iikoWaiter";
    public const string PosApp = "iikoFront";
    public const string DeliveryModule = "iikoDelivery";
    public const string FzModule = "iikoFranchise";
    public const string PosAgent = "iiko Agent";
    public const string CallCenterAppWin = "iikoCallCenter";
    public const string Plazius = "iiko.net";
    public const string DjAppWin = "iikoDJ";
    public const string CloudApiLegacy = "iiko.biz";
    public const string CloudApiLegacyUrl = "https://iiko.biz";
    public const string ExpertModule = "iikoExpert";
    public const string DeploymentCloud = "iikoCloud";
    public const string CallCenterAppWeb = "iikoCallCenter";
    public const string DocflowModule = "iikoDocFlow";
    public const string TroubleshooterModule = "iikoTroubleshooter";
    public const string TariffNano = "iikoNano";
    public const string TableService = "iikoTableService";
    public const string PosPbx = "iikoFrontPBX";
    public const string CheckOut = "iikoCheckOut";
    public const string Logistic = "iikoLogistic";
    public const string Evotor = "iikoEvotor";
    public const string ChainOperations = "iikoChainOperations";
    public const string Chain = "Chain";
    public const string Rms = "RMS";
    public const string MobileApp = "iikoTeam";
    public const string Crm = "iikoCRM";
    public const string MonitoringModule = "iikoWatchDog";
    public const string CopyrightFormat = "© 2005-{0} Resto software, inc.";
    public const string BrandWebsite = "https://iiko.ru";
    public const string BrandFzWebsite = "https://franchise.iiko.ru/";
    public const string PartnersUrl = "partners.iiko.ru";
    public const string TechSupportUrlFormat = "https://{0}/service.html?crmId={1}&salt={2}";
    public const string OfficeRssUrl = "https://integration.iiko.ru/v1/news/get";
    public const string LoyaltyAppPos = "iikoCard5 POS";

    public const bool ShowRealeaseNotesInOffice = true;
    public const bool ShowDocumentationInOffice = true;
    public const bool ShowChequeTemplateGallery = true;
    public const bool ShowExternalPinInFront = true;
    public const bool ShowEulaInOffice = true;

    public static readonly Dictionary<string, string> ReplacementDict = new Dictionary<string, string>
    {
        {"${SERVER_INSTANCE}", ServerInstance},
        {"${OFFICE_APP_WIN}", OfficeAppWin},
        {"${BRAND}", Brand},
        {"${CHAIN_APP_WIN}", ChainAppWin},
        {"${OFFICE_APP_WEB}", OfficeAppWeb},
        {"${LOYALTY_APP}", LoyaltyApp},
        {"${CARD_5}", Card5},
        {"${LOYALTY_APP_CALL_CENTER}", LoyaltyAppCallCenter},
        {"${CLOUD_API}", CloudApi},
        {"${DRIVER_APP}", DriverApp},
        {"${WAITER_APP}", WaiterApp},
        {"${POS_APP}", PosApp},
        {"${DELIVERY_MODULE}", DeliveryModule},
        {"${FZ_MODULE}", FzModule},
        {"${POS_AGENT}", PosAgent},
        {"${CALL_CENTER_APP_WIN}", CallCenterAppWin},
        {"${PLAZIUS}", Plazius},
        {"${DJ_APP_WIN}", DjAppWin},
        {"${CLOUD_API_LEGACY}", CloudApiLegacy},
        {"${CLOUD_API_LEGACY_URL}", CloudApiLegacyUrl},
        {"${EXPERT_MODULE}", ExpertModule},
        {"${DEPLOYMENT_CLOUD}", DeploymentCloud},
        {"${CALL_CENTER_APP_WEB}", CallCenterAppWeb},
        {"${DOCFLOW_MODULE}", DocflowModule},
        {"${TROUBLESHOOTER_MODULE}", TroubleshooterModule},
        {"${TARIFF_NANO}", TariffNano},
        {"${TABLE_SERVICE}", TableService},
        {"${POS_PBX}", PosPbx},
        {"${CHECK_OUT}", CheckOut},
        {"${LOGISTIC}", Logistic},
        {"${EVOTOR}", Evotor},
        {"${CHAIN_OPERATIONS}", ChainOperations},
        {"${CHAIN}", Chain},
        {"${RMS}", Rms},
        {"${MOBILE_APP}", MobileApp},
        {"${CRM}", Crm},
        {"${MONITORING_MODULE}", MonitoringModule},
        {"${COPYRIGHT_FORMAT}", CopyrightFormat},
        {"${BRAND_WEBSITE}", BrandWebsite},
        {"${BRAND_FZ_WEBSITE}", BrandFzWebsite},
        {"${PARTNERS_URL}", PartnersUrl},
        {"${TECH_SUPPORT_URL_FORMAT}", TechSupportUrlFormat},
        {"${OFFICE_RSS_URL}", OfficeRssUrl},
        {"${LOYALTY_APP_POS}", LoyaltyAppPos},
    };
}