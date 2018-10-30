from picar import front_wheels, back_wheels

bw = back_wheels.Back_Wheels()


def move_forward(speed):
    bw.speed = speed
    bw.backward()

def move_backward(speed):
    bw.speed = speed
    bw.forward()

def stop():
    bw.stop()

def getDefaultAngle(hostname):
    if hostname == 'picarA':
        return 95
    elif hostname == 'picarB':
        return 90

"""
Wheel angle above 90 is to the right, below 90 is to the left
"""