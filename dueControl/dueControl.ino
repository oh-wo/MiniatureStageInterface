//shutter constants
const int channel_a_enable  = 6;
const int channel_a_input_1 = 4;
const int channel_a_input_2 = 7;
const int channel_b_enable  = 5;
const int channel_b_input_3 = 3;
const int channel_b_input_4 = 2;
bool calLaserOn = false;
void setup() {
  //Shutter
  pinMode( channel_a_enable, OUTPUT );  // Channel A enable
  pinMode( channel_a_input_1, OUTPUT ); // Channel A input 1
  pinMode( channel_a_input_2, OUTPUT ); // Channel A input 2

  pinMode( channel_b_enable, OUTPUT );  // Channel B enable
  pinMode( channel_b_input_3, OUTPUT ); // Channel B input 3
  pinMode( channel_b_input_4, OUTPUT ); // Channel B input 4


  pinMode(51, OUTPUT); //the calibration laser
  digitalWrite(51, LOW); //off unless turned on.
  // Serial
  Serial.begin(9600);
  Serial1.begin(9600);
  Serial1.print("1 err?\n");
  Serial1.print("2 err?\n");
  Serial.println("--setup complete--");
}
//User definable parameters
const int commandsLength = 500;//max number of commands allowed
const int commandStringLength = 30;//length of any one command ie length between []
const int argumentsLength = 4;//max number of arguments allowed, including the command
//Global variables for program
boolean readComplete = false;
boolean reading = false;
int commandIndex = 0;
String commands[commandsLength];
String Args[argumentsLength];
boolean debugging = false;

void loop() {
  // put your main code here, to run repeatedly:
  ReadFromPc();
  delay(10);
  readFromControllers();
}

void readFromControllers() {
  boolean readComplete = false;
  String readString = "";

  while (Serial1.available()) {
    //delay(100);
    if (Serial1.available()) {
      readString += (char)Serial1.read();
      readComplete = true;
    }
    else {

    }
  }
  if (readComplete) {
    Serial.print(readString);
    readComplete = false;
    readString = "";
  }
}

void ReadFromPc() {
  while (Serial.available()) {
    char inputChar = Serial.read();
    switch (inputChar) {
      case'*':
        readComplete = true;
        break;
      case'[':
        reading = true;
        commandIndex++;
        break;
      case']':
        reading = false;
        break;
    }
    if (reading && inputChar != '[') {
      commands[commandIndex] += inputChar;
    }
  }
  if (readComplete) {
    for (int i = 0; i < commandsLength; i++) {
      if (commands[i] != "") {
        if (debugging) {
          Serial.print("command"); Serial.print(i); Serial.print(":");
          Serial.println(commands[i]);
        }
        ClearArguments();
        ParseArguments(commands[i]);
        if (debugging) {
          for (int j = 0; j < argumentsLength; j++) {
            if (Args[j] != "") {
              Serial.print(" arg"); Serial.print(j); Serial.print(":");
              Serial.println(Args[j]);
            }
          }
        }
        //Execute commands
        switch (commands[i][0]) {
          case 'a':
            //set mode (0 = open, 1 = closed)
            SetMode(StrToInt(Args[1]), StrToInt(Args[2])); //basically the -'0' performs a sort of atoi function
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
            SetOpenLoopVelocity(StrToFloat(Args[1]));
            break;
          case 'f':
            QueryOpenLoopVelocity();
            break;
          case 'g':
            Move(StrToFloat(Args[1]), StrToFloat(Args[2]));
            break;
          case 'h':
            Frf();
            break;
          case 'i':
            SetClosedLoopVelocity(StrToFloat(Args[1]));
            break;
          case 'j':
            QueryClosedLoopVelocity();
            break;
          case 'k':
            SetOpenLoopAnalogDriving(StrToFloat(Args[1]));
            break;
          case 'l':
            QueryOpenLoopAnalogDriving();
            break;
          case 'm':
            RelaxPiezoWalkPiezos();
            break;
          case 'n':
            SetOpenLoopStepMoving(StrToInt(Args[1]));
            break;
          case 'o':
            SetOpenLoopStepAmplitude(StrToFloat(Args[1]));
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
          case 's':
            Debugging(StrToInt(Args[1]));
            break;
          case 't':
            QueryPos();
            break;
          case 'u':
            IsInPos(StrToFloat(Args[1]), StrToFloat(Args[2]), StrToFloat(Args[3]));
            break;
          case 'v':
            MoveAndBeSure(StrToFloat(Args[1]), StrToFloat(Args[2]), StrToFloat(Args[3]));
            break;
          case 'w':
            MoveAndShoot(StrToFloat(Args[1]), StrToFloat(Args[2]), StrToFloat(Args[3]));
            break;
          case 'x':
            Serial.print("dielectric control due");
            break;
          case 'y':
            ToggleCalibrationLaser();
            break;
        }
      }
    }
    commandIndex = 0;
    readComplete = false;
    ClearCommands();
  }
}
float StrToFloat(String str) {
  char carray[str.length() + 1]; //determine size of the array
  str.toCharArray(carray, sizeof(carray)); //put str into an array
  return atof(carray);
}
int StrToInt(String str) {
  char carray[str.length() + 1]; //determine size of the array
  str.toCharArray(carray, sizeof(carray)); //put str into an array

  return atoi(carray);
}

