using System.Linq;
using System.Windows.Input;

namespace Resto.Framework.Common.Helpers
{
    public static class KeyboardHelper
    {
        public static bool IsCtrlPressed
        {
            get { return CheckAnyKeyDown(Key.LeftCtrl, Key.RightCtrl); }
        }

        public static bool IsShiftPressed
        {
            get { return CheckAnyKeyDown(Key.LeftShift, Key.RightShift); }
        }

        public static bool IsAltPressed
        {
            get { return CheckAnyKeyDown(Key.LeftAlt, Key.RightAlt); }
        }

        public static KeyModifiers Modifiers
        {
            get
            {
                return
                    (IsCtrlPressed ? KeyModifiers.Ctrl : 0) |
                    (IsAltPressed ? KeyModifiers.Alt : 0) |
                    (IsShiftPressed ? KeyModifiers.Shift : 0);
            }
        }

        private static bool CheckAnyKeyDown(params Key[] keys)
        {
            return (keys.Aggregate(KeyStates.None, (states, key) => states | Keyboard.GetKeyStates(key)) & KeyStates.Down) == KeyStates.Down;
        }
    }
}
