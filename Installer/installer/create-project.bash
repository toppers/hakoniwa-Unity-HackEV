#!/bin/bash

source config/config.bash

if [ $# -ne 1 -a $# -ne 2 -a $# -ne 3 ]
then
	echo "Usage: $0 {udp|mmap} [apl name] [unity project name]"
	exit 1
fi


if [ $# -ge 2 ]
then
	export APL_NAME=${2}
fi

if [ $# -eq 3 ]
then
	export UNITY_PROJECT_NAME=${3}
fi


bash ${INS_DIR}/utils/create_empty.bash ${1} 

echo "### create config.json for unity"
bash config/scripts/create_config.bash ${1} unity

