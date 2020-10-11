#!/bin/bash

DOCKER_IMAGE=hako/athrill-v850
DOCKER_TAG=v1.1.1
sudo docker build -t ${DOCKER_IMAGE}:${DOCKER_TAG} -f Dockerfile.athrill .
