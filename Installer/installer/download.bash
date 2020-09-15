#!/bin/bash

source env/env.bash

cd downloads

which v850-elf-gcc > /dev/null
IS_FOUND=$?

if [ $IS_FOUND -eq 1 ]
then
	if [ $OS_TYPE = "Linux" ]
	then
		wget ${ATHRILL_GCC_LINUX}
	else
		wget ${ATHRILL_GCC_MAC}
	fi
else
	echo "v850-elf-gcc already installed:"
	which v850-elf-gcc
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
