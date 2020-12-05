#!/bin/bash

WORKSPACE_PATH=simulator/ev3rt-athrill-v850e2m/sdk/workspace/

export ETHERNET=eth0
export ETH_IPADDR=`ifconfig | grep -A1 ${ETHERNET}  | grep inet | awk '{print $2}'`
export SRV_IPADDR=`cat /etc/resolv.conf  | grep nameserver | awk '{print $2}'`

for apl in `ls ${WORKSPACE_PATH}`
do
    if [ -d ${WORKSPACE_PATH}/$apl ]
    then
        echo $apl
        bash utils/scripts/create_athrill_config.bash ${apl}
    fi
done
