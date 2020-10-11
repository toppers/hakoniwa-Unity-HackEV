#!/bin/bash

if [ $# -ne 1 ]
then
	echo "Usage: $0 <apl-name>"
	exit 1
fi

SAMPLE_DIR=workspace/hakoniwa-scenario-samples/single-robot
EV3RT_DIR=workspace/ev3rt-athrill-v850e2m
APL_NAME=${1}

if [ -d ${SAMPLE_DIR}/${APL_NAME} ]
then
    :
else
    echo "ERROR: not found directory: ${SAMPLE_DIR}/${APL_NAME}"
    exit 1
fi

cp -rp ${SAMPLE_DIR}/${APL_NAME} ${EV3RT_DIR}/sdk/workspace/${APL_NAME}
