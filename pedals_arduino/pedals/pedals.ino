int gasPin = 0;                             // This is the pin for the gas (right) pedal
int brakePin = 1;                           // This is the pin for the brake (middle) pedal
int clutchPin = 2;                          // This is the pin for the clutch (left) pedal
String valGas, valBrake, valClutch;         // These are strings for the pedals to be output

void setup() {
  // Setup the serial port
  Serial.begin(115200);
}

void loop() {
  // Read the pedal values
  valGas = String(analogRead(gasPin), DEC);
  valBrake = String(analogRead(brakePin), DEC);
  valClutch = String(analogRead(clutchPin), DEC);

  // Output the gas, brake, and clutch values delimited by a semicolon
  Serial.println(valGas + ";" + valBrake + ";" + valClutch);

  delay(25);
}
