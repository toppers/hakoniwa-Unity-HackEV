#!/bin/bash

if [ $# -ne 2 ]
then
	echo "Usage: $0 {udp|mmap} {unity|mem|dev}"
	exit 1
fi

COMM_TYPE=${1}
CFG_TYPE=${2}
UNITY_CFG_TMPL=
MEM_CFG_TMPL=
DEV_CFG_TMPL=

MEM_CFG_FILE=
DEV_CFG_FILE=

if [ $COMM_TYPE = "mmap" ]
then
	source config/config_mmap.bash
	UNITY_CFG_TMPL=config/scripts/config_mmap_json.mo
	MEM_CFG_TMPL=config/scripts/memory_mmap_txt.mo
	DEV_CFG_TMPL=config/scripts/device_config_mmap_txt.mo
	MEM_CFG_FILE=memory_mmap.txt
	DEV_CFG_FILE=device_config_mmap.txt
else
	source config/config_udp.bash
	UNITY_CFG_TMPL=config/scripts/config_udp_json.mo
	MEM_CFG_TMPL=config/scripts/memory_udp_txt.mo
	DEV_CFG_TMPL=config/scripts/device_config_udp_txt.mo
	MEM_CFG_FILE=memory.txt
	DEV_CFG_FILE=device_config.txt
fi

if [ $CFG_TYPE = "unity" ]
then
	if [ -d  ${UNITY_PROJECT_PATH} ]
	then
		:
	else
		echo "Error: Not found ${UNITY_PROJECT_PATH}"
		exit 1
	fi
	bash config/scripts/mo $UNITY_CFG_TMPL | tee  ${UNITY_PROJECT_PATH}/config.json
	if [ -d ${UNITY_PROJECT_BUILD_PATH} ]
	then
		:
	else
		mkdir ${UNITY_PROJECT_BUILD_PATH}
	fi
	if [ ${UNITY_PROJECT_PATH} = ${UNITY_PROJECT_BUILD_PATH} ]
	then
		:
	else
		cp ${UNITY_PROJECT_PATH}/config.json  ${UNITY_PROJECT_BUILD_PATH}/config.json
	fi
	if [ $OS_TYPE = "Linux" ]
	then
		:
	else
		touch ${UNITY_PROJECT_PATH}/athrill.command
		chmod +x ${UNITY_PROJECT_PATH}/athrill.command
	fi
elif [ $CFG_TYPE = "mem" ]
then
	if [ -d  ${WORKSPACE_PATH_UNIX}/${APL_NAME} ]
	then
		:
	else
		echo "Error: Not found ${WORKSPACE_PATH_UNIX}/${APL_NAME}"
		exit 1
	fi
	bash config/scripts/mo $MEM_CFG_TMPL | tee ${WORKSPACE_PATH_UNIX}/${APL_NAME}/${MEM_CFG_FILE}
elif [ $CFG_TYPE = "dev" ]
then
	if [ -d  ${WORKSPACE_PATH_UNIX}/${APL_NAME} ]
	then
		:
	else
		echo "Error: Not found ${WORKSPACE_PATH_UNIX}/${APL_NAME}"
		exit 1
	fi
	bash config/scripts/mo $DEV_CFG_TMPL | tee ${WORKSPACE_PATH_UNIX}/${APL_NAME}/${DEV_CFG_FILE}
else
	echo "ERROR: Unknown parameter ${CFG_TYPE}"
	exit 1
fi

exit 0
