#!/bin/bash

if [ -d mROS ]
then
    echo "INFO: mROS already installed."
else
    echo "### Install mROS"
    git clone https://github.com/tlk-emb/mROS.git
    echo "### Success mROS"
fi


if [ -d asp-athrill-mbed ]
then
    echo "INFO: asp-athrill-mbed already installed."
else
    echo "### Install asp-athrill-mbed"
    git clone https://github.com/toppers/asp-athrill-mbed.git
    echo "### Success asp-athrill-mbed"
fi
