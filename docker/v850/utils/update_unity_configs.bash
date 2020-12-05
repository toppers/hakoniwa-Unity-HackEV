#!/bin/bash

if [ $# -ne 1 ]
then
    echo "Usage: $0 <aplname>"
    exit 1
fi
export APL_NAME=${1}

UNITY_PROJECT_PATH=simulator/unity/project/

export ETHERNET=eth0
export ETH_IPADDR=`ifconfig | grep -A1 ${ETHERNET}  | grep inet | awk '{print $2}'`
export SRV_IPADDR=`cat /etc/resolv.conf  | grep nameserver | awk '{print $2}'`

for projname in `ls ${UNITY_PROJECT_PATH}`
do
    if [ -d ${UNITY_PROJECT_PATH}/$projname ]
    then
        echo $projname
        bash utils/scripts/create_unity_config.bash ${APL_NAME} ${projname}
    fi
done
