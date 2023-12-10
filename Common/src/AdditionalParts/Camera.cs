using System;

namespace Resto.Data
{
    public partial class Camera : IComparable
    {
        public int CompareTo(object obj)
        {
            if (obj != null)
                return NameLocal.CompareTo((obj as Camera).NameLocal);
            return 1;
        }
    }
}