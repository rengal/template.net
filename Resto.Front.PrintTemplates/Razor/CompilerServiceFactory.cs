using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using RazorEngine;
using RazorEngine.Compilation;
using RazorEngine.Compilation.CSharp;
using RazorEngine.Compilation.ReferenceResolver;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Resto.Front.PrintTemplates.Razor
{
    internal sealed class MyIReferenceResolver : IReferenceResolver
    {
        public IEnumerable<CompilerReference> GetReferences(TypeContext context, IEnumerable<CompilerReference> includeAssemblies)
        {
            return new UseCurrentAssembliesReferenceResolver()
                .GetReferences(context, includeAssemblies)
                .Where(f => !f.GetFile().EndsWith(".winmd"));
        }
    }

    /// <summary>
    /// Специализированная фабрика для создания CompilerService, который компилирует шаблоны в нужном нам пространстве имён.
    /// </summary>
    internal sealed class CompilerServiceFactory : ICompilerServiceFactory
    {
        private static readonly TemplateServiceConfiguration TemplateConfig;

        internal static void Register()
        {
            Engine.Razor = RazorEngineService.Create(TemplateConfig);
        }

        static CompilerServiceFactory()
        {
            TemplateConfig = new TemplateServiceConfiguration { Debug = true, CompilerServiceFactory = new CompilerServiceFactory(), ReferenceResolver = new MyIReferenceResolver() };
        }

        [NotNull]
        public ICompilerService CreateCompilerService(Language language)
        {
            if (language == Language.CSharp)
                return new CSharpDirectCompilerService();

            throw new ArgumentException($"Language '{language}' not supported.");
        }
    }
}