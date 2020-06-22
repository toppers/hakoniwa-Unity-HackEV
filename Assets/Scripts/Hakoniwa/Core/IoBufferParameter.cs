using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IoBufferParameterType
{
    public string name;
    public int off;
    public int size;
}


public class IoBufferParameter : MonoBehaviour
{
    public List<IoBufferParameterType> sensor_params = new List<IoBufferParameterType>();
    public List<IoBufferParameterType> actuator_params = new List<IoBufferParameterType>();
    public int base_sensor_offset = 32;
    public int base_actuator_offset = 32;

    public int GetSensorOffset(string name)
    {
        int off = -1;
        foreach(IoBufferParameterType e in sensor_params)
        {
            //Debug.Log("name=" + e.name + " off=" + e.off);
            if (e.name == name)
            {
                //Debug.Log("found");
                return e.off + base_sensor_offset;
            }
        }
        //Debug.Log("not found:" + name);
        return off;
    }
    public int GetActuatorOffset(string name)
    {
        int off = -1;
        foreach (IoBufferParameterType e in actuator_params)
        {
            if (e.name == name)
            {
                return e.off + base_actuator_offset;
            }
        }
        return off;
    }

}
