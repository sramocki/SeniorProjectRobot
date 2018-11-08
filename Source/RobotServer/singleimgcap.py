import numpy as np
import cv2
import argparse

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("-f", "--file", help="name of the file saved")
    args = vars(ap.parse_args())

    fileName = args["file"]
    filePath = './'+fileName

    camera = cv2.VideoCapture(0)
    
    #loop until 'y' button is pressed and save image with file name
    while(True):
        ret, frame = camera.read()
        cv2.imshow('img', frame)
        if cv2.waitKey(1) & 0xFF == ord('y'):
            cv2.imwrite(filePath, frame)
            cv2.destroyAllWindows()
            break

    camera.release()


if __name__ == "__main__":
    main()