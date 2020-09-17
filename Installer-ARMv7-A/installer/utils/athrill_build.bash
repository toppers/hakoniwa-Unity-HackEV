#!/bin/bash

OS_TYPE=`uname`
ORG_DIR=`pwd`
echo "### building athrill target(ARM)"
if [ $OS_TYPE = "Linux" ]
then
	cd athrill-target-ARMv7-A/build_linux/
else
	cd athrill-target-ARMv7-A/build_mac/
fi

if [ -f athrill2 ]
then
	:
else
	make clean
	make timer32=true
fi

cd $ORG_DIR
if [ -f athrill/bin/linux/athrill2 ]
then
	echo "### Success build athrill target(ARM) binary"
else
	echo "### Failed build athrill target(ARM) binary"
fi
