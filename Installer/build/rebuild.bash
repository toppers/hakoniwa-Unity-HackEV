#!/bin/bash

source config/config.bash

cd ${WORKSPACE_PATH_UNIX}

make -f ../../../../build/Makefile.workspace img=${APL_NAME}

