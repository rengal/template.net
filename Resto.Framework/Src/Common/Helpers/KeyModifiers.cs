using System;

namespace Resto.Framework.Common.Helpers
{
    [Flags]
    public enum KeyModifiers
    {
        None = 0,
        Ctrl = 1 << 0,
        Alt = 1 << 2,
        Shift = 1 << 3,

        CtrlAlt = Ctrl | Alt,
        CtrlShift = Ctrl | Shift,
        AltShift = Alt | Shift,
        CtrlAltShift = Ctrl | Alt | Shift,
    }
}
