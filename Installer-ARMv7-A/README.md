# 概要

ここでは，ARM版単体ロボット向けシミュレータのインストーラを管理しています．  
[箱庭Webサイトのインストール手順](https://toppers.github.io/hakoniwa/single-robot-setup/single-robot-setup-index/)において，手動でインストールすることも可能ですが，操作ミス等が発生する可能性もあります．そこで，インストール自動化可能なものを本リポジトリでツール化しています．

# 前提

インストーラは，以下の環境を前提とします．

* 対象OS
  * Windows10
  * Mac
* コマンド/ツール類(事前にインストールされていること)
  * Unity
  * WSL1(Windows10の場合)
  * gcc
  * make
  * git
  * ruby
  * wget
  * jq

# インストールの流れ

インストールの全体の流れは以下の通りです．  
自動化したところは強調表示し，()内にツール番号を表記しています．

1. 本リポジトリを clone します．
2. 単体ロボット向けシミュレータのコンフィグを行います(後述)．
3. ***unityパッケージをダウンロードします(installer-1)．***
4. Unityを起動します．
5. Unityプロジェクトを新規作成します．  
    * プロジェクトは，simulator/unity/project 配下としてください．
    * プロジェクト名は，config/config.bash の *UNITY_PROJECT_NAME* と一致させてください
6. Unityパッケージをインポートします．
7. ***以下をインストールします(installer-2)．***  
   * ARM版 athrill
   * ARM版 gcc
   * ARM版 ev3rt プロジェクト一式
   * 単体ロボット向けシミュレーション・サンプルコード一式
8. ***サンプルコードを ev3rt プロジェクトにインポートします(installer-3)．***
9. ***Athrill向けのコンフィグファイルを配置します(installer-3)***
10. ***Unity向けのコンフィグファイル(config.json)を配置します(installer-3)．***
11. .bashrc の環境変数を適用します

# インストーラ

## installer-1

unityパッケージをdownloads直下にダウンロードするツールです．  

* ツール配置場所
  * installer/
* ファイル名
  * download.bash
* ダウンロードしたファイルの配置場所
  * downloads/

使い方は以下の通りです．

```shell
 $ bash installer/download.bash
```

## installer-2

以下をインストールするツールです．

1. ARM版 athrill
2. ARM版 gcc
3. ARM版 ev3rt プロジェクト一式
4. 単体ロボット向けシミュレーション・サンプルコード一式


* ツール配置場所
  * installer/
* ファイル名
  * install-single-robot.bash
* インストールしたファイルの配置場所
  * simulator/

使い方は以下の通りです．

```shell
 $ bash installer/install-single-robot.bash
```

## installer-3

本ツールは，config/config.bash の *UNITY_PROJECT_NAME* で指定されたUnityプロジェクトにコンフィグファイルを自動生成します．また，config/config.bash の*APL_NAME* で指定されたサンプルプログラム(例：mros-obj)をev3rtプロジェクトにインポートし，Athrillのコンフィファイルも自動生成/配置します．

* ツール配置場所
  * installer/
* ファイル名
  * import-project.bash
* 作成したファイルの配置場所
  * simulator/ev3rt-athrill-ARMv7-A/sdk/workspace (サンプルコード)
  * simulator/ev3rt-athrill-ARMv7-A/sdk/workspace/<*APL_NAME*> (Athrill向けのコンフィグファイル)
  * simulator/unity/project/<*UNITY_PROJECT_NAME*> (Unity向けのコンフィグファイル)

使い方は以下の通りです(例：mmapの場合)．

```shell
 $ bash installer/import-project.bash mmap
```

上記はMMAPの例ですが，UDPの場合は，udpとしてください．

# コンフィグファイルの説明

インストーラが各種設定ファイルなどを自動生成するために事前に設定いただく必要があるファイルは，以下のものがあります．

* config/config.bash(通信方式に依存しない共通コンフィグファイル)
* config/config_mmap.bash (通信方式を MMAP とする場合)
* config/config_udp.bash (通信方式を UDP とする場合)

## config.bash

以下のパラメータを設定ください．

* ROBOT_NAME
  * 基本的には変更不要です．
  * Unity上で動作させるロボット名を変更する場合は本パラメータも変更ください．
  * 例：```export ROBOT_NAME="RoboModel_1"```
* APL_NAME
  * サンプルプログラム名を指定下さい．
  * 例：```export APL_NAME="line_trace"```
* UNITY_PROJECT_NAME
  * Unity のプロジェクト名と同じ名前を指定下さい．
  * 例：```export UNITY_PROJECT_NAME="single-robot"```

## config/config_mmap.bash

MMAPの場合は，設定事項は特にありません．

## config/config_udp.bash

以下のパラメータを設定ください．

* ATHRILL_IP_ADDR
  * Athrill が動作する PC の IP アドレスを指定してください．
  * 例：```export ATHRILL_IP_ADDR="127.0.0.1"```
* UNITY_IP_ADDR
  * Unity が動作する PC の IP アドレスを指定してください．
  * 例：```export ATHRILL_IP_ADDR="127.0.0.1"```
* ATHRILL_PORTNO
  * Athrill の受信ポート番号を指定してください．
  * 例：```export ATHRILL_PORTNO=54002```
* UNITY_PORTNO
  * Unity の受信ポート番号を指定してください．
  * 例：```export UNITY_PORTNO=54001```

IPアドレスの調べ方については，以下をご参照ください．

https://toppers.github.io/hakoniwa/single-robot-setup-detail/61_unity_install_udp_v2.0/
