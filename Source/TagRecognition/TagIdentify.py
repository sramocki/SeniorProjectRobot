import numpy as np
import cv2


#getting the dictionary of the markers
#our ids are 0,1,2,3
arDict = cv2.aruco.getPredefinedDictionary(cv2.aruco.DICT_6X6_50)
parameters = cv2.aruco.DetectorParameters_create()

#where we define car camera;  this is for testing and needs to be updated for true implementation
carCam = cv2.VideoCapture(0)







while(True):
    ret, frame = carCam.read()

    theGray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    cv2.imshow('frame', theGray)
    
    
    seenMarkers = cv2.aruco.detectMarkers(theGray, arDict)
    
    
    
    if len(seenMarkers[0]) > 0:
        cv2.aruco.drawDetectedMarkers(theGray,seenMarkers[0],seenMarkers[1])
    
    
    
    #print(seenMarkers)
    
   

    # Display the frame for our testing
    cv2.imshow('frame',theGray)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break


carCam.release()
cv2.destroyAllWindows()

