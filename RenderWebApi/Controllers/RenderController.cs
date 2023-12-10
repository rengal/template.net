using log4net.Appender;
using Microsoft.AspNetCore.Mvc;
using Resto.Common.Properties;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;
using Resto.Framework.Common.Print.Tags;
using Resto.Framework.Common.Print.VirtualTape;
using Resto.Framework.Common.XmlSerialization;
using Resto.Framework.Data;
using Resto.Front.Localization.Resources;
using Resto.Front.PrintTemplates.Cheques.Razor;
using Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels;
using System.Xml.Linq;
using System.Xml;
using Resto.Framework.Common.Print.VirtualTape.Fonts;
using Resto.Framework.Src.Common.Print.VirtualTape.Fonts;
using Resto.Framework.Common.Print.VirtualTape.PostProcess;
using Resto.Front.PrintTemplates.Cheques.Resources;
using PrintOrientation = Resto.Framework.Common.Print.VirtualTape.PrintOrientation;

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

        //private readonly ILogger<RenderController> _logger;
        private static TapePostProcessorFactory tapeFactory = new();

        public RenderController(IServiceProvider provider)
        {
            ServiceProviderExtensions.SetServiceProvider(provider);
        }

        /// <summary>
        /// Renders a Razor template
        /// </summary>
        /// <param name="request">Template and data</param>
        /// <returns>The rendered XML document</returns>
        [HttpPost("render")]
        public IActionResult RenderRazorTemplate([FromBody] RenderRequest request)
        {
             var dataFileName = @"G:\Brio\obmen\Razor\BillChequePizza.xml";
             var templateFileName = @"G:\Brio\obmen\Bill1.cshtml";
            
            var dataStr = System.IO.File.ReadAllText(dataFileName);
            var templateStr = System.IO.File.ReadAllText(templateFileName);
            var data = GetRazorPreviewData(dataStr);
            
            var markup = RazorRunner.GetDocument(data, templateStr);
            var document = FormatDocumentOnDefaultPrinter(ConvertXDocumentToXmlElement(markup));
            
            //var template = request.Template;
            //var data = request.Data;
            return Ok(document.ToString());
            return Ok("hello world");
        }

        public static XmlElement ConvertXDocumentToXmlElement(XDocument xDoc)
        {
            XmlDocument xmlDoc = new XmlDocument();
            using (XmlReader reader = xDoc.CreateReader())
            {
                xmlDoc.Load(reader);
            }
            return xmlDoc.DocumentElement;
        }

        public static ITemplateRootModel GetRazorPreviewData([NotNull] string data)
        {
            return Serializer.Deserialize<ITemplateRootModel>(data, true);
        }

        public static XElement FormatDocumentOnDefaultPrinter(XmlElement documentMarkup)
        {
            var driver = new EpsonT88Driver()
            {
                WidthInDots = 576
            };
            var fontPack = GetFontPack(driver);
            var tapeHandler = tapeFactory.CreatePostProcessor(driver);
            var tape = new Tape(fontPack, CharWidthProviderFactory.DefaultCharWidthProvider, "<pagecut />", true);
            return FormatDocument(tape, documentMarkup, tapeHandler, true, TagSets.Full);
        }

        public static XElement FormatDocument(Tape tape, XmlElement documentMarkup, ITapePostProcessor tapeHandler, bool usePageEsc, Dictionary<string, ITag> tags)
        {
            tapeHandler.RingBell = documentMarkup.HasAttribute(TagUtil.Bell.Name);
            tapeHandler.UsePageEsc = usePageEsc;
            tapeHandler.PrintOrientation = PrintOrientation.Default;

            tape.Print(documentMarkup, tags);
            tapeHandler.Process(tape);
            var documentLines = tape.GetLines();

            var result = new XElement(TagDoc.Instance.Name);
            foreach (var documentLine in documentLines.Where(documentLine => documentLine.Element != null))
            {
                // ReSharper disable once PossibleNullReferenceException
                result.Add(documentLine.Element);
            }

            return result;
        }

        public static FontPack GetFontPack(PrinterDriver driver)
        {
            const int font0Width = 42;
            const int font1Width = 30;
            const int font2Width = 21;
            var fontPack = new FontPack(
                new Font
                {
                    Esc = "<f0>",
                    ReplaceUnsupportedChars = driver.ReplaceUnsupportedChars,
                    Width = font0Width
                },
                new Font
                {
                    Esc = "<f1>",
                    ReplaceUnsupportedChars = driver.ReplaceUnsupportedChars,
                    Width = font1Width
                },
                new Font
                {
                    Esc = "<f2>",
                    ReplaceUnsupportedChars = driver.ReplaceUnsupportedChars,
                    Width = font2Width
                })
            {
                BarCode = new BarCodeFont { ConvertToBarcode = driver.ConvertToBarcode, IsSupported = true },
                Logo = new LogoFont { Esc = driver.LogoBeginEsc, ConvertToLogo = driver.ConvertToLogo, IsSupported = true },
                QRCode = new QRCodeFont { ConvertToQRCode = driver.ConvertToQRCode, IsSupported = true },
                Pulse = new PulseFont { ConvertToPulse = driver.ConvertToPulse },
                Image = new ImageFont { ConvertToImage = driver.CanPrintImages ? driver.ConvertToImage : null, },
                FontGlyph = new FontGlyph
                {
                    BoldBeginEsc = driver.BoldBeginEsc,
                    BoldEndEsc = driver.BoldEndEsc,
                    ItalicBeginEsc = driver.ItalicBeginEsc,
                    ItalicEndEsc = driver.ItalicEndEsc,
                    ReverseBeginEsc = driver.ReverseBeginEsc,
                    ReverseEndEsc = driver.ReverseEndEsc,
                    UnderlineBeginEsc = driver.UnderlineBeginEsc,
                    UnderlineEndEsc = driver.UnderlineEndEsc
                }
            };
            return fontPack;
        }
    }
}
