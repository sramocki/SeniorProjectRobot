from picar import front_wheels, back_wheels
import picar
import cv2 as cv
from concurrent import futures
import time
import argparse
import picarserver


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

frame = None
roiPts = []
inputMode = False

mode = 'IDLE'
throttle = 0
direction = 0

def main():

    server = PiCarServicer()
    picarserver.serve()


    #parse the command line arguments
    ap = argparse.ArgumentParser()
    ap.add_argument("-m", "--mode", help="mode that the car will be in")
    args = vars(ap.parse_args())

    # get a reference to the camera, default is 0
    camera = cv2.VideoCapture(0)

    # loop over frames
    while True:
        
        # get reference to current mode
        global mode = server.mode

        if mode == 'LEADER':
            # leader mode

        elif mode == 'FOLLOWER':
            # follower mode
            #grab the current frame from the camera
            (grabbed, frame) = camera.read()

            # if there is no frame to get from the camera, break
            if not grabbed:
                break

            # display frames
            cv2.imshow("frame", frame)

            #record if the user presses a key
            key = cv2.waitKey(1) & 0xFF

            if key == ord("q"):
                break
        else:
            # idle mode

        


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

    camera.release()
    cv2.destroyAllWindows()


def move(throttle, direction):
    motor_speed = abs(throttle)*100
    bw.speed = motor_speed

    fw_angle = 90-30*(direction)
    if front_wheels_enabled and (fw_angle >= FW_ANGLE_MIN and fw_angle <= FW_ANGLE_MAX):
        fw.turn(fw_angle)
    if rear_wheels_enabled:
        if (throttle > 0.0):
            bw.forward()
        elif (throttle < 0.0):
            bw.backward()
        else:
            bw.stop()

def destroy():
    bw.stop()

if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt():
        destroy()