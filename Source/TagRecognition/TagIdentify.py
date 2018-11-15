import numpy as np
import cv2


#getting the dictionary of the markers
#our ids are 1,2,4,8
arDict = cv2.aruco.getPredefinedDictionary(cv2.aruco.DICT_6X6_50)
parameters = cv2.aruco.DetectorParameters_create()

#where we define car camera;  this is for testing and needs to be updated for true implementation
camera = cv2.VideoCapture(0)



        
normEdge = 20



while(True):
    
    ret, frame = camera.read()
    #setting up our frame
    theGray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    cv2.imshow('frame', theGray)
    #gather the parameters of the markers  get size of id    
    corners, ids, reject = cv2.aruco.detectMarkers(theGray, arDict, parameters=parameters)
    
    
    
    #Collsion detection
    if ids is not None:
     #CORNER LAYOUT:corner[0][corner][x][y]
      tLeft = corners[0][0][0]
      tRight = corners[0][0][1]
      print(tLeft,tRight)
      edge = tLeft - tRight
      print(edge)
      if(edge[0] < normEdge):
         # return (0, 0)
         print("stop")
                
    
    
   
   

    # Display the frame for our testing
    cv2.imshow('frame',theGray)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break


camera.release()
cv2.destroyAllWindows()

#[array([[[550.,  76.],[618.,  70.],[622., 144.],[548., 151.]]], dtype=float32)]
