from picar import front_wheels
import picar
import cv2
from concurrent import futures
import time
import picarserver
import picarhelper
import socket

picar.setup()
rear_wheels_enabled = True
front_wheels_enabled = True

fw = front_wheels.Front_Wheels()
fw_default = picarhelper.getDefaultAngle(socket.gethostname())

FW_ANGLE_MAX = fw_default+30
FW_ANGLE_MIN = fw_default-30

#fw.offset = 0
fw.turn(fw_default)

mode = 'IDLE'

#get a reference to the camera, default is 0
camera = cv2.VideoCapture(0)
frame = None
roiPts = []
inputMode = False

def main():

    #start the server
    picarserver.serve()

    print "Server Started on "+socket.gethostname()+"\n"
    print "Press q to cancel"

    # loop unless break occurs
    while True:

        #check if key pressed
        k = cv.waitKey(1) & 0xFF

        #if q key is pressed we break loop
        if k == ord('q'):
            break

        # get reference to current mode
        mode = picarserver.mode

        if mode == 'LEADER':
            # leader mode
            print "picar set to LEADER"
            move(picarserver.throttle, picarserver.direction)

        elif mode == 'FOLLOWER':
            # follower mode
            print "picar set to FOLLOWER"
             
        else:
            # idle mode
            print "picar set to IDLE"

    #cleanup    
    destroy()

def move(throttle, direction):
    motor_speed = abs(throttle)*100
    fw_angle = fw_default-(30*(direction))

    if front_wheels_enabled and (fw_angle >= FW_ANGLE_MIN and fw_angle <= FW_ANGLE_MAX):
        fw.turn(fw_angle)
    if rear_wheels_enabled:
        if (throttle > 0.0):
            picarhelper.move_forward(motor_speed)
        elif (throttle < 0.0):
            picarhelper.move_backwards(motor_speed)
        else:
            picarhelper.stop()

def destroy():
    picarhelper.stop()
    camera.release()
    cv2.destroyAllWindows()

def test():
    print "Begin Test!"
    move(0.0, 0.5)
    time.sleep(1)
    move(0.0, -0.5)
    time.sleep(1)
    move(0.0,0.0)
    print "End Test!"
    destroy()

if __name__ == '__main__':
    try:
        test()
    except KeyboardInterrupt():
        destroy()