#!/bin/bash


function check_tool()
{
	tool_name=${1}
	#echo "### <${tool_name}> : checking"
	FOUND=`which ${tool_name}`
	if [ $? -ne 0 ]
	then
		echo "##NG: <${tool_name}> Not found"
	else
		echo "##OK: <${tool_name}> Instaled: ${FOUND}"
	fi
}

check_tool gcc
check_tool make
check_tool git
check_tool ruby
check_tool wget
check_tool jq
check_tool v850-elf-gcc

