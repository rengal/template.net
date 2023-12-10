namespace Resto.Data
{
    public sealed partial class ProductionOrderBlankTab
    {
        public string FullTabName
        {
            get
            {
                if (Store != null)
                {
                    return string.Format("{0} - {1}", Name, Store.NameLocal);
                }

                return Name;
            }
        }

        /// <summary>
        /// Проверка эквивалентности двух табов
        /// </summary>
        /// <param name="tab1">Первый таб</param>
        /// <param name="tab2">Второй таб</param>
        /// <param name="ignoreNum"><c>true</c>, если при сравнении не нужно учитывать номера табов</param>
        /// <returns><c>true</c>, если табы эквивалентны</returns>
        public static bool AreTabsEqual(ProductionOrderBlankTab tab1, ProductionOrderBlankTab tab2, bool ignoreNum = false)
        {
            return tab1.Name == tab2.Name && Equals(tab1.Store, tab2.Store) && (ignoreNum || tab1.Num == tab2.Num);
        }
    }
}