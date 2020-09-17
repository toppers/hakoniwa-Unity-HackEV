#!/bin/bash
export OS_TYPE=`uname`
export SIM_DIR=`pwd`/simulator
export INS_DIR=`pwd`/installer
export CFG_DIR=`pwd`/config
export ROOT_DIR=`pwd`
export EV3RT_DIR=${SIM_DIR}/ev3rt-athrill-ARMv7-A
export SAMPLE_DIR=${SIM_DIR}/hakoniwa-scenario-samples
export UNITY_DIR=${SIM_DIR}/unity/project
export UNITY_PACKAGE=https://github.com/toppers/hakoniwa-Unity-HackEV/releases/download/v2.1/single-robot-HackEV.unitypackage