void ClearCommands() {
  for (int i = 0; i < commandsLength; i++) {
    commands[i] = "";
  }
}
void QueryMode() {
  Serial1.print("1 svo? \n");
  Serial1.print("2 svo? \n");
}


void ParseArguments(String Command) {
  int i = 0; //argument index
  String thisArgument = "";
  for (int j = 0; j < commandStringLength; j++) {//Command[j]
    if (Command[j] != ' ') {
      //can be a command
      Args[i] += Command[j];
    } else {
      i++;
    }
  }
}

void ClearArguments() {
  for (int i = 0; i < argumentsLength; i++) {
    Args[i] = "";
  }
}


void SetMode(int mode1, int mode2) {

  RelaxPiezoWalkPiezos();//to be courteous

  //check that entered value makes sense before sending to controller
  if (mode1 == 0 || mode1 == 1) {
    Serial1.print("1 svo 1 ");
    Serial1.print(mode1);
    Serial1.print(" \n");
  }
  if (mode2 == 0 || mode2 == 1) {
    Serial1.print("2 svo 1 ");
    Serial1.print(mode2);
    Serial1.print(" \n");
  }
  if (mode1 == 1) {
    Frf();//need to reference axis
  }
  if (mode2 == 1) {
    Frf();
  }
}

void QueryError() {
  Serial1.print("1 err? \n");
  Serial1.print("2 err? \n");
}

void SetOpenLoopVelocity(float vel) {
  //input range is : 0<=vel<=2000 (2kHz)
  if (vel > 0) {
    Serial1.print("1 ovl 1 ");
    Serial1.print(vel);
    Serial1.print(" \n");
    Serial1.print("2 ovl 1 ");
    Serial1.print(vel);
    Serial1.print(" \n");
  }
}

void QueryOpenLoopVelocity() {
  Serial1.print("1 ovl? \n");
}

void SetClosedLoopVelocity(float vel) {
  if (vel > 0) {
    Serial1.print("1 vel 1 ");
    Serial1.print(vel);
    Serial1.print(" \n");
  }
  if (vel > 0) {
    Serial1.print("2 vel 1 ");
    Serial1.print(vel);
    Serial1.print(" \n");
  }
}

void QueryClosedLoopVelocity() {
  Serial1.print("1 vel? \n");
}
void Move(float posX, float posY) {
  //Moves the stage to a set position
  Serial1.print("1 mov 1 ");
  Serial1.print(posX);
  Serial1.print(" \n");
  Serial1.print("2 mov 1 ");
  Serial1.print(posY);
  Serial1.print(" \n");
}


void Frf() {
  Serial1.print("1 frf \n");
  Serial1.print("2 frf \n");
}



void SetOpenLoopAnalogDriving(float voltage) {
  //allowable range:  -50<=voltage<=+50; where sign indicates direction
  Serial1.print("1 oad 1 ");
  Serial1.print(voltage);
  Serial1.print(" \n");
}

void QueryOpenLoopAnalogDriving() {
  Serial1.print("1 oad? \n");
}


