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
            AddComPorts();
            this.ss = new SBPedals.SerialScanner(txtSerial, txtGas, txtBrake, txtClutch);
            this.serialThread = new Thread(() => ss.StartRead("COM9"));

            // Start the thread
            this.serialThread.Start();
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
            txtGasKey.Text = e.Key.ToString();
            this.ss.SetGasKey(newKey);
        }

        /*
        * Before setting the gas key, check if the key is valid.
        */
        private void txtGasKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = FilterKey(e.Key);
        }

        /*
        * Take the text value of the key and set the textbox text.
        */
        private void txtBrakeKey_KeyDown(object sender, KeyEventArgs e)
        {
            VirtualKeyCode newKey = KeyMap.LookupVKey(e.Key);
            txtBrakeKey.Text = e.Key.ToString();
            this.ss.SetBrakeKey(newKey);
        }

        /*
        * Before setting the brake key, check if the key is valid.
        */
        private void txtBrakeKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = FilterKey(e.Key);
        }

        /*
        * Before setting the brake key, check if the key is valid.
        */
        private void txtClutchKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = FilterKey(e.Key);
        }

        /*
        * Take the text value of the key and set the textbox text.
        */
        private void txtClutchKey_KeyDown(object sender, KeyEventArgs e)
        {
            VirtualKeyCode newKey = KeyMap.LookupVKey(e.Key);
            txtClutchKey.Text = e.Key.ToString();
            this.ss.SetClutchKey(newKey);
        }

        /*
         * This method checks the passed in key to see if it is invalid. If it invalid, true is returned. Otherwise, false is returned.
         * The PreviewKeyDown events will set e.Handled to the returned value to filter out invalid key strokes. 
         */
        private bool FilterKey(System.Windows.Input.Key pressedKey)
        {
            bool invalid = false;

            // If the key was not found and is equal to 0, ignore the event
            if (KeyMap.LookupVKey(pressedKey) == 0)
                invalid = true;

            // If the key is invalid, ignore the event (handled = true)
            if (!KeyMap.CheckKey(pressedKey))
            {
                Console.WriteLine("ignored");
                invalid = true;
            }

            return invalid;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangePort(cmbPorts.SelectedItem.ToString());
            Console.WriteLine(cmbPorts.SelectedItem.ToString());
        }
        
        private void AddComPorts()
        {
            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                cmbPorts.Items.Add(port);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangePort(string port)
        {
            this.ss.SetRunning(false);
            Thread.Sleep(100);
            this.ss.Close();
            Thread.Sleep(100);
            this.ss.StartRead(port);
        }
    }
}
