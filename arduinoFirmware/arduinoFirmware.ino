#include <SoftwareSerial.h>
#include <iostream.h>
#include <string.h>

SoftwareSerial mySerial(10, 11); // RX, TX

void setup()  
{
  // Open serial communications and wait for port to open:
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }


  Serial.println("serial started");

  // set the data rate for the SoftwareSerial port
  mySerial.begin(9600);
}
String readString = "";
boolean talkMS = 0;
boolean talkS = 0;
void loop() // run over and over
{
  while (Serial.available()) {
    delay(100);  //delay to allow buffer to fill 
    if (Serial.available() >0) {
      char c = Serial.read();  //gets one byte from serial buffer
      readString += c; //makes the string readString
    } 
    talkS=1;
  }
  if(talkS){
  mySerial.print(readString+"\n");
  readString="";
  talkS=0;
  }
  
  
  while (mySerial.available()) {
    delay(100);  //delay to allow buffer to fill 
    if (mySerial.available() >0) {
      char c = mySerial.read();  //gets one byte from serial buffer
      readString += c; //makes the string readString
    } 
    talkMS=1;
  }
  if(talkMS){
  Serial.print(readString+"\n");
  readString="";
  talkMS=0;
  }
    
}
