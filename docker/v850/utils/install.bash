#!/bin/bash

APL_NAME=base_practice_1

sudo docker pull kanetugu2015/athrill-v850:v1.1.1
sudo docker pull kanetugu2015/ev3rt-v850:v1.0.0

bash create-workspace.bash
bash utils/import-sample-org.bash ${APL_NAME}
bash utils/update_athrill_configs.bash
bash utils/update_unity_configs.bash ${APL_NAME}

