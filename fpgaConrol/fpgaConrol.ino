//This sketch assumes a jumper wire between pin 0 and 1 on the Papilio

HardwareSerial mySerial1(8);
HardwareSerial mySerial2(9);

int commandsLength = 10;
String Commands[10];
int argsLength=10;


void setup() {
  pinMode(3,OUTPUT);
  digitalWrite(3,HIGH);

  pinMode(49, OUTPUT);//LED pin - later to be converted to the laser bin
  digitalWrite(49,LOW);

  //Connect the tx pin of mySerial1 to pin 0 of the Papilio
  pinMode(WC14, OUTPUT);
  pinModePPS(WC14, HIGH);
  outputPinForFunction(WC14,6);

  //Connect the rx pin of mySerial2 to pin 1 of the Papilio
  pinMode(WC15,INPUT);
  inputPinForFunction(WC15,2);


  Serial.begin(9600);
  mySerial1.begin(9600);//print
  mySerial2.begin(9600);//read

  mySerial1.print("err?\n");
  Serial.println("running");

  ClearCommands();

}

void loop() {
  //Send a 1 out of mySerial1, read it on mySerial2, and send it out the USB serial port.
  readFromPc();
  delay(10);
  readFromControllers();
}





void readFromControllers(){
  boolean readComplete = false;
  String readString="";

  while(mySerial2.available()){
    //delay(100);
    if (mySerial2.available()) {
      readString+=(char)mySerial2.read();
      readComplete = true;
    }
    else{

    }
  }
  if(readComplete){
    Serial.print(readString);
    readComplete=false; 
    readString="";
  }
}

void readFromPc(){
  boolean readComplete = false;
  String readString="";


  while(Serial.available()){
    if(Serial.available()){
      readString+=(char)Serial.read();
      readComplete=true;
    }
    else{
    }
  }
  if(readComplete){
    parseInputCommand(readString);
    readComplete=false; 
    readString="";
  }

}


void parseInputCommand(String readString){
  //expects commands in the form [1 1 5.300 ][2 3 4][5]* etc
  int startIndex = 0;

  int terminatorCharacterIndex = readString.indexOf(']');
  if(terminatorCharacterIndex!=-1){
    //then there are meaninful commands input
    int i=0;
    while(terminatorCharacterIndex!=-1){
      Commands[i] = readString.substring(startIndex, terminatorCharacterIndex+1);
      startIndex=terminatorCharacterIndex+1;
      terminatorCharacterIndex = readString.indexOf(']',startIndex);
      i++;
    } 
  }
  if(readString.indexOf("*")!=-1){
    //then readString contains the exectute command
    ExecuteCommands(); 
  }
}


void ParseArguments(String Command, String* Args){
  //char spaceIndex = Command.indexOf(' ');
  int j=0; 
  //String args[10];
  String thisArg = "";
  boolean spaceSequenceBegun = false;
  for(int i=2; i<Command.length(); i++){//starts at 2 because 0=[, 1=command character
    if(Command[i]!=' ' && Command[i]!='['&& Command[i]!=']'){
      spaceSequenceBegun=false;
      thisArg+=Command[i];
    }
    else{
      if(!spaceSequenceBegun){
        spaceSequenceBegun=true;
        if(thisArg!=""){
          Args[j]=thisArg;
          thisArg="";
          j++;
        };

      }
    } 
  }
  /*
  Serial.println(); Serial.println("Arguments: ");
   for(int i=0; i<10; i++){
   Serial.println(Args[i]);
   }*/
}

