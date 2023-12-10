using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Интерфейс записи, содержащейся в буфере обмена.
    /// </summary>
    public interface IClipboardDataRecord
    {
        /// <summary>
        /// Метод, выполняющий действия перед вставкой данных из буфера.
        /// </summary>
        void ClipboardStartPasteRecord();

        /// <summary>
        /// Метод, выполняющий действия после вставки данных из буфера.
        /// </summary>
        void ClipboardEndPasteRecord();
    }
}