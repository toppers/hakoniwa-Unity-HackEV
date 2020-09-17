#!/bin/bash

source env/env.bash

cd downloads

which arm-none-eabi-gcc > /dev/null
IS_FOUND=$?

if [ $IS_FOUND -eq 1 ]
then
	echo "ERROR: Please install arm-none-eabi-gcc"
else
	echo "arm-none-eabi-gcc already installed:"
	which arm-none-eabi-gcc
fi

UNITY_PACKAGE_NUM=`find . -name "*.unitypackage" | wc -l`
if [ $UNITY_PACKAGE_NUM -eq 0 ]
then
	wget ${UNITY_PACKAGE}
else
	echo "Unity Package already downloaded:"
	cd ..
	find ./downloads -name "*.unitypackage"
fi
