/*
 * This static class is responsible for converting the System.Windows.Input.Key enum (obtained from the keydown event in a textbox)
 * and to the WinowsInput.VirtualKeyCode enum used to send keys in the InputSimulator library.
 * 
 * See https://msdn.microsoft.com/en-us/library/system.windows.input.key(v=vs.110).aspx for Key Enumeration docs.
 * See https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx for Virtual Key Codes
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBPedals
{
    public static class KeyMap
    {
        static private Dictionary<System.Windows.Input.Key, WindowsInput.VirtualKeyCode> lookup;
        
        /*
         * The constructor for this class will simply add all of the valid keys to the dictionary.
         */
        static KeyMap()
        {
            InitLookup();
        }

        /*
         * Add all of the Windows.Input enum Keys as dictionary keys and their corresponding VirtualKeyCodes as values.
         */
        private static void InitLookup()
        {
            lookup = new Dictionary<System.Windows.Input.Key, WindowsInput.VirtualKeyCode>();

            lookup.Add(System.Windows.Input.Key.A, WindowsInput.VirtualKeyCode.VK_A);
            lookup.Add(System.Windows.Input.Key.NumPad6, WindowsInput.VirtualKeyCode.NUMPAD6);
            lookup.Add(System.Windows.Input.Key.D1, WindowsInput.VirtualKeyCode.VK_1);
        }

        /*
         * This method performs the lookup and returns the VirtualKeyCode if it is found.
         * If it is not found, a 0 is returned.
         */
        public static WindowsInput.VirtualKeyCode LookupVKey(System.Windows.Input.Key enumKey)
        {
            // Since vKey cannot be null, use 0 to represent an invalid key
            WindowsInput.VirtualKeyCode vKey = 0;

            try
            {
                lookup.TryGetValue(enumKey, out vKey);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid key" + enumKey.ToString() );
            }

            return vKey;
        }
    }
}
