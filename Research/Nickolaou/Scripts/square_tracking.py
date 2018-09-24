import cv2
import numpy as np
import argparse

frame = None
roiPts = []
inputMode = False

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("-v", "--video", help = "path to the (optional) video file")
    args = vars(ap.parse_args())


if __name__ == "__main__":
    main()