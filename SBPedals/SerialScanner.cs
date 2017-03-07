/*
* The SerialScanner class handles reading the serial data from the Arduino, parsing it, and then deciding which keys to send.
*/

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WindowsInput;

namespace SBPedals
{
    public class SerialScanner
    {
        private volatile bool running;      // Flag used to determine when to stop the thread.
        private String portName;            // Arduino COM port.
        private int baudRate;               // Arduino COM port baud rate.
        private SerialPort serialPort;      // SerialPort object instantiated based off of portName and baudRate.
        private TextBox serialDisplay;      // Textbox reference for the serial data.
        private TextBox gas;                // Textbox reference for the gas pedal data.
        private TextBox brake;              // Textbox reference for the brake pedal data.
        private TextBox clutch;             // Textbox reference for the clutch pedal data.
        private int gasThreshold;           // Minimum value that must be attained to initiate a key down event for the gas pedal.
        private int brakeThreshold;         // Minimum value that must be attained to initiate a key down event for the brake pedal.
        private int clutchTreshold;         // Minimum value that must be attained to initiate a key down event for the clutch pedal.
        private Boolean gasDown;            // Is true when the key down event is sent for the gas pedal.
        private Boolean brakeDown;          // Is true when the key down event is sent for the brake pedal.
        private Boolean clutchDown;         // Is true when the key down event is sent for the clutch pedal.
        private WindowsInput.VirtualKeyCode gasKey = WindowsInput.VirtualKeyCode.VK_D;       // Key to send for the gas pedal.
        private WindowsInput.VirtualKeyCode brakeKey = WindowsInput.VirtualKeyCode.VK_A;        // Key to send for the brake pedal.
        private WindowsInput.VirtualKeyCode clutchKey = WindowsInput.VirtualKeyCode.VK_A;    // Key to send for the clutch pedal.

        public SerialScanner(TextBox serial, TextBox gas, TextBox brake, TextBox clutch)
        {
            this.serialDisplay = serial;
            this.gas = gas;
            this.brake = brake;
            this.clutch = clutch;
            this.gasThreshold = 300;
            this.brakeThreshold = 300;
            this.clutchTreshold = 300;
            this.gasDown = false;
            this.brakeDown = false;
            this.clutchDown = false;
        }

        /*
         * Set the baud rate, COM port, and create the SerialPort object. Open the port and set the running flag to true.
         */
        private void SetupSerialCom()
        {
            this.portName = "COM9";     // Hardcoded for now, should make into a variable.
            this.baudRate = 115200;       // This is set in the Arduino sketch

            // Create a SerialPort object based on the port and baud rate and open it.
            this.serialPort = new SerialPort(portName);
            this.serialPort.BaudRate = this.baudRate;
            this.serialPort.ReadTimeout = 1000;
            this.serialPort.Open();

            this.running = true;
        }

        /*
         * This method is called when the window is closed. It simply closes the serial port.
        */
        public void Close()
        {
            this.serialPort.Close();
        }

        /*
         * This method is where the serial port is first created and opened and the data is read/parsed.
        */
        public void StartRead()
        {
            this.SetupSerialCom();
            String data = "";
            String[] pedalVals = new String[3];
            this.serialPort.DiscardInBuffer();           // This helps with preventing an exception on startup of the serial read loop

            // The main read loop
            while (running)
            {
                try
                {
                    // Read the data and parse it into the pedalVals array
                    data = this.serialPort.ReadLine();
                    pedalVals = ParseSerialData(data);

                    //Console.WriteLine(data);

                    // This dispatcher will notify the UI thread of the new values.
                    Application.Current.Dispatcher.Invoke(() => {
                        serialDisplay.Text = data;
                        if (pedalVals.Length == 3)
                        {
                            gas.Text = pedalVals[0];
                            brake.Text = pedalVals[1];
                            clutch.Text = pedalVals[2];
                        }
                    });

                    // Only evaluate the pedals (send keys) if the array has a value for each pedal.
                    if (pedalVals.Length == 3)
                    {
                        EvaluatePedals(pedalVals);
                    }

                }
                catch (Exception e)
                {
                    this.UpAllKeys();
                    Console.WriteLine(e.Message);
                }
            }
            // Before closing, send the up event for each key to prevent the key from getting stuck down.
            this.UpAllKeys();
        }

