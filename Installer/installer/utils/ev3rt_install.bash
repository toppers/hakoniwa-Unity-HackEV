#!/bin/bash

if [ -d ev3rt-athrill-v850e2m ]
then
    echo "INFO: ev3rt already installed."
    exit 0
fi

echo "### Install ev3rt"
git clone https://github.com/toppers/ev3rt-athrill-v850e2m.git
echo "### Success ev3rt"

