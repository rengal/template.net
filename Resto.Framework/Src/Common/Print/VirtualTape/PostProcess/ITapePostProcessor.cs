using System.Collections.Generic;
using System.Xml.Linq;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess
{  
    /// <summary>
    /// Базовый интерфейс для всех постобработчиков ленты печати.
    /// </summary>
    /// <remarks>Данный интерфейс введен для того чтобы иметь возможность проводить \
    /// дополнительную обработку ленты ITape</remarks>
    /// <see cref="DefaultTapePostProcessor"/>
    /// <see cref="Epl2TapePostProcessor"/>
    public interface ITapePostProcessor
    {
        /// <summary>
        /// Установить/получить значение говорящее о том что необходимо 
        /// добавить в результат команду звонка 
        /// </summary>
        bool RingBell { get; set; }

        /// <summary>
        /// Установить/получить значение говорящее о том что необходимо 
        /// добавить в результат Esc-последовательности 
        /// </summary>
        bool UsePageEsc { get; set; }

        /// <summary>
        /// Ориентация печати из настроек принтера
        /// </summary>
        PrintOrientation PrintOrientation { get; set; }

        /// <summary>
        /// Обработать ленту и вернуть форматированную понятную для соответствующего принтера строку 
        /// </summary>        
        /// <param name="tape">Обрабатываемая лента</param>
        /// <returns>Сформированная для отправки принтеру строка</returns>
        IEnumerable<(XElement document, string text)> Process(ITape tape);
    }
}
