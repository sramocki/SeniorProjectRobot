import cv2
import argparse

arDict = cv2.aruco.getPredefinedDictionary(cv2.aruco.DICT_6X6_50)
parameters = cv2.aruco.DetectorParameters_create()

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("-f", "--file", help="name of the file input")
    args = vars(ap.parse_args())

    fileName = args["file"]

    img = cv2.imread(fileName)
    corners = getCorners(img)
    if corners is not None:
        tLeft = corners[0][0][0]
        tRight = corners[0][0][1]
        bRight = corners[0][0][2]
        bLeft = corners[0][0][3]
        print "Top Left: {x}, {y}".format(x=tLeft[0], y=tLeft[1])
        print "Top Right: {x}, {y}".format(x=tRight[0], y=tRight[1])
        print "Bottom Right: {x}, {y}".format(x=bRight[0], y=bRight[1])
        print "Bottom Left: {x}, {y}".format(x=bLeft[0], y=bLeft[1])


def getCorners(img):
    grayImg = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    #gather the parameters of the markers ID is important to us
    corners, ids, reject = cv2.aruco.detectMarkers(grayImg, arDict, parameters)
    if ids is not None:
        return corners