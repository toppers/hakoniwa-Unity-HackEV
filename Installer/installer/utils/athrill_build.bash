#!/bin/bash

OS_TYPE=`uname`
ORG_DIR=`pwd`
echo "### building athrill target(v850)"
if [ $OS_TYPE = "Linux" ]
then
	cd athrill-target-v850e2m/build_linux/
else
	cd athrill-target-v850e2m/build_mac/
fi
make clean
make timer32=true

cd $ORG_DIR
if [ -f athrill/bin/linux/athrill2 ]
then
	echo "### Success build athrill target(v850) binary"
else
	echo "### Failed build athrill target(v850) binary"
fi
