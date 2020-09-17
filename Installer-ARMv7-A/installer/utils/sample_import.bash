#!/bin/bash

if [ $# -ne 1 ]
then
	echo "Usage: $0 {udp|mmap}"
	exit 1
fi

COMM_TYPE=${1}


if [ $COMM_TYPE = "mmap" ]
then
	source config/config_mmap.bash
else
	source config/config_udp.bash
fi

SRC=${SAMPLE_DIR}/single-robot/${APL_NAME}
DST=${WORKSPACE_PATH_UNIX}/${APL_NAME}

if [ -d $SRC ]
then
	:
else
	echo "ERROR: can not found $SRC"
	exit 1
fi

if [ -d $DST ]
then
	echo "INFO: rewriting... already exist on workspace"
	echo "INFO: $DST"
else
	:
fi

cp -rp $SRC ${WORKSPACE_PATH_UNIX}

bash config/scripts/create_config.bash $COMM_TYPE dev
bash config/scripts/create_config.bash $COMM_TYPE mem
