namespace Resto.Framework.Data
{
    /// <summary>
    /// Интерфейс для строк грида способных выполнять проверку на заполненность критическими данными
    /// </summary>
    public interface IWithEmptyCheck
    {
        bool IsEmptyRow { get; }
    }
}
