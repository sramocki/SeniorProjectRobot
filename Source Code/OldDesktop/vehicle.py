class Vehicle(object):
  def __init__(self, ipAddress, macAddress,role,batteryLife):
    self.ipAddress = ipAddress
    self.macAddress = macAddress
	self.role = role
	self.batteryLife = batteryLife


p1 = Person("John", 36)
p1.myfunc()