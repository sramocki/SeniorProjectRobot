from picar import front_wheels, back_wheels
import picar


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



def main():
    print "Begin drive!"
    fw_angle = 0
    
    




if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt():
        destroy()