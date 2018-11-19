import numpy as np
import cv2
import argparse

arDict = cv2.aruco.getPredefinedDictionary(cv2.aruco.DICT_6X6_50)
parameters = cv2.aruco.DetectorParameters_create()

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("-f", "--file", help="name of the file saved")
    args = vars(ap.parse_args())

    fileName = args["file"]
    filePath = fileName

    camera = cv2.VideoCapture(0)
    
    #loop until 'y' button is pressed and save image with file name
    while(True):
        ret, frame = camera.read()
        
        corners = getCorners(frame)
        if corners is not None:
            tLeft = corners[0][0][0]
            bRight = corners[0][0][2]
            cv2.rectangle(img, (tLeft[0],tLeft[1]),(bRight[0], bRight[1]), (0,255,0), 3)

        cv2.imshow('img', frame)
        if cv2.waitKey(1) & 0xFF == ord('y'):
            cv2.imwrite(filePath, frame)
            cv2.destroyAllWindows()
            break

    camera.release()


def getCorners(img):
    grayImg = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    #gather the parameters of the markers ID is important to us
    corners, ids, reject = cv2.aruco.detectMarkers(grayImg, arDict, parameters)
    if ids is not None:
        return corners

if __name__ == "__main__":
    main()