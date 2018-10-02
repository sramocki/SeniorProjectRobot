#!/bin/bash

# setup.bash 
# Run one time on Raspberry Pi to ensure all resources/dependencies are prepared.

# Update Raspberry Pi
sudo apt-get update
sudo apt-get dist-upgrade

# Switch to root
homedir = $HOME
echo Set password for root:
sudo passwd root
su root
cd $homedir

# Install Dependencies

# VirtualEnv
pip install virtualenv
pip install virtualenvwrapper
virtualenv venv && source venv/bin/activate

# SMBus/I2C
apt-get install python-smbus i2c-tools

# OpenCV
apt-get install python-opencv

# LibJPEG8
apt-get install libjpeg8-dev

# gRPC
virtualenv --python -v grpc-net
cd grpc-net
source ./bin/activate
python -m pip install grpcio
python -m pip install grpio-tools

# Link Git
cd $homedir 
apt-get install git
git clone --recursive https://github.com/sramocki/SeniorProjectRobot.git
cd SeniorProjectRobot
git pull

# Reboot system to finish all installations
su pi
sudo reboot