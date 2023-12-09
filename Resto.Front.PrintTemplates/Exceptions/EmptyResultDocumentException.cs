namespace Resto.Front.PrintTemplates.Exceptions
{
    /// <summary>
    /// Исключение, генерируемое в случае, когда шаблон вернул пустой документ 
    /// </summary>
    public sealed class EmptyResultDocumentException : InvalidTemplateResultException
    {
        public EmptyResultDocumentException(string message)
            : base(message)
        {}
    }
}