#!/bin/bash

source config/config.bash

if [ $# -ne 1 ]
then
	echo "Usage: $0 {udp|mmap}"
	exit 1
fi

echo "### Importing sample"
bash ${INS_DIR}/utils/sample_import.bash ${1} 

echo "### create config.json for unity"
bash config/scripts/create_config.bash ${1} unity

