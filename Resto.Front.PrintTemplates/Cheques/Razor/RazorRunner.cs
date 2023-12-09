using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;
using RazorEngine;
using RazorEngine.Templating;
using Resto.Framework.Common;
using log4net;
using Resto.Common;
using Resto.Framework.Common.Log;
using Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels;
using Resto.Front.PrintTemplates.Razor;

namespace Resto.Front.PrintTemplates.Cheques.Razor
{
    public static class RazorRunner
    {
        private static ILog Log => LogFactory.Instance.GetLogger(typeof(RazorRunner));

        static RazorRunner()
        {
            CompilerServiceFactory.Register();
        }
        private static readonly HashSet<Pair<string, Type>> InvalidCustomTemplates = new HashSet<Pair<string, Type>>();

        [NotNull]
        public static XDocument GetDocument<T>([NotNull] T model, [NotNull] string defaultTemplate, [CanBeNull] string customTemplate)
             where T : class, ITemplateRootModel
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (defaultTemplate == null)
                throw new ArgumentNullException(nameof(defaultTemplate));

            if (customTemplate == null)
                return GetDocument(model, defaultTemplate);

            if (InvalidCustomTemplates.Contains(new Pair<string, Type>(customTemplate, typeof(T))))
            {
                Log.Debug("Custom template is invalid, creating document markup with default template.");
                return GetDocument(model, defaultTemplate);
            }

            try
            {
                return GetDocument(model, customTemplate);
            }
            catch (Exceptions.TemplateCompilationException)
            {
                InvalidCustomTemplates.Add(new Pair<string, Type>(customTemplate, typeof(T)));
            }
            catch (Exceptions.TemplateExecutionException)
            { }
            catch (Exceptions.InvalidTemplateResultException)
            { }

            return GetDocument(model, defaultTemplate);
        }

        [NotNull]
        public static XDocument GetDocument<T>([NotNull] T model, [NotNull] string template)
             where T : class, ITemplateRootModel
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            return GetDocument(template, model, typeof(T));
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
        public static XDocument GetDocument([NotNull] string template, [NotNull] ITemplateRootModel model, [NotNull] Type modelType)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));
            if (!typeof(ITemplateRootModel).IsAssignableFrom(modelType))
                throw new ArgumentOutOfRangeException(nameof(modelType), modelType, "Model type must derived from 'ITemplateRootModel'");
            if (!modelType.IsInstanceOfType(model))
                throw new ArgumentException("Model must be instance of model type", nameof(model));

            return model.RunTemplateFor(template, TemplatesCache[new Pair<string, Type>(template, modelType)], Log);
        }

        /// <exception cref="Exceptions.TemplateCompilationException">
        /// Шаблон <paramref name="template"/> не является допустимым шаблоном.
        /// </exception>
        [PublicAPI]
        public static void ValidateTemplate([NotNull] string template, [NotNull] Type modelType)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (modelType == null)
                throw new ArgumentNullException(nameof(modelType));
            if (!typeof(ITemplateRootModel).IsAssignableFrom(modelType))
                throw new ArgumentOutOfRangeException(nameof(modelType), modelType, "Model type must derived from 'ITemplateRootModel'");

            // ReSharper disable UnusedVariable
            var _ = TemplatesCache[new Pair<string, Type>(template, modelType)];
            // ReSharper restore UnusedVariable
        }

        private static readonly ThreadSafeCache<Pair<string, Type>, string> TemplatesCache =
            new ThreadSafeCache<Pair<string, Type>, string>(pair => CompileTemplate(pair.First, pair.Second));

        private static string CompileTemplate([NotNull] string template, [NotNull] Type modelType)
        {
            Debug.Assert(template != null);
            Debug.Assert(modelType != null);

            var templateId = CryptographyUtils.CalculateMd5Hash(template);

            const int iterCount = 5;
            for (var i = 0; i < iterCount; i++)
            {
                try
                {
                    if (!Engine.Razor.IsTemplateCached(templateId, typeof(ITemplateRootModel)))
                    {
                        Engine.Razor.Compile(template, templateId, typeof(ITemplateRootModel));
                    }
                    break;
                }
                catch (TemplateParsingException e)
                {
                    Log.ErrorFormat(e, "Failed to parse template. Model type: {0}, line: {1}, column: {2}.", modelType, e.Line, e.Column);
                    Log.LogTemplateText(template);
                    throw new Exceptions.TemplateCompilationException($"Failed to parse template, line: {e.Line}, column: {e.Column}.", e);
                }
                catch (TemplateCompilationException e)
                {
                    Log.ErrorFormat(e, "Failed to compile template. Model type: {0}.", modelType);
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
                    Log.ErrorFormat(e, "Failed to compile template. Model type: {0}, iteration number: {1}.", modelType, i + 1);
                    Log.LogTemplateText(template);

                    if (i == iterCount - 1)
                        throw;
                }
            }

            return templateId;
        }
    }
}