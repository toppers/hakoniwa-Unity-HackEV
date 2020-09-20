#!/bin/bash

if [ -z ${ROOT_DIR} ]
then
	ATHRILL_GCC_PATH=`pwd`/tools
	OS_TYPE=`uname`
else
	ATHRILL_GCC_PATH=${ROOT_DIR}/tools
fi

which v850-elf-gcc > /dev/null
if [ $? -eq 0 ]
then
	echo "INFO: v850-elf-gcc already installed:"
	which v850-elf-gcc
	exit 0
fi

cd ${ATHRILL_GCC_PATH}
if [ $OS_TYPE = "Linux" ]
then
	if [ -f ../downloads/athrill-gcc-package.tar.gz ]
	then
		if [ -d ${ATHRILL_GCC_PATH}/athrill-gcc-package ]
		then
			:
		else
			tar xzvf ../downloads/athrill-gcc-package.tar.gz
			cd  ${ATHRILL_GCC_PATH}/athrill-gcc-package
			tar xzvf athrill-gcc.tar.gz
		fi
	else
		echo "ERROR: not found ../downloads/athrill-gcc-package.tar.gz"
		echo "Please do following command for download package."
		echo "bash installer/download.bash"
		exit 1
	fi
else
	if [ -f ../downloads/athrill-gcc-package-mac.tar.gz ]
	then
		if [ -d ${ATHRILL_GCC_PATH}/athrill-gcc-package ]
		then
			:
		else
			tar xzvf ../downloads/athrill-gcc-package-mac.tar.gz
		fi
	else
		echo "ERROR: not found ../downloads/athrill-gcc-package-mac.tar.gz"
		echo "Please do following command for download package."
		echo "bash installer/download.bash"
		exit 1
	fi
fi
