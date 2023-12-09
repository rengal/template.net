using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess
{
    /// <summary>
    /// Базовый класс для большинства подпроцессоров 
    /// </summary>
    /// <remarks>Набор свойств класса фактически повторяет свойства PrinterDriver</remarks>
    public abstract class BaseTapePostProcessor : ITapePostProcessor
    {
        protected BaseTapePostProcessor() 
        {                       
            var ascii = Encoding.ASCII;            
            PagecutEsc = ascii.GetString(new byte[] { 0x6 });
            StartEsc = ascii.GetString(new byte[] { 0x7 });
            BellEsc = ascii.GetString(new byte[] { 0x8 });
        }

        /// <summary>
        /// Обработать ленту и вернуть форматированную понятную для соответствующего принтера строку 
        /// </summary>        
        /// <param name="tape">Обрабатываемая лента</param>
        /// <returns>Сформированные для отправки принтеру строки</returns>
        public abstract IEnumerable<(XElement document, string text)> Process(ITape tape);

        /// <summary>
        /// Установить/получить значение говорящее о том что необходимо 
        /// добавить в результат команду звонка 
        /// </summary>
        //bool ITapePostProcessor.RingBell { get; set; }
        public virtual bool RingBell { get; set; }

        /// <summary>
        /// Установить/получить значение говорящее о том что необходимо 
        /// добавить в результат Esc-последовательности 
        /// </summary>
        public virtual bool UsePageEsc { get; set; }

        /// <summary>
        /// Ориентация печати из настроек принтера
        /// </summary>
        public virtual PrintOrientation PrintOrientation { get; set; }

        /// <summary>
        /// Вернуть/установить esc-код обрыва страницы
        /// </summary>
        public string PagecutEsc { get; set; }
        /// <summary>
        /// Вернуть/установить esc-код начала печати 
        /// </summary>
        public string StartEsc { get; set; }
        /// <summary>
        /// Вернуть/установить esc-код звонка
        /// </summary>
        public string BellEsc { get; set; }
      

        #region Перенос в DefaultTapeProcessor
#warning // TODO:перенести в DefaultTapeProcessor
        /// <summary>
        /// Вернуть/установить esc-код пустой линии 
        /// </summary>
        public string EmptyLinesEsc { get; set; }

        /// <summary>
        /// Вернуть/установить количество линий на странице
        /// </summary>
        public int LinesOnPageParam { get; set; }

        /// <summary>
        /// Вернуть/установить количество пустых линий
        /// </summary>
        public int EmptyLinesParam { get; set; }

        /// <summary>
        /// Вернуть/установить значение говорящее о том если ли пустые линии
        /// </summary>
        public bool HasLinesParams { get; set; }
#endregion Перенос в DefaultTapeProcessor

    }

}
