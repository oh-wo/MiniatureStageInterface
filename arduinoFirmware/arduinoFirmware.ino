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
  //ConfigureOpenLoopMode();
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
  double y;
  int yIndex;
  int spaceCount=0;

  for(int i=0; i<20; i++){
    delay(50);
    if(inputCommands[i]!=""){
      switch(inputCommands[i][1]){
      case '1':
        //move command
        // [commandType  shootLaser  xPos  yPos]


        //find index of y coordinate

          for(int z=0; z<inputCommands[i].length(); z++){
          //loop through and find location of 3rd space character
          if(inputCommands[i][z]==' '){
            spaceCount++; 
          }
          if(spaceCount==3){
            yIndex=z+1;
            spaceCount=0;
            break; 
          }
        }
        Serial.print("Yindex: ");
            Serial.println(yIndex);
        x=parseDouble(inputCommands[i],5);
        y=parseDouble(inputCommands[i],yIndex);
        mySerial.print("1 mov 1 ");
        mySerial.print(x,5);
        mySerial.print("\n");
        mySerial.print("2 mov 1 ");
        mySerial.print(y,5);
        mySerial.print("\n");
        Serial.print("1 mov 1 ");
        Serial.print(x,5);
        Serial.print("\n");
        Serial.print("2 mov 1 ");
        Serial.print(y,5);
        Serial.print("\n");
        while(hasntMovedToPos(x,y,0.001)){
          delay(100); 
        }
        break;

      case '2':
        //pos
        mySerial.print("1 pos? \n");
        delay(50);
        mySerial.print("2 pos? \n");
        break;

      case '3':
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
boolean hasntMovedToPos(double xPos,double yPos, double tol){//target x pos, target y pos, required tolerance
  String readString="";
  double curXPos;
  double curYPos;
  boolean notInPos = false;
  boolean talkMS=0;
  int xIndex;
  int yIndex;
  int equalsCount=0;

  //Ask controller for the axes position
  mySerial.print("1 pos?\n");
  delay(500);
  mySerial.print("2 pos?\n");
  //Wait for a reply

  //delay(1000);
  while(readString.length()<18){//needs to get two lines worth of: "0 1 1=0.00000" and "0 2 1=0.00000"-----------can use the number of equals signs received
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
          if(equalsCount==1){
            yIndex=i+1;
            break;
          }
          if(equalsCount==0){
            xIndex=i+1;
            equalsCount++;
          }
          
        }
        talkMS=0;
      }
      curXPos=parseDouble(readString,xIndex);
      curYPos=parseDouble(readString,yIndex);
      Serial.println("\n-----current positions----");
      Serial.print("*    xPos: ");
      Serial.print(curXPos,5);
      Serial.print("    yPos: ");
       Serial.println(curYPos,5);
       Serial.println("\n");
    }
  }
  if(abs(xPos-curXPos)>tol && abs(yPos-curYPos)>tol){
    notInPos=true;
  }
  return notInPos;
}


double parseDouble(String str,int startIndex){
  int j=0;
  char outputString[20];
  for(int i=startIndex; i<str.length(); i++){
    if(str[i]!=' '&& str[i]!=']'){
      outputString[j]=str[i];
      j++;
    }
    else{
      break; 
    }
  }
  return atof(outputString);
}
















