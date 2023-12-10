using System;

namespace Resto.Data
{
    /// <summary>
    /// Тип медицинских анализов
    /// </summary>
    public partial class MedicalAnalysisType : IComparable, IComparable<MedicalAnalysisType>
    {
        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(MedicalAnalysisType other)
        {
            if (other == null)
            {
                return 1;
            }

            var value1 = ToString();
            var value2 = other.ToString();

            return String.Compare(value1, value2, StringComparison.Ordinal);
        }

        public int CompareTo(object other)
        {
            return CompareTo((MedicalAnalysisType)other);
        }
    }
}
