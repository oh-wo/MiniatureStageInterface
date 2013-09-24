//This sketch assumes a jumper wire between pin 0 and 1 on the Papilio

HardwareSerial mySerial1(8);
HardwareSerial mySerial2(9);
String readString1="";
String readString2="";
void setup() {
  pinMode(3,OUTPUT);
  digitalWrite(3,HIGH);

  //Connect the tx pin of mySerial1 to pin 0 of the Papilio
  pinMode(0, OUTPUT);
  pinModePPS(0, HIGH);
  outputPinForFunction(0,6);

  //Connect the rx pin of mySerial2 to pin 1 of the Papilio
  pinMode(1,INPUT);
  inputPinForFunction(1,2);


  Serial.begin(9600);
  mySerial1.begin(9600);
  mySerial2.begin(9600);

  mySerial1.print("err?\n");
  Serial.println("running");

}
boolean read1Complete = false;
boolean read2Complete = false;
void loop() {
  //Send a 1 out of mySerial1, read it on mySerial2, and send it out the USB serial port.
  while(mySerial2.available()){
    delay(100);
    if (mySerial2.available()) {
      readString2+=(char)mySerial2.read();

      read2Complete = true;
    }
    else{

    }
  }
  if(read2Complete){
    Serial.print(readString2);
    read2Complete=false; 
    readString2="";
  }
  delay(100);
  while(Serial.available()){
    delay(100);
    if(Serial.available()){
      readString1+=(char)Serial.read();
      read1Complete=true;
    }
    else{

    }
  }
  if(read1Complete){
    mySerial1.print(readString1+"\n");
    read1Complete=false; 
    readString1="";
  }
}




