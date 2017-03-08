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
        static private HashSet<System.Windows.Input.Key> invalidKeys;
        
        /*
         * The constructor for this class will simply add all of the valid keys to the dictionary, and all of the invalid keys to the HashSet.
         */
        static KeyMap()
        {
            InitLookup();
            InitInvalidKeys();
        }

        /*
         * Add all of the Windows.Input enum Keys as dictionary keys and their corresponding VirtualKeyCodes as values.
         */
        private static void InitLookup()
        {
            lookup = new Dictionary<System.Windows.Input.Key, WindowsInput.VirtualKeyCode>();

            // Alphabetic keys
            lookup.Add(System.Windows.Input.Key.A, WindowsInput.VirtualKeyCode.VK_A);
            lookup.Add(System.Windows.Input.Key.B, WindowsInput.VirtualKeyCode.VK_B);
            lookup.Add(System.Windows.Input.Key.C, WindowsInput.VirtualKeyCode.VK_C);
            lookup.Add(System.Windows.Input.Key.D, WindowsInput.VirtualKeyCode.VK_D);
            lookup.Add(System.Windows.Input.Key.E, WindowsInput.VirtualKeyCode.VK_E);
            lookup.Add(System.Windows.Input.Key.F, WindowsInput.VirtualKeyCode.VK_F);
            lookup.Add(System.Windows.Input.Key.G, WindowsInput.VirtualKeyCode.VK_G);
            lookup.Add(System.Windows.Input.Key.H, WindowsInput.VirtualKeyCode.VK_H);
            lookup.Add(System.Windows.Input.Key.I, WindowsInput.VirtualKeyCode.VK_I);
            lookup.Add(System.Windows.Input.Key.J, WindowsInput.VirtualKeyCode.VK_J);
            lookup.Add(System.Windows.Input.Key.K, WindowsInput.VirtualKeyCode.VK_K);
            lookup.Add(System.Windows.Input.Key.L, WindowsInput.VirtualKeyCode.VK_L);
            lookup.Add(System.Windows.Input.Key.M, WindowsInput.VirtualKeyCode.VK_M);
            lookup.Add(System.Windows.Input.Key.N, WindowsInput.VirtualKeyCode.VK_N);
            lookup.Add(System.Windows.Input.Key.O, WindowsInput.VirtualKeyCode.VK_O);
            lookup.Add(System.Windows.Input.Key.P, WindowsInput.VirtualKeyCode.VK_P);
            lookup.Add(System.Windows.Input.Key.Q, WindowsInput.VirtualKeyCode.VK_Q);
            lookup.Add(System.Windows.Input.Key.R, WindowsInput.VirtualKeyCode.VK_R);
            lookup.Add(System.Windows.Input.Key.S, WindowsInput.VirtualKeyCode.VK_S);
            lookup.Add(System.Windows.Input.Key.T, WindowsInput.VirtualKeyCode.VK_T);
            lookup.Add(System.Windows.Input.Key.U, WindowsInput.VirtualKeyCode.VK_U);
            lookup.Add(System.Windows.Input.Key.V, WindowsInput.VirtualKeyCode.VK_V);
            lookup.Add(System.Windows.Input.Key.W, WindowsInput.VirtualKeyCode.VK_W);
            lookup.Add(System.Windows.Input.Key.X, WindowsInput.VirtualKeyCode.VK_X);
            lookup.Add(System.Windows.Input.Key.Y, WindowsInput.VirtualKeyCode.VK_Y);
            lookup.Add(System.Windows.Input.Key.Z, WindowsInput.VirtualKeyCode.VK_Z);

            // Numeric keys
            lookup.Add(System.Windows.Input.Key.D0, WindowsInput.VirtualKeyCode.VK_0);
            lookup.Add(System.Windows.Input.Key.D1, WindowsInput.VirtualKeyCode.VK_1);
            lookup.Add(System.Windows.Input.Key.D2, WindowsInput.VirtualKeyCode.VK_2);
            lookup.Add(System.Windows.Input.Key.D3, WindowsInput.VirtualKeyCode.VK_3);
            lookup.Add(System.Windows.Input.Key.D4, WindowsInput.VirtualKeyCode.VK_4);
            lookup.Add(System.Windows.Input.Key.D5, WindowsInput.VirtualKeyCode.VK_5);
            lookup.Add(System.Windows.Input.Key.D6, WindowsInput.VirtualKeyCode.VK_6);
            lookup.Add(System.Windows.Input.Key.D7, WindowsInput.VirtualKeyCode.VK_7);
            lookup.Add(System.Windows.Input.Key.D8, WindowsInput.VirtualKeyCode.VK_8);
            lookup.Add(System.Windows.Input.Key.D9, WindowsInput.VirtualKeyCode.VK_9);

            // Non-alphanumeric keys
            lookup.Add(System.Windows.Input.Key.OemPeriod, WindowsInput.VirtualKeyCode.OEM_PERIOD);

            // Numpad keys
            lookup.Add(System.Windows.Input.Key.NumPad0, WindowsInput.VirtualKeyCode.NUMPAD0);
            lookup.Add(System.Windows.Input.Key.NumPad1, WindowsInput.VirtualKeyCode.NUMPAD1);
            lookup.Add(System.Windows.Input.Key.NumPad2, WindowsInput.VirtualKeyCode.NUMPAD2);
            lookup.Add(System.Windows.Input.Key.NumPad3, WindowsInput.VirtualKeyCode.NUMPAD3);
            lookup.Add(System.Windows.Input.Key.NumPad4, WindowsInput.VirtualKeyCode.NUMPAD4);
            lookup.Add(System.Windows.Input.Key.NumPad5, WindowsInput.VirtualKeyCode.NUMPAD5);
            lookup.Add(System.Windows.Input.Key.NumPad6, WindowsInput.VirtualKeyCode.NUMPAD6);
            lookup.Add(System.Windows.Input.Key.NumPad7, WindowsInput.VirtualKeyCode.NUMPAD7);
            lookup.Add(System.Windows.Input.Key.NumPad8, WindowsInput.VirtualKeyCode.NUMPAD8);
            lookup.Add(System.Windows.Input.Key.NumPad9, WindowsInput.VirtualKeyCode.NUMPAD9);
        }

        /*
         * This constructor adds the keys I have deemed invalid to a HashSet.
         */
        public static void InitInvalidKeys()
        {
            invalidKeys = new HashSet<System.Windows.Input.Key>();

            invalidKeys.Add(System.Windows.Input.Key.Space);
            invalidKeys.Add(System.Windows.Input.Key.LeftAlt);
            invalidKeys.Add(System.Windows.Input.Key.LeftCtrl);
            invalidKeys.Add(System.Windows.Input.Key.LeftShift);
            invalidKeys.Add(System.Windows.Input.Key.RightAlt);
            invalidKeys.Add(System.Windows.Input.Key.RightCtrl);
            invalidKeys.Add(System.Windows.Input.Key.RightShift);
        }

        /*
         * Check the invalidKeys HashSet. If the key is found, the key is deemed invalid and false is returned.
         */
        public static bool CheckKey(System.Windows.Input.Key key)
        {
            return !invalidKeys.Contains(key);
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
