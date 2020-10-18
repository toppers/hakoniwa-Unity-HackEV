#!/bin/bash

if [ $# -ne 1 ]
then
	echo "Usage: $0 <unity-projname>"
	exit 1
fi

PROJNAME=${1}

./simulator/unity/project/${PROJNAME}/Build/${PROJNAME}.exe
