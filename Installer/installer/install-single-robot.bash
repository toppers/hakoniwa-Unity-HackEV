#!/bin/bash

source env/env.bash

cd simulator

TOOL_DIR=../installer/utils

which athrill2 > /dev/null
if [ $? -ne 0 ]
then
	bash ${TOOL_DIR}/athrill_install.bash
	bash ${TOOL_DIR}/athrill_build.bash
	grep "athrill/bin/linux/" ${HOME}/.bashrc > /dev/null
	if [ $? -ne 0 ]
	then
		echo "export PATH=`pwd`/athrill/bin/linux/:\${PATH}" >> ${HOME}/.bashrc
		echo "Updated: ${HOME}/.bashrc"
		echo "Please do following command:"
		echo "source ${HOME}/.bashrc"
	else
		echo "INFO: .bashrc already has athrill path:"
		grep "athrill/bin/linux/" ${HOME}/.bashrc
	fi

else
	echo "INFO: athrill2 already installed:"
	which athrill2
fi

bash ${TOOL_DIR}/ev3rt_install.bash
bash ${TOOL_DIR}/sample_install.bash
bash ${INS_DIR}/install-v850-gcc.bash
