namespace Resto.Data
{
    public partial class AllergenGroup
    {
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(NameLocal)
                ? code
                : string.Format("{0} - {1}", code, NameLocal);
        }
    }
}