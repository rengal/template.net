namespace Resto.Common
{
    /// <summary>
    /// "Режим визуализации бэка" = iikoOffice или iikoOffice Chain.
    /// Аналог серверного resto.web.ServerEditions.
    /// </summary>
    public enum BackVisualizationMode
    {
        IIKO_RMS = 1,
        IIKO_CHAIN = 2
    }
    /**
     * Тип клиента. 
     * Вынесен из GUI.Common в Common. 
     * От значения этой глобальной переменной завитит внешний вид бекофиса.
     * Также тип визуализации должен соответствовать текущей редакции сервера - нельзя подключаться 
     * чейновым клиентом к РМС - серверу.
     */
    public class Edition
    {
        /**
         * по дефолту - RMS.
         */
        public static BackVisualizationMode CurrentEdition = BackVisualizationMode.IIKO_RMS;
    }
}
