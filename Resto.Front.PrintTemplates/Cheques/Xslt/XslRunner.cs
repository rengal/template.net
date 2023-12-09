using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;
using log4net;

using Resto.Framework.Common.Log;
using Resto.Front.PrintTemplates.Exceptions;

namespace Resto.Front.PrintTemplates.Cheques.Xslt
{
    public static class XslRunner
    {
        private static ILog Log
        {
            get { return LogFactory.Instance.GetLogger(typeof(XslRunner)); }
        }

        private static readonly string MsXslNamespace = XNamespace.Get("urn:schemas-microsoft-com:xslt").NamespaceName;
        private static readonly string XmlnsNamespace = XNamespace.Xmlns.NamespaceName;

        #region Script Text
        private const string ScriptText = @"
        public string FormatAmount(decimal amount)
        {
            return XslRunner.FormatAmount(amount);
        }

        public string FormatFoodValueItem(decimal foodValueItem)
        {
            return XslRunner.FormatFoodValueItem(foodValueItem);
        }

        public string FormatMoney(decimal money)
        {
            return XslRunner.FormatMoney(money);
        }

        public string FormatMoneyMin(decimal money)
        {
            return XslRunner.FormatMoneyMin(money);
        }

        public string FormatMoneyInWords(decimal money)
        {
            return XslRunner.FormatMoneyInWords(money);
        }

        public string FormatPercent(decimal value)
        {
            return XslRunner.FormatPercent(value);
        }

        public string FormatTime(DateTime time)
        {
            return XslRunner.FormatTime(time);
        }

        public string FormatLongTime(DateTime time)
        {
            return XslRunner.FormatLongTime(time);
        }

        public string FormatLongDateTime(DateTime dateTime)
        {
            return XslRunner.FormatLongDateTime(dateTime);
        }

        public string FormatFullDateTime(DateTime dateTime)
        {
            return XslRunner.FormatFullDateTime(dateTime);
        }

        public string FormatDate(DateTime date)
        {
            return XslRunner.FormatDate(date);
        }

        public string FormatDateTimeCustom(DateTime date, string format)
        {
            return XslRunner.FormatDateTimeCustom(date, format);
        }

        public string FormatTimeSpan(string timeSpan, bool displaySeconds)
        {
            return XslRunner.FormatTimeSpan(timeSpan, displaySeconds);
        }

        public string Format(string format, string _1) { return string.Format(format, _1); }
        public string Format(string format, string _1, string _2) { return string.Format(format, _1, _2); }
        public string Format(string format, string _1, string _2, string _3) { return string.Format(format, _1, _2, _3); }
        public string Format(string format, string _1, string _2, string _3, string _4) { return string.Format(format, _1, _2, _3, _4); }
        public string Format(string format, string _1, string _2, string _3, string _4, string _5) { return string.Format(format, _1, _2, _3, _4, _5); }
";
        #endregion

        #region Script Methods
        [UsedImplicitly]
        public static string FormatAmount(decimal amount)
        {
            return PrintUtils.FormatAmount(amount);
        }

        [UsedImplicitly]
        public static string FormatFoodValueItem(decimal foodValueItem)
        {
            return PrintUtils.FormatFoodValueItem(foodValueItem);
        }

        [UsedImplicitly]
        public static string FormatMoney(decimal money)
        {
            return PrintUtils.FormatPrice(money);
        }

        [UsedImplicitly]
        public static string FormatMoneyMin(decimal money)
        {
            return PrintUtils.FormatPriceMin(money);
        }

        [UsedImplicitly]
        public static string FormatMoneyInWords(decimal money)
        {
            return money.CreateCurrencyStr();
        }

        [UsedImplicitly]
        public static string FormatPercent(decimal value)
        {
            return PrintUtils.FormatPercent(value);
        }

        [UsedImplicitly]
        public static string FormatTime(DateTime time)
        {
            return PrintUtils.FormatTime(time);
        }

        [UsedImplicitly]
        public static string FormatLongTime(DateTime time)
        {
            return PrintUtils.FormatLongTime(time);
        }

        [UsedImplicitly]
        public static string FormatLongDateTime(DateTime dateTime)
        {
            return PrintUtils.FormatLongDateTime(dateTime);
        }

        [UsedImplicitly]
        public static string FormatFullDateTime(DateTime dateTime)
        {
            return PrintUtils.FormatFullDateTime(dateTime);
        }

        [UsedImplicitly]
        public static string FormatDate(DateTime date)
        {
            return PrintUtils.FormatDate(date);
        }

        [UsedImplicitly]
        public static string FormatDateTimeCustom(DateTime dateTime, string format)
        {
            return PrintUtils.FormatDateTimeCustom(dateTime, format);
        }

        [UsedImplicitly]
        public static string FormatTimeSpan(string timeSpan, bool displaySeconds)
        {
            return PrintUtils.FormatTimeSpan(TimeSpan.Parse(timeSpan), displaySeconds);
        }
        #endregion

        private static readonly HashSet<string> InvalidCustomTemplates = new HashSet<string>();

        public static bool IsTemplateInvalid([NotNull] string template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            return InvalidCustomTemplates.Contains(template);
        }

        [CanBeNull]
        public static XDocument TryGetDocument([NotNull] XDocument data, [NotNull] string template)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            if (InvalidCustomTemplates.Contains(template))
            {
                Log.Debug("Template is invalid.");
                return null;
            }

