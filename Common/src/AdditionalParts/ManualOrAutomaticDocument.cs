using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    partial interface ManualOrAutomaticDocument
    {
        // На сервере свойство редактируемости объявлено именно в этом интерфейсе.
        // Из-за ограничений ClassConverter'а в C# это свойство объявляется отдельно в каждом наследнике.
        bool? Editable { get; }

        bool? IsAutomatic { get; }
    }
}
