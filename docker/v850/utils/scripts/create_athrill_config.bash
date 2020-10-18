#!/bin/bash

if [ $# -ne 1 ]
then
	echo "Usage: $0  <aplname>"
	exit 1
fi

APL_NAME=${1}

WORKSPACE_PATH=simulator/ev3rt-athrill-v850e2m/sdk/workspace
MEM_CFG_TMPL=utils/scripts/memory_udp_txt.mo
DEV_CFG_TMPL=utils/scripts/device_config_udp_txt.mo
MEM_CFG_FILE=memory.txt
DEV_CFG_FILE=device_config.txt

bash utils/scripts/mo $MEM_CFG_TMPL > ${WORKSPACE_PATH}/${APL_NAME}/${MEM_CFG_FILE}
bash utils/scripts/mo $DEV_CFG_TMPL > ${WORKSPACE_PATH}/${APL_NAME}/${DEV_CFG_FILE}

exit 0
