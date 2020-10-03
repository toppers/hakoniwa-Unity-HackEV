#!/bin/bash

if [ $# -ne 1 -a $# -ne 2 ]
then
    echo "Usage: $0 unity_project_name [apl_name]"
    exit 1
fi

source config/config.bash

if [ $# -ge 2 ]
then
	export UNITY_PROJECT_NAME=${1}
fi

if [ $# -eq 3 ]
then
	export APL_NAME=${2}
fi

if [ -d ${UNITY_PROJECT_BUILD_PATH} ]
then
    :
else
    echo "ERROR: not found directory: ${UNITY_PROJECT_BUILD_PATH}"
    exit 1
fi


cat ${UNITY_PROJECT_BUILD_PATH}/config.json | grep Udp | grep null > /dev/null
if [ $? -eq 0 ]
then
    echo "### create config.json for unity"
    bash config/scripts/create_config.bash mmap unity
else
    bash config/scripts/create_config.bash udp unity
fi

cd ${UNITY_PROJECT_BUILD_PATH}

if [ ${OS_TYPE} = "Linux" ]
then
    ./${UNITY_PROJECT_NAME}.exe &
else
    "OS_TYPE=${OS_TYPE} is not supported yet.."
fi
