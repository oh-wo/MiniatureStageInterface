//This sketch assumes a jumper wire between pin 0 and 1 on the Papilio


int commandsLength = 10;
String Commands[10];
int argsLength=10;


void setup() {
  pinMode(3,OUTPUT);
  digitalWrite(3,HIGH);

  pinMode(49, OUTPUT);//LED pin - later to be converted to the laser bin
  digitalWrite(49,LOW);



  Serial.begin(9600);
  Serial1.begin(9600);

  Serial1.print("err?\n");
  Serial.println("running");

  ClearCommands();

}

void loop() {
  //Send a 1 out of Serial1, read it on Serial1, and send it out the USB serial port.
  readFromPc();
  delay(10);
  readFromControllers();
}





void readFromControllers(){
  boolean readComplete = false;
  String readString="";

  while(Serial1.available()){
    //delay(100);
    if (Serial1.available()) {
      readString+=(char)Serial1.read();
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
        SetMode(Arguments[0][0]-'0',Arguments[0][0]-'0');//basically the -'0' performs a sort of atoi function
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
        Move(StrToFloat(Arguments[0]),StrToFloat(Arguments[1]));
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
  Serial1.print("1 svo? \n"); 
}

void SetMode(int mode){

  RelaxPiezoWalkPiezos();//to be courteous

    //check that entered value makes sense before sending to controller
  if(mode==0 || mode==1){
    Serial1.print("1 svo 1 ");
    Serial1.print(mode);
    Serial1.print(" \n");
  } 
  if(mode==1){
    Frf();//need to reference axis
  }
}

void QueryError(){
  Serial1.print("1 err? \n"); 
  Serial1.print("2 err? \n"); 
}

void SetOpenLoopVelocity(float vel){
  //input range is : 0<=vel<=2000 (2kHz)
  if(vel>0){
    Serial1.print("1 ovl 1 ");
    Serial1.print(vel);
    Serial1.print(" \n"); 
    Serial1.print("2 ovl 1 ");
    Serial1.print(vel);
    Serial1.print(" \n"); 
  }
}

void QueryOpenLoopVelocity(){
  Serial1.print("1 ovl? \n");
}

void SetClosedLoopVelocity(float vel){
  if(vel>0){
    Serial1.print("1 vel 1 ");
    Serial1.print(vel);
    Serial1.print(" \n");
  } 
  if(vel>0){
    Serial1.print("2 vel 1 ");
    Serial1.print(vel);
    Serial1.print(" \n");
  } 
}

void QueryClosedLoopVelocity(){
  Serial1.print("1 vel? \n"); 
}
void Move(float posX,float posY){
  //Moves the stage to a set position 
  Serial1.print("1 mov 1 ");
  Serial1.print(posX);
  Serial1.print(" \n"); 
  Serial1.print("2 mov 1 ");
  Serial1.print(posY);
  Serial1.print(" \n"); 
}


void Frf(){
  Serial1.print("1 frf \n"); 
  Serial1.print("2 frf \n"); 
}



void SetOpenLoopAnalogDriving(float voltage){
  //allowable range:  -50<=voltage<=+50; where sign indicates direction
  Serial1.print("1 oad 1 ");
  Serial1.print(voltage);
  Serial1.print(" \n"); 
}

void QueryOpenLoopAnalogDriving(){
  Serial1.print("1 oad? \n"); 
}


void RelaxPiezoWalkPiezos(){
  Serial1.print("1 rnp 1 0\n");
}

void SetOpenLoopStepMoving(int steps){
  Serial1.print("1 osm 1 ");
  Serial1.print(steps);
  Serial1.print("\n");
}

void SetOpenLoopStepAmplitude(float voltage){
  //allowable range:  0 <= voltage <= 55
  if(voltage>0){
    Serial1.print("1 ssa 1 ");
    Serial1.print(voltage);
    Serial1.print("\n");
  } 
}

void QueryOpenLoopStepAmplitude(){
  Serial1.print("1 ssa? \n"); 
}

void PulseLaser(){
  digitalWrite(49,HIGH);
  delay(10);
  digitalWrite(49,LOW);
}

void QueryVolatileMemoryParameters(){
  Serial1.print("1 spa? \n");
}







