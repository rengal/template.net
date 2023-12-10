using System.Globalization;

namespace Resto.Common
{
    public class GregorianArabicCulture : CultureInfo
    {
        GregorianCalendar calendar;
        public GregorianArabicCulture() : base("ar-SA") { }
        public override Calendar Calendar => calendar ?? (calendar = new GregorianCalendar(GregorianCalendarTypes.Arabic));
    }
}
