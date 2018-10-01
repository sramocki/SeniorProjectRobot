#!/bin/bash

# setup.bash 
# Run one time on Raspberry Pi to ensure all resources/dependencies are prepared.

# Update Raspberry Pi
sudo apt-get update -y
sudo apt-get dist-upgrade -y

# Install Python Dependencies

# VirtualEnv - for reading Python code
sudo pip install virtualenv -y
sudo pip install virtualenvwrapper -y
virtualenv venv && source venv/bin/activate

# Django - for web server
sudo pip install -e django -y

# SMBus/I2C - for server connection
sudo apt-get install python-smbus i2c-tools -y

# OpenCV - for camera tracking
sudo apt-get install python-opencv -y

# LibJPEG8 - for handling JPEG files
sudo apt-get install libjpeg8-dev -y

# Link Git / 
cd $HOME
sudo apt-get install git -y
git clone --recursive https://github.com/sramocki/SeniorProjectRobot.git
cd SeniorProjectRobot
git checkout scottdudley
git pull
django-admin startproject project

# Reboot system to finish all installations
sudo reboot
