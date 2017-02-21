/*
 * SBPedals
 * 
 * This program reads serial data from an Arduino that is connected to a pedal set from the Steel Battalion controller.
 * When pedals are depressed, this program will send simulated keystrokes to the currently focused application.
 * 
 * Uses the InputSimulator library found here: https://inputsimulator.codeplex.com/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO.Ports;
using System.Threading;
using WindowsInput;

namespace SBPedals
{
    public partial class MainWindow : Window
    {
        private SerialScanner ss;       // SerialScanner object that is used to get data from the Arduino.
        private Thread serialThread;    // Thread in which the SerialScanner will read the Arduino serial port.
        private String prevGasKey;

        // These statements allow for a P/Invoke of the VkKeyScan() function to convert a character into a VirtualKeyCode
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        /*
         * Create the SerialScanner object and start executing the StartRead method in a new thread.
         */
        public MainWindow()
        {
            InitializeComponent();
            this.prevGasKey = "A";
            this.ss = new SBPedals.SerialScanner(txtSerial, txtGas, txtBrake, txtClutch);
            this.serialThread = new Thread(new ThreadStart(ss.StartRead));

            // Start the thread and wait for the thread to be closed before exiting.
            this.serialThread.Start();
           // this.txtGasKey.Text = this.prevGasKey;
            while (!this.serialThread.IsAlive);
        }

        /*
         * When the window is closing, stop the serial thread and close the COM port.
         */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Abort and join the serial thread to close the application. Be sure to close the serial port.
            ss.SetRunning(false);
            Thread.Sleep(100);          // Give the read loop time to respond before aborting and joining.
            this.serialThread.Abort();
            this.serialThread.Join();
            ss.Close();
            Console.WriteLine("Shutting down");
        }

        /*
         * This method is a P/Invoke of user32.dll's VkKeyScan() function. It allows me to pass in a character and get the 
         * WindowsInput.VirtualKeyCode constant I need to change the gas, brake, and pedal keys.
         * 
         * This method will not work properly on numpad keys or any other special keys.
         * 
         * Code borrowed from Stack Overflow: http://stackoverflow.com/questions/2898806/how-to-convert-a-character-to-key-code
         */
        public static WindowsInput.VirtualKeyCode ConvertCharToVirtualKey(char ch)
        {
            WindowsInput.VirtualKeyCode newKey;
            short vkey = VkKeyScan(ch);
            newKey = (WindowsInput.VirtualKeyCode)(vkey & 0xff);
            /* The following is code from the Stack Overflow example I looked at.
            int modifiers = vkey >> 8;
            if ((modifiers & 1) != 0) retval |= Keys.Shift;
            if ((modifiers & 2) != 0) retval |= Keys.Control;
            if ((modifiers & 4) != 0) retval |= Keys.Alt;
            */
            return newKey;
        }

        /*
         * Check the key to see if it is an invalid key, such as space. Return true if the key is valid, otherwise return false.
         */
        private static Boolean CheckKey(Key key)
        {
        if (key == Key.Space)
            return false;

        return true;
        }

        /*
         * Take the text value of the key and set the textbox text.
         */
        private void txtGasKey_KeyDown(object sender, KeyEventArgs e)
        {
            txtGasKey.Text = e.Key.ToString();
        }

        /*
         * When the gas key textbox changes, check if the string is of length 1. If it is, convert the string to a character 
         * and then convert that character to a WindowsInput.VirtualKeyCode constant. Change the gas pedal key with the setGasKey() method.
         */
        private void txtGasKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            String txt = txtGasKey.Text;
            Console.WriteLine(txt);

            // Don't convert the key if the string is empty - it will crash.
            if (txt.Length == 1)
            {
                prevGasKey = txt;
                char ch = txt.ToCharArray()[0];
                WindowsInput.VirtualKeyCode newKey = ConvertCharToVirtualKey(ch);
                Console.WriteLine(ch);
                this.ss.setGasKey(newKey);
            }
            else
            {
                txtGasKey.Text = prevGasKey;
            }
        }

        /*
        * Before setting the gas key, check if the key is valid.
        */
        private void txtGasKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // If the key is invalid, ignore the event (handled = true)
            KeyMap.LookupVKey(e.Key);

            if (!CheckKey(e.Key))
            {
                Console.WriteLine("ignored");
                e.Handled = true;
            }
        }
    }
}
