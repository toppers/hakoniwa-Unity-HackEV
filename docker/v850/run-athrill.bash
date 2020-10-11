#!/bin/bash

WORKSPACE_DIR=`pwd`/simulator
DOCKER_IMAGE=hako/athrill-v850:v1.1.1

sudo docker run -v ${WORKSPACE_DIR}:/root/workspace -it --rm --net host --name athrill-v850 ${DOCKER_IMAGE} 
