using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using log4net;
using RazorEngine;
using RazorEngine.Templating;
using Resto.Common;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.Log;
using Resto.Front.PrintTemplates.Razor;
using Resto.Front.PrintTemplates.Reports.TemplateModels;

namespace Resto.Front.PrintTemplates.Reports
{
    public static class RazorRunner
    {
        private static ILog Log => LogFactory.Instance.GetLogger(typeof(RazorRunner));

        static RazorRunner()
        {
            CompilerServiceFactory.Register();
        }

        /// <exception cref="Exceptions.TemplateCompilationException">
        /// Шаблон не удалось скомпилировать.
        /// </exception>
        /// <exception cref="Exceptions.TemplateExecutionException">
        /// В процессе выполнения шаблона произошла ошибка.
        /// </exception>
        /// <exception cref="Exceptions.InvalidTemplateResultException">
        /// Шаблон вернул некорректный результат.
        /// </exception>
        [NotNull, PublicAPI]
        public static XDocument GetDocument([NotNull] ITemplateModel model, [NotNull] string template)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            return model.RunTemplateFor(template, TemplatesCache[template], Log);
        }

        /// <exception cref="Exceptions.TemplateCompilationException">
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

        private static readonly ThreadSafeCache<string, string> TemplatesCache = new ThreadSafeCache<string, string>(CompileTemplate);

        private static string CompileTemplate([NotNull] string template)
        {
            Debug.Assert(template != null);

            var templateId = CryptographyUtils.CalculateMd5Hash(template);

            const int iterCount = 5;
            for (var i = 0; i < iterCount; i++)
            {
                try
                {
                    var modelType = typeof(ITemplateModel);
                    if (!Engine.Razor.IsTemplateCached(templateId, modelType))
                    {
                        Engine.Razor.Compile(template, templateId, modelType);
                    }
                    break;
                }
                catch (TemplateParsingException e)
                {
                    Log.ErrorFormat(e, "Failed to parse template. Line: {0}, column: {1}.", e.Line, e.Column);
                    Log.LogTemplateText(template);
                    throw new Exceptions.TemplateCompilationException($"Failed to parse template, line: {e.Line}, column: {e.Column}", e);
                }
                catch (TemplateCompilationException e)
                {
                    Log.Error("Failed to compile template.", e);
                    Log.LogData("Errors", string.Join(Environment.NewLine, e.CompilerErrors));
                    Log.LogTemplateText(template);
                    if (!string.IsNullOrWhiteSpace(e.SourceCode))
                        Log.LogData("Template source code", e.SourceCode);

                    var errorMessage = "Failed to compile template";
                    if (e.CompilerErrors.Any(error => !error.IsWarning))
                        errorMessage += ", errors:" + string.Concat(e.CompilerErrors.Where(error => !error.IsWarning).Select(error => Environment.NewLine + error));

                    throw new Exceptions.TemplateCompilationException(errorMessage, e);
                }
                catch (IOException e)
                {
                    Log.ErrorFormat(e, "Failed to compile template. Iteration number: {0}.", i + 1);
                    Log.LogTemplateText(template);

                    if (i == iterCount - 1)
                        throw;
                }
            }

            return templateId;
        }
    }
}