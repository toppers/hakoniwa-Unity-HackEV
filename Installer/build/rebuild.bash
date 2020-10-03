#!/bin/bash

source config/config.bash

if [ $# -eq 1 ]
then
    export APL_NAME=${1}
fi

cd ${WORKSPACE_PATH_UNIX}

make -f ../../../../build/Makefile.workspace img=${APL_NAME}

