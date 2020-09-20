#!/bin/bash

if [ $# -ne 1 ]
then
	echo "Usage: $0 <workspace name>"
	exit 1
fi

if [ -d Installer ]
then
	:
else
	echo "ERROR: can not found Installer"
	exit 1
fi

WORKSPACE_NAME=${1}

function create_workspace_template()
{
	if [ -d ${WORKSPACE_NAME} ]
	then
		echo "ERROR: ${WORKSPACE_NAME} already exists."
		exit 1
	fi
	mkdir ${WORKSPACE_NAME}
	mkdir ${WORKSPACE_NAME}/config
	cp -rp Installer/config/* ${WORKSPACE_NAME}/config/
	mkdir ${WORKSPACE_NAME}/env
	cp -rp Installer/env/* ${WORKSPACE_NAME}/env/
	mkdir ${WORKSPACE_NAME}/build
	cp -rp Installer/build/* ${WORKSPACE_NAME}/build/
	mkdir ${WORKSPACE_NAME}/simulator
	mkdir ${WORKSPACE_NAME}/simulator/unity
	mkdir ${WORKSPACE_NAME}/simulator/unity/project
	mkdir ${WORKSPACE_NAME}/downloads
	mkdir ${WORKSPACE_NAME}/tools
}

create_workspace_template

