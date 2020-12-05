#!/bin/bash

ETHERNET=eth0
if [ $# -eq 1 ]
then
    ETHERNET=${1}
fi

export ETH_IPADDR=`ifconfig | grep -A1 ${ETHERNET}  | grep inet | awk '{print $2}'`
export SRV_IPADDR=`cat /etc/resolv.conf  | grep nameserver | awk '{print $2}'`
