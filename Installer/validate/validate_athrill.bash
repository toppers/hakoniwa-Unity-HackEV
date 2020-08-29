#!/bin/bash

if [ $# -ne 1 ]
then
	echo "Usage: $0 {mmap|udp}"
	exit 1
fi

COM_TYPE=${1}

if [ $COM_TYPE = "mmap" ]
then
	source config/config_mmap.bash
	CFG_FILE=${WORKSPACE_PATH_UNIX}/${APL_NAME}/device_config_mmap.txt
	MEM_FILE=${WORKSPACE_PATH_UNIX}/${APL_NAME}/memory_mmap.txt
else
	source config/config_udp.bash
	CFG_FILE=${WORKSPACE_PATH_UNIX}/${APL_NAME}/device_config.txt
	MEM_FILE=${WORKSPACE_PATH_UNIX}/${APL_NAME}/memory.txt
fi

function check_device()
{
	param=${1}
	expect=${2}
	value=`cat ${CFG_FILE} | grep "${param}" | awk '{print $2}'`
	if [ -z $value ]
	then
		echo "NG: not found param($param)"
		return
	fi
	if [ $expect =  $value ]
	then
		echo "OK: param(${param}): same $expect"
	else
		echo "NG: param(${param}): device_config(${value}) expect(${expect})"
	fi
}

function check_memory()
{
	param=${1}
	expect=${2}
	value=`cat ${MEM_FILE} | grep "${param}" | awk '{print $NF}'`
	if [ -z $value ]
	then
		echo "NG: not found param($param)"
		return
	fi
	if [ $expect =  $value ]
	then
		echo "OK: param(${param}): same $expect"
	else
		echo "NG: param(${param}): device_config(${value}) expect(${expect})"
	fi
}

#device_config.txt
if [ $COM_TYPE = "mmap" ]
then
	check_device DEBUG_FUNC_VDEV_SIMSYNC_TYPE MMAP
	check_memory 0x40000000 ${WORKSPACE_PATH_UNIX}/${APL_NAME}/athrill_mmap.bin
	check_memory 0x40010000 ${WORKSPACE_PATH_UNIX}/${APL_NAME}/unity_mmap.bin
else
	check_device DEBUG_FUNC_VDEV_TX_PORTNO ${UNITY_PORTNO}
	check_device DEBUG_FUNC_VDEV_RX_IPADDR ${ATHRILL_IP_ADDR}
	check_device DEBUG_FUNC_VDEV_TX_IPADDR ${UNITY_IP_ADDR}
	check_device DEBUG_FUNC_VDEV_RX_PORTNO ${ATHRILL_PORTNO}
fi

#memmory.txt
