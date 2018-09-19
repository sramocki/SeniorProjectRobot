# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'registration.ui'
#
# Created by: PyQt5 UI code generator 5.5
#
# WARNING! All changes made in this file will be lost!

from PyQt5 import QtCore, QtGui, QtWidgets

class Ui_RegistrationWindow(object):
    def setupUi(self, RegistrationWindow):
        RegistrationWindow.setObjectName("RegistrationWindow")
        RegistrationWindow.resize(800, 600)
        self.centralwidget = QtWidgets.QWidget(RegistrationWindow)
        self.centralwidget.setObjectName("centralwidget")
        self.deviceTable = QtWidgets.QTableView(self.centralwidget)
        self.deviceTable.setGeometry(QtCore.QRect(20, 120, 391, 311))
        self.deviceTable.setObjectName("deviceTable")
        self.setLeaderButton = QtWidgets.QPushButton(self.centralwidget)
        self.setLeaderButton.setGeometry(QtCore.QRect(520, 200, 75, 23))
        self.setLeaderButton.setObjectName("setLeaderButton")
        self.setFollowerButton = QtWidgets.QPushButton(self.centralwidget)
        self.setFollowerButton.setGeometry(QtCore.QRect(700, 200, 75, 23))
        self.setFollowerButton.setObjectName("setFollowerButton")
        self.scanButton = QtWidgets.QPushButton(self.centralwidget)
        self.scanButton.setGeometry(QtCore.QRect(20, 60, 101, 23))
        self.scanButton.setObjectName("scanButton")
        self.scanButton.clicked.connect(self.deviceScan)
        self.progressScan = QtWidgets.QProgressBar(self.centralwidget)
        self.progressScan.setGeometry(QtCore.QRect(270, 60, 118, 23))
        self.progressScan.setProperty("value", 24)
        self.progressScan.setObjectName("progressScan")
        self.setDefaultButton = QtWidgets.QPushButton(self.centralwidget)
        self.setDefaultButton.setGeometry(QtCore.QRect(610, 200, 75, 23))
        self.setDefaultButton.setObjectName("setDefaultButton")
        self.shutdownButton = QtWidgets.QPushButton(self.centralwidget)
        self.shutdownButton.setGeometry(QtCore.QRect(660, 260, 75, 23))
        self.shutdownButton.setObjectName("shutdownButton")
        self.line = QtWidgets.QFrame(self.centralwidget)
        self.line.setGeometry(QtCore.QRect(433, -80, 51, 671))
        self.line.setFrameShape(QtWidgets.QFrame.VLine)
        self.line.setFrameShadow(QtWidgets.QFrame.Sunken)
        self.line.setObjectName("line")
        self.label = QtWidgets.QLabel(self.centralwidget)
        self.label.setGeometry(QtCore.QRect(170, 100, 81, 16))
        self.label.setObjectName("label")
        self.label_2 = QtWidgets.QLabel(self.centralwidget)
        self.label_2.setGeometry(QtCore.QRect(530, 60, 131, 16))
        self.label_2.setObjectName("label_2")
        self.label_3 = QtWidgets.QLabel(self.centralwidget)
        self.label_3.setGeometry(QtCore.QRect(530, 160, 81, 16))
        self.label_3.setObjectName("label_3")
        self.batteryField = QtWidgets.QLineEdit(self.centralwidget)
        self.batteryField.setGeometry(QtCore.QRect(610, 160, 113, 20))
        self.batteryField.setObjectName("batteryField")
        self.macField = QtWidgets.QLineEdit(self.centralwidget)
        self.macField.setGeometry(QtCore.QRect(660, 60, 113, 20))
        self.macField.setObjectName("macField")
        self.label_4 = QtWidgets.QLabel(self.centralwidget)
        self.label_4.setGeometry(QtCore.QRect(530, 130, 81, 16))
        self.label_4.setObjectName("label_4")
        self.roleField = QtWidgets.QLineEdit(self.centralwidget)
        self.roleField.setGeometry(QtCore.QRect(610, 130, 113, 20))
        self.roleField.setObjectName("roleField")
        self.returnHomeButton = QtWidgets.QPushButton(self.centralwidget)
        self.returnHomeButton.setGeometry(QtCore.QRect(570, 260, 75, 23))
        self.returnHomeButton.setObjectName("returnHomeButton")
        RegistrationWindow.setCentralWidget(self.centralwidget)
        self.menubar = QtWidgets.QMenuBar(RegistrationWindow)
        self.menubar.setGeometry(QtCore.QRect(0, 0, 800, 21))
        self.menubar.setObjectName("menubar")
        self.menuExit = QtWidgets.QMenu(self.menubar)
        self.menuExit.setObjectName("menuExit")
        self.menuHelp = QtWidgets.QMenu(self.menubar)
        self.menuHelp.setObjectName("menuHelp")
        RegistrationWindow.setMenuBar(self.menubar)
        self.statusbar = QtWidgets.QStatusBar(RegistrationWindow)
        self.statusbar.setObjectName("statusbar")
        RegistrationWindow.setStatusBar(self.statusbar)
        self.actionExit = QtWidgets.QAction(RegistrationWindow)
        self.actionExit.setObjectName("actionExit")
        self.actionExit_2 = QtWidgets.QAction(RegistrationWindow)
        self.actionExit_2.setObjectName("actionExit_2")
        self.actionAbout = QtWidgets.QAction(RegistrationWindow)
        self.actionAbout.setObjectName("actionAbout")
        self.menuExit.addAction(self.actionExit_2)
        self.menuHelp.addAction(self.actionAbout)
        self.menubar.addAction(self.menuExit.menuAction())
        self.menubar.addAction(self.menuHelp.menuAction())

        self.retranslateUi(RegistrationWindow)
        QtCore.QMetaObject.connectSlotsByName(RegistrationWindow)
        
    def deviceScan(self):
        v = Vehicle("127.0.0.1","60-5B-9A-5B-92-05","Default","1%")
        self.deviceTable.rowCountChanged(1,2)
        #self.deviceTable.setCellWidget(0,1,"test")
        self.macField.setText(v.macAddress)
        self.roleField.setText(v.role)
        self.batteryField.setText(v.batteryLife)
    

    def retranslateUi(self, RegistrationWindow):
        _translate = QtCore.QCoreApplication.translate
        RegistrationWindow.setWindowTitle(_translate("RegistrationWindow", "MainWindow"))
        self.setLeaderButton.setText(_translate("RegistrationWindow", "Set Leader"))
        self.setFollowerButton.setText(_translate("RegistrationWindow", "Set Follower"))
        self.scanButton.setText(_translate("RegistrationWindow", "Scan for devices"))
        self.setDefaultButton.setText(_translate("RegistrationWindow", "Set Default"))
        self.shutdownButton.setText(_translate("RegistrationWindow", "Shutdown"))
        self.label.setText(_translate("RegistrationWindow", "Device List (IP)"))
        self.label_2.setText(_translate("RegistrationWindow", "Device Information (MAC)"))
        self.label_3.setText(_translate("RegistrationWindow", "Battery Level:"))
        self.label_4.setText(_translate("RegistrationWindow", "Current Role: "))
        self.returnHomeButton.setText(_translate("RegistrationWindow", "Return Home"))
        self.menuExit.setTitle(_translate("RegistrationWindow", "File"))
        self.menuHelp.setTitle(_translate("RegistrationWindow", "Help"))
        self.actionExit.setText(_translate("RegistrationWindow", "Exit"))
        self.actionExit_2.setText(_translate("RegistrationWindow", "Exit"))
        self.actionAbout.setText(_translate("RegistrationWindow", "About"))


if __name__ == "__main__":
    import sys
    app = QtWidgets.QApplication(sys.argv)
    RegistrationWindow = QtWidgets.QMainWindow()
    ui = Ui_RegistrationWindow()
    ui.setupUi(RegistrationWindow)
    RegistrationWindow.show()
    sys.exit(app.exec_())
    
class Vehicle(object):
  def __init__(self, ipAddress, macAddress,role,batteryLife):
    self.ipAddress = ipAddress
    self.macAddress = macAddress
    self.role = role
    self.batteryLife = batteryLife


