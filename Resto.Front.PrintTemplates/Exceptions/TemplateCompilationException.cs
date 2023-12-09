using System;

namespace Resto.Front.PrintTemplates.Exceptions
{
    /// <summary>
    /// Исключение, генерируемое при ошибке компиляции шаблона.
    /// </summary>
    public sealed class TemplateCompilationException : Exception
    {
        public TemplateCompilationException(string message)
            : base(message)
        {}

        public TemplateCompilationException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}