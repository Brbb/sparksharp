// -----------------
// Read temperature
// -----------------

// Create a variable that will store the temperature value
double temperature = 0;
int rawtemperature = 0;

char temp1[64];
char temp2[64];

void setup()
{
    
  Serial.begin(9600);
  Serial.println("Starting...");
    
  // Register a Spark variable here
  Spark.variable("temperature", &temperature, DOUBLE);
  Spark.variable("rawtemp", &rawtemperature, INT);

  // Connect the temperature sensor to A7 and configure it
  // to be an input
  pinMode(A7, INPUT);
}

void loop()
{
  // Keep reading the temperature so when we make an API
  // call to read its value, we have the latest one
  //temperature = analogRead(A7);
  
  Serial.println("Reading...");
  rawtemperature = analogRead(A7);
  
  temperature = (((rawtemperature * 3.3)/4095) - 0.5) * 100;
  
  sprintf(temp1, "%.2f", temperature);

  
  Spark.publish("Temperature",temp1);    
  delay(5000);
}

// The returned value from the Core is going to be in the range from 0 to 4095. 
// You can easily convert this value to actual temperature reading by using the following formula:
// voltage = (sensor reading x 3.3)/4095
// Temperature (in Celsius) = (voltage - 0.5) X 1
