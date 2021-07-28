using Hakoniwa.PluggableAsset.Assets.Robot.TB3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBot3Parts : MonoBehaviour, ITB3Parts
{
    private string[] motors = new string[2] {
        "base_footprint/imu_link/wheel_right_link/Wheel",
        "base_footprint/imu_link/wheel_left_link/Wheel"
    };

    public string GetIMU()
    {
        return "base_footprint/imu_link";
    }

    public string GetLaserScan()
    {
        return "base_footprint/imu_link/base_link/base_scan/Scan";
    }

    public string GetMotor(int index)
    {
        return motors[index];
    }
}
