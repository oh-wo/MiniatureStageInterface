from sys import path
path.append('C:\Program Files\Enthought\Canopy\App\appdata\canopy-1.0.3.1262.win-x86_64\Lib\site-packages')
import ctypes

import serial

ser = serial.Serial()
ser.setBaudrate(9600)
ser.setPort('COM1')
print('Serial port: '+ser.getPort().__str__())
ser.close()
ser.open()
print(ser._isOpen)
if ser._isOpen:
    ser.write('1 frf')

ser.close()

raw_input()