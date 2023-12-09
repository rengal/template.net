using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;
using log4net;
using RazorEngine;
using RazorEngine.Templating;
using Resto.Framework.Common;
using Resto.Front.PrintTemplates.Exceptions;

namespace Resto.Front.PrintTemplates.Razor
{
    internal static class RazorRunnerHelpers
    {
        internal static XDocument RunTemplateFor<T>([NotNull] this T templateModel, [NotNull] string template, [NotNull] string templateId, [NotNull] ILog log)
            where T : class
        {
            if (templateModel == null)
                throw new ArgumentNullException(nameof(templateModel));
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (templateId == null)
                throw new ArgumentNullException(nameof(templateId));
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            string templateOutput;

            try
            {
                templateOutput = Engine.Razor.Run(templateId, typeof(T), templateModel);
            }
            catch (OperationCanceledException ex)
            {
                log.WarnFormat("Template running was canceled. Message: {0}", ex.Message);
                templateOutput = new XElement(CommonConstants.NoDocTag).ToString();
            }
            catch (Exception e)
            {
                log.Error("Failed to run template.", e);
                log.LogTemplateText(template);
                throw new TemplateExecutionException("Failed to run template", e);
            }

            XDocument result;
            try
            {
                result = XDocument.Parse(templateOutput);
            }
            catch (XmlException e)
            {
                log.Error("Template returned invalid xml document.", e);
                log.LogTemplateText(template);
                log.LogData("Template result", templateOutput);
                throw new InvalidTemplateResultException("Template returned invalid xml document", e);
            }

            if (result.Root == null)
            {
                log.Error("Template returned xml document without root.");
                log.LogTemplateText(template);
                log.LogData("Template result", result.ToString());
                throw new EmptyResultDocumentException("Template returned empty document");
            }

            TrimStrings(result.Root, false);
            return result;
        }

        private static void TrimStrings([NotNull] XElement element, bool preserveWhitespaces)
        {
            Debug.Assert(element != null);

            const string whitespacePreserveElementName = "whitespace-preserve";

            if (element.Name == whitespacePreserveElementName)
            {
                foreach (var childElement in element.Elements().ToList())
                {
                    TrimStrings(childElement, true);
                }

                element.AddBeforeSelf(element.Nodes());

                element.Remove();
            }
            else
            {
                foreach (var childNode in element.Nodes().ToList())
                {
                    if (childNode is XText text)
                    {
                        if (!preserveWhitespaces)
                            text.Value = text.Value.Trim();
                    }
                    else
                    {
                        if (childNode is XElement childElement)
                            TrimStrings(childElement, preserveWhitespaces);
                    }
                }
            }
        }

        internal static void LogTemplateText([NotNull] this ILog log, [NotNull] string template)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            log.LogData("Template text", template);
        }

        internal static void LogData([NotNull] this ILog log, [NotNull] string title, [NotNull] string data)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));
            if (title == null)
                throw new ArgumentNullException(nameof(title));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            log.DebugFormat("{0}:{1}{2}", title, Environment.NewLine, data);
        }
    }
}