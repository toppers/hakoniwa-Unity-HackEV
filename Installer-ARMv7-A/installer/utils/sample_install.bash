#!/bin/bash

if [ -d hakoniwa-scenario-samples ]
then
    echo "INFO: sample scenario already installed."
    exit 0
fi

echo "### Install sample"
git clone https://github.com/toppers/hakoniwa-scenario-samples.git
echo "### Success sample"