void RelaxPiezoWalkPiezos() {
  Serial1.print("1 rnp 1 0\n");
}

void SetOpenLoopStepMoving(int steps) {
  Serial1.print("1 osm 1 ");
  Serial1.print(steps);
  Serial1.print("\n");
}

void SetOpenLoopStepAmplitude(float voltage) {
  //allowable range:  0 <= voltage <= 55
  if (voltage > 0) {
    Serial1.print("1 ssa 1 ");
    Serial1.print(voltage);
    Serial1.print("\n");
  }
}

void QueryOpenLoopStepAmplitude() {
  Serial1.print("1 ssa? \n");
}

void PulseLaser() {
  analogWrite( channel_a_enable, 100);
  digitalWrite( channel_a_input_1, LOW);
  digitalWrite( channel_a_input_2, HIGH);
  delay(10l);
  allInputsOff();
}
void allInputsOff()
{
  //for the shutter
  digitalWrite( 4, LOW );
  digitalWrite( 7, LOW );
  digitalWrite( 6, LOW );
  digitalWrite( 3, LOW );
  digitalWrite( 2, LOW );
  digitalWrite( 5, LOW );

  delay (2000);
}

void QueryVolatileMemoryParameters() {
  Serial1.print("1 spa? \n");
}
void Debugging(int isDebugging) {
  //0=any other integer=false, 1=true
  debugging = isDebugging == 1;
}
void MoveAndBeSure(float posX, float posY, float positionTolerance) {
  int timeOut = 20;
  int timeOutCounter = 0;
  Move(posX, posY);
  while (!IsInPos(posX, posY, positionTolerance)) {
    timeOutCounter++;
    if (timeOutCounter > timeOut) {
      Serial.println("Move and be sure timeout error");
      break;
    }
  }
}
void MoveAndShoot(float posX, float posY, float positionTolerance) {
  MoveAndBeSure(posX, posY, positionTolerance);
  PulseLaser();
}
boolean IsInPos(float posX, float posY, float tolerance) {
  Serial1.flush();
  QueryPos();
  boolean _IsInPos = false;
  String posReadString = "";
  boolean posReadComplete = false;
  //Will take over serial reading until gets a result
  int timeOut = 40000;
  int timeOutCounter = 0;
  float actualPosX = 0;
  float actualPosY = 0;
  bool allDataRead = false;
  while (!posReadComplete) {
    if (Serial1.available()) {
      posReadString += (char)Serial1.read();

      allDataRead = posReadString.length() >= 28;

      int indexAxisOnePlusSix = posReadString.indexOf("0 1 1") + 6;
      int indexAxisTwoPlusSix = posReadString.indexOf("0 2 1") + 6;
      if (allDataRead) {
        posReadComplete = true;
        actualPosX = StrToFloat(posReadString.substring(indexAxisOnePlusSix, indexAxisOnePlusSix + 7));
        actualPosY = StrToFloat(posReadString.substring(indexAxisTwoPlusSix, indexAxisTwoPlusSix + 7));
        if (debugging) {
          Serial.print("actualPosX:"); Serial.println(actualPosX);
          Serial.print("actualPosY:"); Serial.println(actualPosY);
        }
      }
    }
    else {

    }
    timeOutCounter++;
    if (timeOut < timeOutCounter) {
      posReadComplete = true;
      Serial.println("_isInPos timeout error");
    }
  }
  if (debugging) {
    Serial.print("pos read string:["); Serial.print(posReadString); Serial.println("]");
    Serial.print("timout counter:"); Serial.println(timeOutCounter);//10349//21345
  }
  if (posX - tolerance <= actualPosX && actualPosX <= posX + tolerance) {
    if (posY - tolerance <= actualPosY && actualPosY <= posY + tolerance) {
      _IsInPos = true;
    }
  }
  if (debugging) {
    Serial.println(_IsInPos ? "in position" : "not in position");
  };
  return _IsInPos;
}
void QueryPos() {
  Serial1.println("1 pos?\n");
  delay(100);
  Serial1.println("2 pos?\n");
}

void ToggleCalibrationLaser() {
  calLaserOn = !calLaserOn;
  digitalWrite(51, calLaserOn ? HIGH : LOW);
}
