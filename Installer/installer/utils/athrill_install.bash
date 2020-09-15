#!/bin/bash


if [ -d athrill ]
then
    echo "INFO: athrill is already cloned"
else
    echo "### Install athrill core"
    git clone https://github.com/toppers/athrill.git
    echo "### Success athrill core"
fi

if [ -d athrill-target-v850e2m ]
then
    echo "INFO: athrill-target-v850e2m is already cloned"
else
    echo "### Installing athrill target(v850)"
    git clone https://github.com/toppers/athrill-target-v850e2m
    echo "### Success athrill target(v850)"
fi
