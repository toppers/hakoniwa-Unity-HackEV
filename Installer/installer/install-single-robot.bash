#!/bin/bash

source env/env.bash

cd simulator

TOOL_DIR=${INS_DIR}/utils

which athrill2 > /dev/null
if [ $? -ne 0 ]
then
	bash ${TOOL_DIR}/athrill_install.bash
	bash ${TOOL_DIR}/athrill_build.bash
	grep "athrill/bin/linux/" ${HOME}/.bashrc > /dev/null
	if [ $? -eq 0 ]
	then
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


ATHRILL_GCC_PATH=${ROOT_DIR}/tools
echo ""

grep "athrill/bin/linux/" ${HOME}/.bashrc > /dev/null
if [ $? -ne 0 ]
	then
    grep "athrill-gcc-package" ${HOME}/.bashrc > /dev/null
    if [ $? -ne 0 ]
    then
      echo "INFO: Please do following 2 commands to setup athrill & athrill-gcc paths"
      echo "echo \"export PATH=\`pwd\`/simulator/athrill/bin/linux/:${ATHRILL_GCC_PATH}/athrill-gcc-package/usr/local/athrill-gcc/bin/:\\\${PATH}\" >> \${HOME}/.bashrc"
      echo "source ${HOME}/.bashrc"

    else
      echo "INFO: .bashrc already has athrill-gcc path:"
      grep "athrill-gcc-package" ${HOME}/.bashrc
      echo ""
      echo "INFO: Please do following 2 commands to setup athrill path"
      echo "echo \"export PATH=\`pwd\`/simulator/athrill/bin/linux/:\\\${PATH}\" >> \${HOME}/.bashrc"
      echo "source ${HOME}/.bashrc"
    fi

  else
    grep "athrill-gcc-package" ${HOME}/.bashrc > /dev/null
    if [ $? -ne 0 ]
    then
      echo "INFO: .bashrc already has athrill path:"
      grep "athrill/bin/linux/" ${HOME}/.bashrc
      echo ""
      echo "INFO: Please do following 2 commands to setup athrill-gcc path"
	  echo "echo \"export PATH=${ATHRILL_GCC_PATH}/athrill-gcc-package/usr/local/athrill-gcc/bin/:\\\${PATH}\" >> ${HOME}/.bashrc"
      echo "source ${HOME}/.bashrc"

    else
      echo "INFO: .bashrc already has athrill & athrill-gcc paths:"
      grep "athrill/bin/linux/" ${HOME}/.bashrc
      grep "athrill-gcc-package" ${HOME}/.bashrc
    fi

fi
