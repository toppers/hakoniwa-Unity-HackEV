#!/bin/bash

if [ -d  simulator/unity/projet ]
then
	:
else
	mkdir -p simulator/unity/project
fi

CURDIR=`pwd`
cd simulator/unity/project
wget https://github.com/toppers/hakoniwa-Unity-HackEV/releases/download/v2.1-practice/single-robot-for-docker.tar.gz
tar xzvf single-robot-for-docker.tar.gz
cd $CURDIR

cd simulator

if [ -d athrill ]
then
	echo "INFO: athrill already cloned."
else
	git clone https://github.com/toppers/athrill.git
        echo "### Success ev3rt"
fi

if [ -d ev3rt-athrill-v850e2m ]
then
	echo "INFO: ev3rt already cloned."
else
	echo "### Install ev3rt"
	git clone https://github.com/toppers/ev3rt-athrill-v850e2m.git
        echo "### Success ev3rt"
fi


if [ -d hakoniwa-scenario-samples ]
then
	echo "INFO: ev3rt already cloned."
else
	echo "### Install ev3rt"
	git clone https://github.com/toppers/hakoniwa-scenario-samples.git
        echo "### Success ev3rt"
fi

