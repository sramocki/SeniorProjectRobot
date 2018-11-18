from picar import front_wheels
import picar
import cv2
from concurrent import futures
import time
import picarserver
import picarhelper
import socket
import grpc
import picar_pb2
import picar_pb2_grpc


picar.setup()
rear_wheels_enabled = True
front_wheels_enabled = True

FW_ANGLE_MAX = 90+30
FW_ANGLE_MIN = 90-30

fw = front_wheels.Front_Wheels()
fw_default = picarhelper.getDefaultAngle(socket.gethostname())

FW_ANGLE_MAX = fw_default+30
FW_ANGLE_MIN = fw_default-30

mode = 0


#det up our tag dictionary and parameter value 
#we use tag ids 1,2,4,8
arDict = cv2.aruco.getPredefinedDictionary(cv2.aruco.DICT_6X6_50)
parameters = cv2.aruco.DetectorParameters_create()


#get a reference to the camera, default is 0
camera = cv2.VideoCapture(0)
frame = None
inputMode = False

def main():

    #start the server
    server = picarserver.getServer()
    server.start()

    print "Server Started on "+socket.gethostname()+"\n"
    print "Press Ctrl-C to quit"

    move(0.0,0.0)

    # loop unless break occurs
    while True:

        #check if key pressed
        k = cv2.waitKey(1) & 0xFF

        #if q key is pressed we break loop
        if k == ord('q'):
            break

        # get reference to current mode
        mode = picarserver.mode
	   
        #get the current frame
        (grabbed, frame) = camera.read()

        #draw green rectangle where baseline tag should be

        
        
        if mode == 1:
            # leader mode
            #print "picar set to LEADER"
            move(picarserver.throttle, picarserver.direction)

        elif mode == 2:
            # follower mode
            #print "picar set to FOLLOWER"
            throttle, direction = tagID()
            move(throttle, direction)
            picarserver.setFrame(frame)
             
        # else:
            # idle mode
            #print "picar set to IDLE"
        
        #wait 1 second after loop    
        time.sleep(1/60)

    #cleanup    
    destroy()

def move(throttle, direction):
    motor_speed = int(abs(throttle)*100)
    fw_angle = fw_default+(30*(direction))


    if front_wheels_enabled and (fw_angle >= FW_ANGLE_MIN and fw_angle <= FW_ANGLE_MAX):
        fw.turn(fw_angle)
    if rear_wheels_enabled:
        if (throttle > 0.0):
            picarhelper.move_forward(motor_speed)
        elif (throttle < 0.0):
            picarhelper.move_backward(motor_speed)
        else:
            picarhelper.stop()


#method to recognize tags
def tagID():
	
    #setting up our frame
	gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    
	#gather the parameters of the markers  ID is important to us    
    corners, ids, reject = cv2.aruco.detectMarkers(gray, arDict, parameters=parameters)
    

    if ids is not None:
        #CORNER LAYOUT: corner[0][corner][x][y]
        #starts top left and moves clockwise
        tLeft = corners[0][0][0]
        tRight = corners[0][0][1]
        bRight = corners[0][0][2]
        bLeft = corners[0][0][3]

        #draw rectangle around detected tag
        cv2.rectangle(frame, (tLeft[0],tLeft[1]),(bRight[0], bRight[1]), (0,0,255), 1)

    else:
        return(0.0, 0.0)
    



def destroy():
    picarhelper.stop()
    camera.release()
    cv2.destroyAllWindows()

if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt():
        destroy(