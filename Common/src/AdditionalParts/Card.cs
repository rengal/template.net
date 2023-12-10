namespace Resto.Data
{
    public partial class Card
    {
        public string SlipText
        {
            get
            {
                if (Slip == null)
                    return string.Empty;
                string text = Slip;
                return text.Length > 3 ? text.Substring(text.Length - 3) : text;
            }
        }
    }
}