#!/bin/bash

WORKSPACE_DIR=`pwd`/simulator
DOCKER_IMAGE=hako/ev3rt-v850:v1.0.0

sudo docker run -v ${WORKSPACE_DIR}:/root/workspace -it --rm --net host --name ev3rt-v850 ${DOCKER_IMAGE} 
