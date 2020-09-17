#!/bin/bash


if [ -d athrill ]
then
    echo "INFO: athrill is already cloned"
else
    echo "### Install athrill core"
    git clone https://github.com/toppers/athrill.git
    echo "### Success athrill core"
fi

if [ -d athrill-target-ARMv7-A ]
then
    echo "INFO: athrill-target-ARMv7-A is already cloned"
else
    echo "### Installing athrill target(ARM)"
    git clone https://github.com/toppers/athrill-target-ARMv7-A.git
    echo "### Success athrill target(ARM)"
fi
