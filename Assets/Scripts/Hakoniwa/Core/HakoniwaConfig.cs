using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hakoniwa.Core
{

    [System.Serializable]
    public class HakoniwaConfigInfo
    {
        public string AthrillPath;
        public string TerminalPath;
        public HakoniwaRobotConfigInfo[] robots;
    }
    [System.Serializable]
    public class HakoniwaRobotConfigInfo
    {
        public string RobotName;
        public string WorkspacePathWin;
        public string WorkspacePathUnix;
        public string ApplicationName;
        public string BinaryName;
#if VDEV_IO_MMAP
#else
        public HakoniwaRobtUdpConfigInfo Udp;
#endif
    }
#if VDEV_IO_MMAP
#else
    [System.Serializable]
    public class HakoniwaRobtUdpConfigInfo
    {
        public string AthrillIpAddr;
        public int AthrillPort;
        public int UnityPort;
    }
#endif

    public class HakoniwaConfig : MonoBehaviour
    {
        private string configPath = "config.json";
        private HakoniwaConfigInfo cfg = null;
        private string FilePath;
        // Start is called before the first frame update
        public void Initialize()
        {
#if UNITY_EDITOR
            FilePath = Directory.GetCurrentDirectory();
#else
            FilePath = AppDomain.CurrentDomain.BaseDirectory;
#endif
            Debug.Log(FilePath);
            configPath = FilePath + System.IO.Path.DirectorySeparatorChar + configPath;
            try
            {
                string jsonString = File.ReadAllText(configPath);
                Debug.Log("jsonString=" + jsonString);
                this.cfg = JsonUtility.FromJson<HakoniwaConfigInfo>(jsonString);
                Debug.Log("cfg=" + cfg);
            }
            catch (Exception e)
            {
                Debug.LogError("Not found config file:" + e);
            }
            Debug.Log("json=" + cfg);
            Debug.Log("TerminalPath=" + cfg.TerminalPath);
            Debug.Log("AthrillPath=" + cfg.AthrillPath);
            Debug.Log("Robot=" + cfg.robots);
            for (int i = 0; i < cfg.robots.Length; i++)
            {
                Debug.Log("RobotName=" + cfg.robots[i].RobotName);
                Debug.Log("ApplicationName=" + cfg.robots[i].ApplicationName);
                Debug.Log("BinaryName=" + cfg.robots[i].BinaryName);
                Debug.Log("WorkspacePathWin=" + cfg.robots[i].WorkspacePathWin);
                Debug.Log("WorkspacePathUnix=" + cfg.robots[i].WorkspacePathUnix);
#if VDEV_IO_MMAP
#else
                Debug.Log("AthrillIpAddr=" + cfg.robots[i].Udp.AthrillIpAddr);
                Debug.Log("AthrillPort=" + cfg.robots[i].Udp.AthrillPort);
                Debug.Log("UnityPort=" + cfg.robots[i].Udp.UnityPort);
#endif
            }
        }
        public string GetCurrentPath()
        {
            return this.FilePath;
        }

        public HakoniwaConfigInfo GetConfig()
        {
            //Debug.Log("cfg=" + cfg);
            return cfg;
        }
        public HakoniwaRobotConfigInfo GetRobotConfig(string robotName)
        {
            if (this.cfg == null)
            {
                return null;
            }
            int i = 0;
            for (i = 0; i < cfg.robots.Length; i++)
            {
                //Debug.Log("chk=" + cfg.robots[i].RobotName + ":arg=" + robotName);
                if (cfg.robots[i].RobotName.Equals(robotName))
                {
                    return cfg.robots[i];
                }
            }
            return null;
        }
        public string GetRobotMmapWriterFilePath(string robotName)
        {
            HakoniwaRobotConfigInfo robot_cfg = GetRobotConfig(robotName);
            if (robot_cfg != null)
            {
                return robot_cfg.WorkspacePathWin + System.IO.Path.DirectorySeparatorChar + robot_cfg.ApplicationName + System.IO.Path.DirectorySeparatorChar + "unity_mmap.bin";
            }
            else
            {
                return null;
            }
        }
        public string GetRobotMmapReaderFilePath(string robotName)
        {
            HakoniwaRobotConfigInfo robot_cfg = GetRobotConfig(robotName);
            if (robot_cfg != null)
            {
                return robot_cfg.WorkspacePathWin + System.IO.Path.DirectorySeparatorChar + robot_cfg.ApplicationName + System.IO.Path.DirectorySeparatorChar + "athrill_mmap.bin";
            }
            else
            {
                return null;
            }
        }
    }
}