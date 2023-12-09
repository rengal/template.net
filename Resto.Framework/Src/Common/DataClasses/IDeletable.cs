using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Framework.Data
{
    /// <summary>
    /// Интерфейс, используемый для определения, является ли объект (entity) удалённым.
    /// Пока используется для зачёркивания удаленных объектов в комплишенах (RMS-36554).
    /// Нельзя использовать напрямую <see cref="PersistedEntity.Deleted"/>, т.к. в некоторых
    /// случаях в комплишены попадают не сами <see cref="PersistedEntity"/>, а вью-модели,
    /// их содержащие (например, <see cref="ProductNumCompletionData"/>).
    /// </summary>
    public interface IDeletable
    {
        bool Deleted { get; }
    }
}
