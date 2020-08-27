#概要

ここでは，v850版単体ロボット向けシミュレータのインストーラを管理しています．  
[箱庭Webサイトのインストール手順](https://toppers.github.io/hakoniwa/single-robot-setup/single-robot-setup-index/)手動でインストールすることも可能ですが，操作ミス等が発生する可能性もあります．そこで，インストール自動化可能なものを本リポジトリでツール化しています．

#前提

本インストーラは，以下の環境を前提とします．

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

#インストールの流れ

インストールの全体の流れは以下の通りです．  
自動化したところは強調表示し，()内にツール番号を表記しています．

1. 本リポジトリを clone します．
2. 単体ロボット向けシミュレータのコンフィグを行います．
3. ***unityパッケージをダウンロードします(installer-1)．***
4. ***v850版gccをダウンロードします(installer-1)．***
5. Unityを起動します．
6. Unityプロジェクトを新規作成します．
7. Unityパッケージをインポートします．
8. ***以下をインストールします(installer-2)．***  
   * v850版 athrill
   * v850版 gcc
   * v850版 ev3rt プロジェクト一式
   * 単体ロボット向けシミュレーション・サンプルコード一式
9. ***サンプルコードを ev3rt プロジェクトにインポートする(installer-3)．***
10. ***Athrill向けのコンフィグファイルを配置する(installer-3)***
11. ***Unity向けのコンフィグファイル(config.json)を配置する(installer-3)．***
12. .bashrc の環境変数を適用する

#インストーラ

##installer-1

unityパッケージとv850版gccをdownloads直下にダウンロードするツールです．  

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

##installer-2

以下をインストールするツールです．

1. v850版 athrill
2. v850版 gcc
3. v850版 ev3rt プロジェクト一式
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

##installer-3

本ツールは，config/config.bash のUNITY_PROJECT_NAMEで指定されたUnityプロジェクトにコンフィグファイルを自動生成します．また，config/config.bashのAPL_NAMEで指定されたサンプルプログラム(例：line_trace)をev3rtプロジェクトにインポートし，Athrillのコンフィファイルも自動生成/配置します．

* ツール配置場所
  * installer/
* ファイル名
  * create-project.bash
* 作成したファイルの配置場所
  * simulator/ev3rt-athrill-v850e2m/sdk/workspace (サンプルコード)
  * simulator/ev3rt-athrill-v850e2m/sdk/workspace/<APL_NAME> (Athrill向けのコンフィグファイル)
  * simulator/unity/project/<UNITY_PROJECT_NAME> (Unity向けのコンフィグファイル)

使い方は以下の通りです．

```shell
 $ bash installer/install-single-robot.bash
```

