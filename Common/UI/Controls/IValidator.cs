namespace Resto.Common.UI.Controls
{
    public delegate void Callback();

    public interface IValidator
    {
        bool Valid { get; }
        string Info { get; }
        Callback Callback { set; }
    }
}
