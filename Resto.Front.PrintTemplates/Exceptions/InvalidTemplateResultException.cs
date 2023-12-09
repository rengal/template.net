using System;

namespace Resto.Front.PrintTemplates.Exceptions
{
    /// <summary>
    /// Исключение, генерируемое в случае некорректного результата выполнения шаблона.
    /// </summary>
    public class InvalidTemplateResultException : Exception
    {
        protected InvalidTemplateResultException(string message)
            : base(message)
        {}

        public InvalidTemplateResultException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}