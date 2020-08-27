#!/bin/bash
export OS_TYPE=`uname`
export SIM_DIR=`pwd`/simulator
export INS_DIR=`pwd`/installer
export CFG_DIR=`pwd`/config
export EV3RT_DIR=${SIM_DIR}/ev3rt-athrill-v850e2m
export SAMPLE_DIR=${SIM_DIR}/hakoniwa-scenario-samples
export UNITY_DIR=${SIM_DIR}/unity/project
export ATHRILL_GCC_LINUX=https://github.com/toppers/athrill-gcc-v850e2m/releases/download/v1.1/athrill-gcc-package.tar.gz
export ATHRILL_GCC_MAC=https://github.com/toppers/athrill-gcc-v850e2m/releases/download/v1.1-mac/athrill-gcc-package-mac.tar.gz
export UNITY_PACKAGE=https://github.com/toppers/hakoniwa-Unity-HackEV/releases/download/v2.1/single-robot-HackEV.unitypackage

