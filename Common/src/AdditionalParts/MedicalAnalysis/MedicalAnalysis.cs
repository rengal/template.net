using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;

// ReSharper disable CheckNamespace
namespace Resto.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Медицинский анализ
    /// </summary>
    public partial class MedicalAnalysis
    {
        public MedicalAnalysis([NotNull] MedicalAnalysisType type, DateTime? dateActivate): this(type)
        {
            this.dateActivate = dateActivate;
        }

        /// <summary>
        /// Дата окончания анализа
        /// </summary>
        public DateTime? DateEnd
        {
            get
            {
                if (Type == null)
                {
                    throw new NullReferenceException("Expected not null analysis type");
                }

                if (Type.Duration == 0)
                {
                    return null;
                }

                if (DateActivate == null)
                {
                    return null;
                }

                return ((DateTime)DateActivate).AddMonths(Type.Duration);                
            }
        }

        /// <summary>
        /// Будет ли просрочен ли анализ на указанную дату (если текущая дата будет равна <param name="limithDay"></param>)
        /// Помогает отфильтровать по признаку "осталось до окончания срока"
        /// </summary>
        /// <returns>true - просрочен, false - не просрочен</returns>
        public bool IsExpiredByDate(DateTime limithDay)
        {
            // Если анализ бессрочный или не сдан, он не может быть просрочен
            if (IsUnlimited() || IsNotHanded())
            {
                return false;
            }

            if (DateEnd == null)
            {
                throw new NullReferenceException("Expected not null NullableDateEnd if analysis is limithed and handed");
            }

            return DateEnd.Value.Date <= limithDay.Date;
        }

        /// <summary>
        /// Просрочен ли анализ на текущую дату
        /// </summary>
        /// <returns>true - просрочен, false - не просрочен</returns>
        public bool IsExpiredToday()
        {
            return IsExpiredByDate(DateTime.Today);
        }

        /// <summary>
        /// Является ли анализ бессрочным (единовременным)
        /// </summary>
        /// <returns>true - бессрочный, false - ограничен сроком</returns>
        public bool IsUnlimited()
        {
            return Type.Duration == 0;
        }

        /// <summary>
        /// Анализ еще не сдан
        /// </summary>
        /// <returns>true - не сдан, false - уже сдан</returns>
        public bool IsNotHanded()
        {
            return DateActivate == null;
        }

        /// <summary>
        /// Осталось количество месяцев
        /// </summary>
        public int? GetMonthsBeforeEnd()
        {
            if (IsUnlimited() || IsNotHanded())
            {
                return null;
            }
            
            if (DateEnd == null)
            {
                throw new NullReferenceException("Expected not null NullableDateEnd if analysis is limithed and handed");
            }

            return DateEnd.Value.Date.MonthDifference(DateTime.Today);
        }

        /// <summary>
        /// Осталось (количество дней)
        /// </summary>
        public int? GetDaysBeforeEnd()
        {
            if (IsUnlimited() || IsNotHanded())
            {
                return null;
            }

            if (DateEnd == null)
            {
                throw new NullReferenceException("Expected not null NullableDateEnd if analysis is limithed and handed");
            }

            return DateEnd.Value.Date.DayDifference(DateTime.Today);
        }

        public override string ToString()
        {
            return string.Format("{0} (Date={1})", Type, (object)DateActivate ?? "null");
        }

        /// <summary>
        /// Дата сдачи анализа в текстовом формате
        /// </summary>
        public string DateActivateText
        {
            get { return DateActivate.HasValue ? DateActivate.Value.ToString("d") : string.Empty; }
        }

        /// <summary>
        /// Дата окончания анализа в текстовом формате
        /// </summary>
        public string DateEndText
        {
            get { return DateEnd.HasValue ? DateEnd.Value.ToString("d") : string.Empty; }
        }
    }
}