        /*
         * This method is called when the window is closed and it sets the volatile running flag to false, stopping the loop
         * when it begins a new iteration.
         */
        public void SetRunning(Boolean run)
        {
            this.running = run;
        }

        /*
         * This method is called when the user changes the gas key textbox. It simply changes which key value is sent.
         */
        public void setGasKey(WindowsInput.VirtualKeyCode newKey)
        {
            // Be sure to reset the key if it is down.
            if (gasDown)
            {
                InputSimulator.SimulateKeyUp(gasKey);
                gasDown = false;
            }

            this.gasKey = newKey;
        }

        /*
         * This method is called when the user changes the brake key textbox. It simply changes which key value is sent.
         */
        public void setBrakeKey(WindowsInput.VirtualKeyCode newKey)
        {
            // Be sure to reset the key if it is down.
            if (brakeDown)
            {
                InputSimulator.SimulateKeyUp(brakeKey);
                brakeDown = false;
            }

            this.brakeKey = newKey;
        }

        /*
         * This method is called when the user changes the clutch key textbox. It simply changes which key value is sent.
        */
        public void setClutchKey(WindowsInput.VirtualKeyCode newKey)
        {
            // Be sure to reset the key if it is down.
            if (clutchDown)
            {
                InputSimulator.SimulateKeyUp(clutchKey);
                clutchDown = false;
            }

            this.clutchKey = newKey;
        }

        /*
         * The serial data coming from the Arduino is a semicolon delimited string that should contain three values.
         * If the string contains any more or less than three values, something went wrong.
         * Index 0 = gas value
         * Index 1 = brake value
         * Index 2 = clutch value
         */
        private static String[] ParseSerialData(String data)
        {
            String[] values;

            values = data.Trim().Split(';');

            return values;
        }

        /*
         * The gas, brake, and clutch pedal values are compared to their threshold settings to determine if a key up or down event should be sent.
         */
        private void EvaluatePedals(String[] pedalVals)
        {
            int[] vals = Array.ConvertAll(pedalVals, int.Parse);
            int gas = vals[0];
            int brake = vals[1];
            int clutch = vals[2];

            try
            {
                // Gas check. If the threshold is met, send the key down event and set the gasDown flag to true.
                if (gas >= this.gasThreshold && !gasDown)
                {
                    InputSimulator.SimulateKeyDown(gasKey);
                    gasDown = true;
                }
                // Send the key up event when the threshold is no longer met and the gasDown flag is true.
                else
                {
                    if (gasDown)
                    {
                        InputSimulator.SimulateKeyUp(gasKey);
                        gasDown = false;
                    }
                }

                // Brake check. If the threshold is met, send the key down event and set the brakeDown flag to true.
                if (brake >= this.brakeThreshold && !brakeDown)
                {
                    InputSimulator.SimulateKeyDown(brakeKey);
                    brakeDown = true;
                }
                // Send the key up event when the threshold is no longer met and the brakeDown flag is true.
                else
                {
                    if (brakeDown)
                    {
                        InputSimulator.SimulateKeyUp(brakeKey);
                        brakeDown = false;
                    }
                }

                // Clutch check. If the threshold is met, send the key down event and set the clutchDown flag to true.
                if (clutch >= this.clutchTreshold && !clutchDown)
                {
                    InputSimulator.SimulateKeyDown(clutchKey);
                    clutchDown = true;
                }
                // Send the key up event when the threshold is no longer met and the clutchDown flag is true.
                else
                {
                    if (clutchDown)
                    {
                        InputSimulator.SimulateKeyUp(clutchKey);
                    }
                }
            }
            // Log the exception to the console for now.
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                UpAllKeys();
            }
        }

        /*
         * This method will send the key up event on all keys. It should be used when the program is closed to make sure
         * keys don't get stuck down.
         */
        private void UpAllKeys()
        {
            InputSimulator.SimulateKeyUp(gasKey);
            InputSimulator.SimulateKeyUp(brakeKey);
            InputSimulator.SimulateKeyUp(clutchKey);
        }
    }
}
