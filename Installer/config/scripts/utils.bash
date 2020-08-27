#!/bin/bash

export OUT_WIN_PATH=
wslpath2winpath()
{
	arg=${1}
	OUT_WIN_PATH=`echo $arg | sed -e 's/\/mnt\/'${WSL_WIN_DRIVE_NAME}'/'${WSL_WIN_DRIVE_NAME}':/g' | sed -e 's/\//\\\\\\\\/g'`
}

export TERMINAL_PATH=
function find_terminal()
{
	if [ $TERMINAL_TYPE = "wsl" ]
	then
		if [ -f ${TERMINAL_ROOT_PATH}/wsl.exe ]
		then
			wslpath2winpath ${TERMINAL_ROOT_PATH}/wsl.exe
			export TERMINAL_PATH=${OUT_WIN_PATH}
		else
			echo "ERROR: not found WSL1"
			exit 1
		fi
	else
		export TERMINAL_PATH="/usr/bin/open"
	fi
}

