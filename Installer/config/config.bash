#!/bin/bash

source env/env.bash
source config/scripts/utils.bash

export ATHRILL_PATH=`which athrill2`
export WORKSPACE_PATH_UNIX=${EV3RT_DIR}/sdk/workspace
export ROBOT_NAME="RoboModel"
export APL_NAME="test001"
export UNITY_PROJECT_NAME="single-robot"
export UNITY_PROJECT_PATH=${UNITY_DIR}/${UNITY_PROJECT_NAME}
export UNITY_PROJECT_BUILD_PATH=${UNITY_DIR}/${UNITY_PROJECT_NAME}/Build

if [ $OS_TYPE = "Linux" ]
then
	export TERMINAL_TYPE="wsl"
	export TERMINAL_ROOT_PATH=/mnt/c/Windows/System32
	export WSL_WIN_DRIVE_NAME=c
else
	export TERMINAL_TYPE="terminal"
	export TERMINAL_ROOT_PATH=/usr/bin
	export WSL_WIN_DRIVE_NAME=
fi

find_terminal

if [ $TERMINAL_TYPE = "wsl" ]
then
	wslpath2winpath ${WORKSPACE_PATH_UNIX}
	export WORKSPACE_PATH_WIN=${OUT_WIN_PATH}
else
	export WORKSPACE_PATH_WIN=${WORKSPACE_PATH_UNIX}
fi

