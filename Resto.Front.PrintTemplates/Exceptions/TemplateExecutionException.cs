using System;

namespace Resto.Front.PrintTemplates.Exceptions
{
    /// <summary>
    /// Исключение, генерируемое при ошибке выполнения шаблона.
    /// </summary>
    public sealed class TemplateExecutionException : Exception
    {
        public TemplateExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}