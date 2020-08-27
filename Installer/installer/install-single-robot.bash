#!/bin/bash

cd simulator

TOOL_DIR=../installer/utils

which athrill2
if [ $? -ne 0 ]
then
	bash ${TOOL_DIR}/athrill_install.bash
	bash ${TOOL_DIR}/athrill_build.bash
	echo "export PATH=`pwd`/athrill/bin/linux/:\${PATH}" >> ${HOME}/.bashrc
fi

bash ${TOOL_DIR}/ev3rt_install.bash
bash ${TOOL_DIR}/sample_install.bash

