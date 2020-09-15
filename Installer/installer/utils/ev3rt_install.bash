#!/bin/bash

if [ -d ev3rt-athrill-v850e2m ]
then
    echo "INFO: ev3rt already installed."
else
    echo "### Install ev3rt"
    git clone https://github.com/toppers/ev3rt-athrill-v850e2m.git
    echo "### Success ev3rt"
fi


if [ -d athrill ]
then
    echo "INFO: athrill already installed."
else
    echo "### Install athrill core"
    git clone https://github.com/toppers/athrill.git
    echo "### Success athrill core"
fi
