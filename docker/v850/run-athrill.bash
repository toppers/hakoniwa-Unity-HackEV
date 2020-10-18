#!/bin/bash

if [ $# -ne 1 ]
then
	echo "Usage: $0 <aplname>"
	exit 1
fi

WORKSPACE_DIR=`pwd`/simulator
DOCKER_IMAGE=hako/athrill-v850:v1.1.1

echo "APL_NAME=${1}" > ./env.txt

sudo docker run -v ${WORKSPACE_DIR}:/root/workspace -it --rm --net host --env-file ./env.txt --name athrill-v850 ${DOCKER_IMAGE} 
