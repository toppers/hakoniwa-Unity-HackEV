#!/bin/bash

ATHRILL_GCC_PATH=`pwd`/tools
OS_TYPE=`uname`

which v850-elf-gcc
if [ $? -eq 0 ]
then
	echo "INFO: already installed."
	exit 0
fi

cd ${ATHRILL_GCC_PATH}
if [ $OS_TYPE = "Linux" ]
then
	tar xzvf ../downloads/athrill-gcc-package.tar.gz
	cd  ${ATHRILL_GCC_PATH}/athrill-gcc-package
	tar xzvf athrill-gcc.tar.gz
else
	tar xzvf ../downloads/athrill-gcc-package-mac.tar.gz
fi


echo "export PATH=${ATHRILL_GCC_PATH}/athrill-gcc-package/usr/local/athrill-gcc/bin/:\${PATH}" >> ${HOME}/.bashrc

