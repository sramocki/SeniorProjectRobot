import cv2
import numpy as np
import argparse

# initialize the current frame of the video, along with the list of
# ROI points along with whether or not this is input mode
frame = None
roiPts = []
inputMode = False

def selectROI (event, x, y, flags, param):
    # get references to current frame, roiPts, and if in selection mode
    global frame, roiPts, inputMode

    # update the list of ROI points with the (x,y) location of the click
    if inputMode and event == cv2.EVENT_LBUTTONDOWN and len(roiPts) < 4:
        roiPts.append((x, y))
        cv2.circle(frame, (x, y), 4, (0, 255, 0), 2)
        cv2.imshow("frame", frame)

def main():
    # parse the command line arguments in order to obtain the video path
    ap = argparse.ArgumentParser()
    ap.add_argument("-v", "--video", help = "path to the (optional) video file")
    args = vars(ap.parse_args())

if __name__ == "__main__":
    main()