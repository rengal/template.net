namespace Resto.Data
{
    /// <summary>
    /// Объект, имеющий признак "НДС оплачивается гостем".
    /// </summary>
    public partial interface WithSplitVat
    {
        /// <summary>
        /// Признак того, что гость платит НДС самостоятельно.
        /// По умолчанию false, т.е.НДС платит заведение общепита.
        /// </summary>
        bool SplitVat { get; }
    }
}
