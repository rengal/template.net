namespace Resto.Data
{
    public partial class RGBColor
    {
        public override bool Equals(object obj)
        {
            return obj is RGBColor color && color.Red == Red && color.Green == Green && color.Blue == Blue;
        }
    }
}