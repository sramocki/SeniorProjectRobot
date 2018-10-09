from picar import front_wheels, back_wheels
import picar
import cv2 as cv
import time

picar.setup()
rear_wheels_enabled = True
front_wheels_enabled = True

FW_ANGLE_MAX = 90+30
FW_ANGLE_MIN = 90-30

bw = back_wheels.Back_Wheels()
fw = front_wheels.Front_Wheels()

fw.offset = 0
fw.turn(90)
bw.speed = 0

motor_speed = 60
fw_angle = 90 #straight


def main():
    print "Begin drive!"
    move(0.6,0.0)
    time.sleep(3)
    ## wait for input
    #python 2
    #raw_input("Press enter to stop the car")
    #python 3
    #input("Press enter to stop the car")
    move(0.0,0.0)
    print "End Drive"


def move(throttle, direction):
    motor_speed = abs(throttle)*100
    bw.speed = motor_speed

    fw_angle = 90-30*(direction)
    if front_wheels_enabled and (fw_angle >= FW_ANGLE_MIN and fw_angle <= FW_ANGLE_MAX):
        fw.turn(fw_angle)
    if rear_wheels_enabled:
        if (throttle > 0.0):
            bw.foward()
        else if (throttle < 0.0):
            bw.backward()
        else
            bw.stop()

def destroy():
    bw.stop()


if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt():
        destroy()