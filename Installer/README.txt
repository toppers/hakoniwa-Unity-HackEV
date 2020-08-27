0. prepare installer
 0.1 tar expand             [auto0]
 0.2 edit config params     [auto0]
 0.3 download all           [auto0]
 0.4 unity‹N“®              [manual]
 0.4 create unity project   [manual]
 0.5 unity package          [manual]

1. install
 1.1 athrill                [auto1]
 1.2 athrill                [auto1]
 1.3 v850-gcc               [auto1]
 1.4 ev3rt                  [auto1]
 1.5 sample                 [auto1]

2. create unity/ev3rt projects
 2.1 import ev3rt apl       [auto2]
 2.2 create config.json     [auto2]
 2.3 source ${HOME}/.bashrc [manual]

3. edit & build
 3.1 edit apl               [manual]
 3.2 build apl              [auto3]

4. exec
 4.1 unity start            [manual]
 4.2 athrill start          [manual]

auto0:installer/download.bash
auto1:installer/install-single-robot.bash
auto2:installer/create-project.bash
auto3:build/ev3rt_apl_build.bash

install operation example:
 $ bash installer/download.bash
 $ bash installer/install-single-robot.bash
 $ bash installer/install-v850-gcc.bash
 $ source ${HOME}/.bashrc   (windows)
 $ bash                     (mac)
 $ bash installer/create-project.bash mmap
 $ bash build/ev3rt_apl_build.bash

TODO:
 install gcc
 install make
 install git
 install ruby
 install ruby shell
 install unity
 install athrill-gcc for linux
 install athrill-gcc for mac
 install wget
 