void ExecuteCommands(){
  String Arguments[10];
  for(int i=0; i<10; i++){
    if(Commands[i]!=""){
      ParseArguments(Commands[i], Arguments);
      //this is a legit command
      switch(Commands[i][1]){
      case 'a':
        //set mode (0 = open, 1 = closed)
        SetMode(Arguments[0][0]-'0');//basically the -'0' performs a sort of atoi function
        break;
      case 'b':
        QueryMode();
        break;
      case 'c':
        //Clear the commands array
        ClearCommands();
        break;
      case 'd': 
        //query error
        QueryError();
        break;
      case 'e':
        //set open loop velocity
        SetOpenLoopVelocity(StrToFloat(Arguments[0]));
        break;
      case 'f':
        QueryOpenLoopVelocity();
        break;
      case 'g':
        Move(StrToFloat(Arguments[0]));
        break;
      case 'h':
        Frf();
        break;
      case 'i':
        SetClosedLoopVelocity(StrToFloat(Arguments[0]));
        break;
      case 'j':
        QueryClosedLoopVelocity();
        break;
      case 'k':
        SetOpenLoopAnalogDriving(StrToFloat(Arguments[0]));
        break;
      case 'l':
        QueryOpenLoopAnalogDriving();
        break;
      case 'm':
        RelaxPiezoWalkPiezos();
        break;
      case 'n':
        SetOpenLoopStepMoving(StrToInt(Arguments[0]));
        break;
      case 'o':
        SetOpenLoopStepAmplitude(StrToFloat(Arguments[0]));
        break;
      case 'p':
        QueryOpenLoopStepAmplitude();
        break;
      case 'q':
        PulseLaser();
        break;
      case 'r':
        QueryVolatileMemoryParameters();
        break;
      }


    } 
    ClearArguments(Arguments);
  }
}
float StrToFloat(String str){
  char carray[str.length() + 1]; //determine size of the array
  str.toCharArray(carray, sizeof(carray)); //put str into an array
  return atof(carray);
}
int StrToInt(String str){
  char carray[str.length() + 1]; //determine size of the array
  str.toCharArray(carray, sizeof(carray)); //put str into an array

    return atoi(carray);
}

void ClearCommands(){
  for(int i=0; i<commandsLength; i++){
    Commands[i]="";
  } 
}

void ClearArguments(String* Args){
  for(int i=0; i<argsLength; i++){
    Args[i]="";
  } 
}

void QueryMode(){
  mySerial1.print("1 svo? \n"); 
}

void SetMode(int mode){

  RelaxPiezoWalkPiezos();//to be courteous

    //check that entered value makes sense before sending to controller
  if(mode==0 || mode==1){
    mySerial1.print("1 svo 1 ");
    mySerial1.print(mode);
    mySerial1.print(" \n");
  } 
  if(mode==1){
    Frf();//need to reference axis
  }
}

void QueryError(){
  mySerial1.print("1 err? \n"); 
}

void SetOpenLoopVelocity(float vel){
  //input range is : 0<=vel<=2000 (2kHz)
  if(vel>0){
    mySerial1.print("1 ovl 1 ");
    mySerial1.print(vel);
    mySerial1.print(" \n"); 
  }
}

void QueryOpenLoopVelocity(){
  mySerial1.print("1 ovl? \n");
}

void SetClosedLoopVelocity(float vel){
  if(vel>0){
    mySerial1.print("1 vel 1 ");
    mySerial1.print(vel);
    mySerial1.print(" \n");
  } 
}

void QueryClosedLoopVelocity(){
  mySerial1.print("1 vel? \n"); 
}
void Move(float pos){
  //Moves the stage to a set position 
  mySerial1.print("1 mov 1 ");
  mySerial1.print(pos);
  mySerial1.print(" \n"); 
}


void Frf(){
  mySerial1.print("1 frf \n"); 
}



void SetOpenLoopAnalogDriving(float voltage){
  //allowable range:  -50<=voltage<=+50; where sign indicates direction
  mySerial1.print("1 oad 1 ");
  mySerial1.print(voltage);
  mySerial1.print(" \n"); 
}

void QueryOpenLoopAnalogDriving(){
  mySerial1.print("1 oad? \n"); 
}


void RelaxPiezoWalkPiezos(){
  mySerial1.print("1 rnp 1 0\n");
}

void SetOpenLoopStepMoving(int steps){
  mySerial1.print("1 osm 1 ");
  mySerial1.print(steps);
  mySerial1.print("\n");
}

void SetOpenLoopStepAmplitude(float voltage){
  //allowable range:  0 <= voltage <= 55
  if(voltage>0){
    mySerial1.print("1 ssa 1 ");
    mySerial1.print(voltage);
    mySerial1.print("\n");
  } 
}

void QueryOpenLoopStepAmplitude(){
  mySerial1.print("1 ssa? \n"); 
}

void PulseLaser(){
  digitalWrite(49,HIGH);
  delay(10);
  digitalWrite(49,LOW);
}

void QueryVolatileMemoryParameters(){
  mySerial1.print("1 spa? \n");
}







