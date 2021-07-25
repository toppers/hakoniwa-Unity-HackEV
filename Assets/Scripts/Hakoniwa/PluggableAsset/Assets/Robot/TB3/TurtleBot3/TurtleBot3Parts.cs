using Hakoniwa.PluggableAsset.Assets.Robot.TB3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBot3Parts : MonoBehaviour, ITB3Parts
{
    private string[] motors = new string[2] { 
        "Phys/RightTire", 
        "Phys/LeftTire" 
    };

    public string GetIMU()
    {
        return "Phys/Body";
    }

    public string GetLaserScan()
    {
        return "Phys/Body/LaserScanner";
    }

    public string GetMotor(int index)
    {
        return motors[index];
    }
}
