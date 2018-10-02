#!/bin/bash

# setup.bash 
# Run one time on Raspberry Pi to ensure all resources/dependencies are prepared.

# Update Raspberry Pi
sudo apt-get update
sudo apt-get dist-upgrade

# Switch to root
homedir = $HOME
echo Default password for root is 'raspberry'
su root
cd $homedir

# Install Dependencies

# VirtualEnv
sudo pip install virtualenv
sudo pip install virtualenvwrapper
virtualenv venv && source venv/bin/activate

# SMBus/I2C
sudo apt-get install python-smbus i2c-tools

# OpenCV
sudo apt-get install python-opencv

# LibJPEG8
sudo apt-get install libjpeg8-dev

# gRPC
virtualenv --python=python3.7 -v grpc-net
cd grpc-net
source ./bin/activate
sudo python -m pip install grpcio
sudo python -m pip install grpio-tools googleapis-common-protos

# Link Git
cd $homedir 
sudo apt-get install git
git clone --recursive https://github.com/sramocki/SeniorProjectRobot.git
cd SeniorProjectRobot
git pull

# Reboot system to finish all installations
sudo reboot
