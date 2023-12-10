
namespace Resto.Framework.Common.DataTerminal
{
    #region IDataTerminal

    /// <summary>
    /// Интерфейс для описания драйвера под ТСД (терминалы сбора данных).
    /// </summary>
    public interface IDataTerminalDriver
    {
        string Name { get; set; }
        string OutputPath { get; set; }
        string InputPath { get; set; }
        string ExePath { get; set; }
        bool IsActive { get; set; }
        void Update(IDataTerminalDriver newTerminal);
    }

    #endregion IDataTerminal
}
