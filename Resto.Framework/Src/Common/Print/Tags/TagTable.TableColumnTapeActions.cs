using System;
using System.Collections.Generic;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed partial class TagTable
    {
        private sealed class TableColumnTapeActions : Dictionary<string, Dictionary<string, Action<TableColumnTape>>>
        {
            public void Add(string name, string value, Action<TableColumnTape> action)
            {
                if (!ContainsKey(name))
                {
                    Add(name, new Dictionary<string, Action<TableColumnTape>>());
                }
                this[name].Add(value, action);
            }

            public Action<TableColumnTape> this[string name, string value]
            {
                get { return this[name][value]; }
            }
        }
    }
}
