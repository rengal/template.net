using Microsoft.AspNetCore.Mvc;

namespace RenderWebApi.Controllers
{
    [ApiController]
    [Route("api/v0")]
    public class RenderController : ControllerBase
    {
        public class RenderRequest
        {
            public RenderRequest(string template, string data)
            {
                Template = template;
                Data = data;
            }

            /// <summary>
            /// Razor template
            /// </summary>
            public string Template { get; set; }
            public string Data { get; set; }
        }

        private readonly ILogger<RenderController> _logger;

        public RenderController(ILogger<RenderController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Renders a Razor template
        /// </summary>
        /// <param name="request">Template and data</param>
        /// <returns>The rendered XML document</returns>
        [HttpPost("render")]
        public IActionResult RenderRazorTemplate([FromBody] RenderRequest request)
        {
            // var dataFileName = @"G:\Brio\obmen\Razor\BillChequePizza.xml";
            // var templateFileName = @"G:\Brio\obmen\Bill1.cshtml";
            // EntityManager.EntitiesProviderFactory = new CommonEntitiesProviderFactory();
            // LogFactory.Instance.Configure(@"d:\templates.log", LogFactory.SHORT_LOG_PATTERN, new FileAppender.MinimalLock(), true, true, Settings.Default.LogFileAgeDays);
            //
            // var currency = new Currency(ChequesPreviewLocalResources.CurrencyIsoName,
            //     ChequesPreviewLocalResources.CurrencyShortName, ChequesPreviewLocalResources.CurrencyShortName2,
            //     ChequesPreviewLocalResources.CurrencyShortName2);
            //
            // var corporation = new Corporation(Guid.NewGuid(), string.Empty, ChequesPreviewLocalResources.RestaurantName, null,
            //     string.Empty, string.Empty, currency,
            //     DistributionAlgorithmType.DISTRIBUTION_NOT_SPECIFIED,
            //     VatAccounting.VAT_INCLUDED_IN_PRICE,
            //     new PersonalDataProcessingSettings(true, true, CustomerDataTransferType.SEND),
            //     new AddressShowTypeSettings(false, AddressShowType.LEGACY, false),
            //     new DateFormatSettings(WeekDays.MONDAY, 4, 1, 0, 15),
            //     AllowableDeviationAction.NOT_NOTIFY,
            //     new StoreAccountingSettings());
            //
            // CurrencyHelper.SetCurrencyProviderResolver(() => corporation);
            //
            // var dataStr = System.IO.File.ReadAllText(dataFileName);
            // var templateStr = System.IO.File.ReadAllText(templateFileName);
            // var data = ViewData.GetRazorPreviewData(dataStr);
            //
            // var markup = RazorRunner.GetDocument(data, templateStr);
            // var document = FormatDocumentOnDefaultPrinter(ConvertXDocumentToXmlElement(markup));



            var template = request.Template;
            var data = request.Data;
            return Ok("hello world");
        }
    }
}
