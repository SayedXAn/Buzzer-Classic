int pins[10] = {4,5,16,17,19,21,22,23,25,26};

int activeRelay = -1; 

void setup() {
  for(int i=0; i<10; i++)
  {
    pinMode(pins[i], OUTPUT);
    digitalWrite(pins[i], HIGH);
  }
  
  Serial.begin(115200);
}

void loop() {

  if(Serial.available() > 0)
  {
    char input = Serial.read();
    if(input == 'A' || input == 'a')
    {
      for(int i=0; i<10; i++)
      {
        digitalWrite(pins[i], HIGH);
      }
      activeRelay = -1;
    }

    else if(input >= '1' && input <= '9')
    {
      int relayIndex = input - '1';

      if(activeRelay == -1)
      {
        digitalWrite(pins[relayIndex], LOW);
        activeRelay = relayIndex;
        Serial.print("Relay ");
        Serial.print(relayIndex + 1);
        Serial.println(" ON");
      }
      else
      {
        Serial.println("A relay is already ON. Reset first using A.");
      }
    }

    else if(input == '0')
    {
      if(activeRelay == -1)
      {
        digitalWrite(pins[9], LOW);
        activeRelay = 9;
        Serial.println("Relay 10 ON");
      }
      else
      {
        Serial.println("A relay is already ON. Reset first using A.");
      }
    }
  }
}
/*4
5
16
17
19
21
22
23
*/