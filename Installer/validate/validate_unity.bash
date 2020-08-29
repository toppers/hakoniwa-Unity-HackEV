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
else
	source config/config_udp.bash
fi

function check_entry
{
	json_file=${1}
	json_entry="${2}"
	target_entry="${3}"
	entry=`cat ${json_file} | jq -r ${json_entry}`
	if [ $entry = $target_entry ]
	then
		echo "OK: same ${target_entry}"
	else
		echo "NG: config.json(${entry}) configxx.bash(${target_entry})"
	fi
}

cat ${UNITY_PROJECT_PATH}/config.json
if [ $? -ne 0 ]
then
	echo "NG: Invalid json format: ${UNITY_PROJECT_PATH}/config.json"
	exit 1
fi

check_entry ${UNITY_PROJECT_PATH}/config.json .AthrillPath ${ATHRILL_PATH}

check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].WorkspacePathUnix ${WORKSPACE_PATH_UNIX}
if [ $OS_TYPE = "Linux" ]
then
	check_entry ${UNITY_PROJECT_PATH}/config.json .TerminalPath "${WSL_WIN_DRIVE_NAME}:\\Windows\\System32\\wsl.exe"
	check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].WorkspacePathWin  `echo ${WORKSPACE_PATH_WIN} | sed -e 's/\\\\\\\\/\\\\/g'`
else
	check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].WorkspacePathWin  ${WORKSPACE_PATH_WIN}
	check_entry ${UNITY_PROJECT_PATH}/config.json .TerminalPath "/usr/bin/open"
fi


check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].ApplicationName ${APL_NAME}
check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].BinaryName asp

if [ $COM_TYPE = "mmap" ]
then
	check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].Udp null
else
	check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].Udp.AthrillIpAddr ${ATHRILL_IP_ADDR}
	check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].Udp.AthrillPort ${ATHRILL_PORTNO}
	check_entry ${UNITY_PROJECT_PATH}/config.json .robots[0].Udp.UnityPort ${UNITY_PORTNO}
fi
