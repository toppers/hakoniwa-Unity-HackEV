#!/bin/bash

source env/env.bash

cd downloads

which v850-elf-gcc
IS_FOUND=$?

if [ $IS_FOUND -eq 1 ]
then
	if [ $OS_TYPE = "Linux" ]
	then
		wget ${ATHRILL_GCC_LINUX}
	else
		wget ${ATHRILL_GCC_MAC}
	fi
fi

wget ${UNITY_PACKAGE}
