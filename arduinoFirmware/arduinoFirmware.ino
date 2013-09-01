#include <SoftwareSerial.h>
#include <iostream.h>
#include <string.h>

SoftwareSerial mySerial(10, 11); // RX, TX
String inputCommands[20] = {
};


void setup()  
{
  // Open serial communications and wait for port to open:
  Serial.begin(9600);
  Serial.println("serial started");
  // set the data rate for the SoftwareSerial port
  mySerial.begin(9600);
  ConfigureOpenLoopMode();
}




void loop() // run over and over
{

  ReadMessagesFromPc();
  SendMessagesFromControllerToPc();
}

void SendMessagesFromControllerToPc(){
  boolean talkMS = 0;
  String readString = "";


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


void ReadMessagesFromPc(){
  String readString="";
  boolean readingIn = 0;
  int cIndex=0;
  boolean talkS = 0;

  while (Serial.available()) {
    delay(100);  //delay to allow buffer to fill 

    if (Serial.available() >0) {
      char c = Serial.read();  //gets one byte from serial buffer
      if(c=='*'){
        ExecuteInputCommands();
      }
      if(c=='['){
        readingIn=1;
      }
      if(readingIn){
        readString +=c; //makes the string readString
      }
      if(c==']'){
        readingIn=0;
        inputCommands[cIndex]=readString;
        readString="";
        cIndex++;
      }

    } 
    talkS=1;

  }


  if(talkS){
    for(int i=0; i<20; i++){
      if(inputCommands[i]!=""){
        Serial.print(inputCommands[i]+"\n");
        readString="";
        talkS=0;
      }
    }

  }
}

void ConfigureOpenLoopMode(){
  mySerial.print("1 err?\n");
  delay(500);
  mySerial.print("2  err?\n");
  delay(500);
  mySerial.print("1 svo 1 1\n");
  delay(500);
  mySerial.print("2 svo 1 1\n");
  delay(500);
  mySerial.print("1 svo?\n");
  delay(500);
  mySerial.print("2 svo?\n");
}

void ExecuteInputCommands(){
  double x;
  for(int i=0; i<20; i++){
    delay(50);
    if(inputCommands[i]!=""){
      switch(inputCommands[i][1]){
      case '1':
        //move x
        x=parseDouble(inputCommands[i],3);
        Serial.print("1 mov 1 ");
        Serial.print(x,5);
        Serial.print("\n");
        mySerial.print("1 mov 1 ");
        mySerial.print(x,5);
        mySerial.print("\n");
        while(hasntMovedToPos(1,x,0.1)){
         delay(100); 
        }
        break;
      case '2':
        //move y
        x=parseDouble(inputCommands[i],3);
        mySerial.print("2 mov 1 ");
        mySerial.print(x,5);
        mySerial.print("\n");
        while(hasntMovedToPos(2,x,0.1)){
         delay(100); 
        }
        break;
      case '3':
        //pos
        mySerial.print("1 pos? \n");
        delay(50);
        mySerial.print("2 pos? \n");
        break;
      case '4':
        //clear inputCommands
        for(int i=0; i<20; i++){
          inputCommands[i]=""; 
          Serial.println(inputCommands[i]);
        }
        Serial.println("------------------cleared");
        break;
      }
    }
  }

}

boolean hasntMovedToPos(int axis, double pos,double tolerance){
  String readString="";
  double currentPos;
  boolean success = false;
  boolean talkMS=0;
  
  //Ask controller for the axis position
  mySerial.print(axis);
  mySerial.print(" pos?\n");
  Serial.print(axis);
  Serial.print(" pos?\n");
  //Wait for a reply
  
  while(readString==""){
    while (mySerial.available()) {
      delay(50);  //delay to allow buffer to fill 
      if (mySerial.available() >0) {
        char c = mySerial.read();  //gets one byte from serial buffer
        readString += c; //makes the string readString
      } 
      talkMS=1;
    }
    if(talkMS){
      for(int i=0; i<readString.length(); i++){
        if(readString[i]=='='){
          currentPos=parseDouble(readString,i+1);
          break;
        }
        talkMS=0;
      }
    }
  }
  if(abs(pos-currentPos)>tolerance){
      success=true;
  }
  return success;
}


double parseDouble(String str,int startIndex){
  int j=0;
  char outputString[20];
  for(int i=startIndex; i<(str.length()-(startIndex-1)); i++){
    if(str[i]!=' ' && str[i]!=']'){
      outputString[j]=str[i];
      j++;
      // Serial.print(str[i]);
    }
    else{
      break; 
    }
  }
  Serial.print("output string=");
  Serial.println(outputString);
  // Serial.print("output string= ");
  //Serial.print(atof(outputString),10);

  //Serial.print("\n");
  Serial.println(outputString);
  return atof(outputString);
}













