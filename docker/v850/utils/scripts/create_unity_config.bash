#!/bin/bash

if [ $# -ne 2 ]
then
	echo "Usage: $0  <aplname> <unity-project>"
	exit 1
fi

APL_NAME=${1}
UNITY_PROJECT_NAME=${2}

source utils/scripts/ipaddr.bash
echo "ETH_IPADDR=${ETH_IPADDR}"
echo "SRV_IPADDR=${SRV_IPADDR}"

UNITY_PROJECT_PATH=simulator/unity/project/${UNITY_PROJECT_NAME}
UNITY_CFG_TMPL=utils/scripts/config_udp_json.mo

bash utils/scripts/mo $UNITY_CFG_TMPL >  ${UNITY_PROJECT_PATH}/config.json
cp ${UNITY_PROJECT_PATH}/config.json  ${UNITY_PROJECT_PATH}/Build/config.json


exit 0
