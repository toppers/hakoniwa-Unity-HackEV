#!/bin/bash

source config/config.bash

function check_path()
{
	dir_path=${1}
	if [ -d  ${dir_path} ]
	then
		echo "##OK: found ${dir_path}"
	else
		echo "##NG: not found ${dir_path}"
	fi
}

#EV3RT
check_path simulator
check_path simulator/ev3rt-athrill-v850e2m
check_path simulator/ev3rt-athrill-v850e2m/sdk
check_path simulator/ev3rt-athrill-v850e2m/sdk/workspace
check_path simulator/ev3rt-athrill-v850e2m/sdk/workspace/${APL_NAME}

check_path simulator/unity
check_path simulator/unity/project
check_path simulator/unity/project/${UNITY_PROJECT_NAME}
check_path simulator/unity/project/${UNITY_PROJECT_NAME}/Build
check_path simulator/unity/project/${UNITY_PROJECT_NAME}/config.json
check_path simulator/unity/project/${UNITY_PROJECT_NAME}/Build/config.json
