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

        /*
         * Create the SerialScanner object and start executing the StartRead method in a new thread.
         */
        public MainWindow()
        {
            InitializeComponent();
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
         * Take the text value of the key and set the textbox text.
         */
        private void txtGasKey_KeyDown(object sender, KeyEventArgs e)
        {
            VirtualKeyCode newKey = KeyMap.LookupVKey(e.Key);
            txtGasKey.Text = newKey.ToString();
            this.ss.setGasKey(newKey);
        }

        /*
        * Before setting the gas key, check if the key is valid.
        */
        private void txtGasKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // If the key was not found and is equal to 0, ignore the event
            if (KeyMap.LookupVKey(e.Key) == 0)
                e.Handled = true;

            // If the key is invalid, ignore the event (handled = true)
            if (!KeyMap.CheckKey(e.Key))
            {
                Console.WriteLine("ignored");
                e.Handled = true;
            }
        }
    }
}
