from picar import front_wheels, back_wheels
import picar


picar.setup()
rear_wheels_enabled = True
front_wheels_enabled = True

FW_ANGLE_MAX = 90+30
FW_ANGLE_MIN = 90-30

bw = back_wheels.Back_Wheels()
fw = front_wheels.Front_Wheels()

fw.offset(0)
bw.speed = 0


def main():
    print "Begin drive!"
    
    





if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt():
        destroy()