            try
            {
                return GetDocument(template, data);
            }
            catch (TemplateCompilationException)
            {
                InvalidCustomTemplates.Add(template);
                return null;
            }
            catch (TemplateExecutionException)
            {
                return null;
            }
            catch (InvalidTemplateResultException)
            {
                return null;
            }
        }

        /// <exception cref="TemplateCompilationException">
        /// Шаблон не удалось скомпилировать.
        /// </exception>
        /// <exception cref="TemplateExecutionException">
        /// В процессе выполнения шаблона произошла ошибка.
        /// </exception>
        /// <exception cref="InvalidTemplateResultException">
        /// Шаблон вернул некорректный результат.
        /// </exception>
        [NotNull, PublicAPI]
        public static XDocument GetDocument([NotNull] string template, [NotNull] XDocument data)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (data == null)
                throw new ArgumentNullException(nameof(data));


            var transform = TemplatesCache[template];

            var result = new XDocument();

            try
            {
                using (var writer = result.CreateWriter())
                    transform.Transform(data.CreateNavigator(), writer);
            }
            catch (XsltException e)
            {
                Log.ErrorFormat(e, "Failed to transform template. LineNumber: {0}, LinePosition: {1}.", e.LineNumber, e.LinePosition);
                LogTemplateText(template);
                throw new TemplateExecutionException("Failed to transform template", e);
            }
            catch (XmlException e)
            {
                Log.ErrorFormat(e, "Failed to transform template. LineNumber: {0}, LinePosition: {1}.", e.LineNumber, e.LinePosition);
                LogTemplateText(template);
                throw new TemplateExecutionException("Failed to transform template", e);
            }
            catch (EncoderFallbackException e)
            {
                Log.ErrorFormat(e, "Failed to transform template. CharUnknown (char code): {0}", (int)e.CharUnknown);
                LogTemplateText(template);
                throw new TemplateExecutionException("Failed to transform template", e);
            }
            catch (InvalidOperationException e)
            {
                Log.Error("Failed to transform template.", e);
                LogTemplateText(template);
                throw new TemplateExecutionException("Failed to transform template", e);
            }

            if (result.Root == null)
            {
                Log.Error("Template returned xml document without root.");
                LogTemplateText(template);
                LogData("Template result", result.ToString());
                throw new EmptyResultDocumentException("Template returned empty document");
            }

            result.Root.Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
            return result;
        }

        /// <exception cref="TemplateCompilationException">
        /// Шаблон <paramref name="template"/> не является допустимым шаблоном.
        /// </exception>
        [PublicAPI]
        public static void ValidateTemplate([NotNull] string template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            // ReSharper disable UnusedVariable
            var _ = TemplatesCache[template];
            // ReSharper restore UnusedVariable
        }

        private static readonly ThreadSafeCache<string, XslCompiledTransform> TemplatesCache =
            new ThreadSafeCache<string, XslCompiledTransform>(CompileTemplate);

        [NotNull, Pure]
        private static XslCompiledTransform CompileTemplate([NotNull] string template)
        {
            Debug.Assert(template != null);

            try
            {
                // Разбираем xslt
                var xslt = XDocument.Parse(template);
                Debug.Assert(xslt.Root != null);
                if (xslt.Root.Attributes().Any(a => a.Name.NamespaceName == XmlnsNamespace && a.Value == MsXslNamespace))
                {
                    Log.Error("Template contains custom scripts.");
                    LogTemplateText(template);
                    throw new TemplateCompilationException("Templates is disallowed to use custom code scripts.");
                }

                // Добавляем в xslt свои скрипты
                xslt.Root.Add(
                    new XAttribute(XName.Get("msxsl", XmlnsNamespace), MsXslNamespace),
                    new XAttribute(XName.Get("iiko", XmlnsNamespace), "urn:iiko-scripts"));

                xslt.Root.AddFirst(
                    new XElement(
                        XName.Get("script", MsXslNamespace),
                        new XAttribute("language", "C#"),
                        new XAttribute("implements-prefix", "iiko"),
                        new XElement(XName.Get("assembly", MsXslNamespace), new XAttribute("name", Assembly.GetExecutingAssembly().GetName().Name)),
                        new XElement(XName.Get("using", MsXslNamespace), new XAttribute("namespace", "Resto.Front.PrintTemplates.Cheques.Xslt")),
                        ScriptText));

                // Создаём скомпилированное преобразование
                var transform = new XslCompiledTransform(false);
                transform.Load(xslt.CreateNavigator(), new XsltSettings(true, true), new XmlUrlResolver());

                return transform;
            }
            catch (XmlException e)
            {
                var errorMessage = $"Failed to parse template text as xml. LineNumber: {e.LineNumber}, LinePosition: {e.LinePosition}.";
                Log.Error(errorMessage, e);
                LogTemplateText(template);
                throw new TemplateCompilationException(errorMessage, e);
            }
            catch (XsltException e)
            {
                var errorMessage = $"Failed to compile template. LineNumber: {e.LineNumber}, LinePosition: {e.LinePosition}.";
                Log.Error(errorMessage, e);
                LogTemplateText(template);
                throw new TemplateCompilationException(errorMessage, e);
            }
        }

        private static void LogTemplateText([NotNull] string template)
        {
            Debug.Assert(template != null);

            LogData("Template text", template);
        }

        private static void LogData([NotNull] string title, [NotNull] string data)
        {
            Debug.Assert(title != null);
            Debug.Assert(data != null);

            Log.DebugFormat("{0}:{1}{2}", title, Environment.NewLine, data);
        }
    }
}