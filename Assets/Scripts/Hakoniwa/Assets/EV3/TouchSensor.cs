using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hakoniwa.Assets;

public class TouchSensor : MonoBehaviour, IRobotTouchSensor
{
    private GameObject touchSensor;
    private bool isTouched;

    public void Initialize(GameObject root)
    {
        touchSensor = root;
        this.isTouched = false;
    }
    public bool IsPressed()
    {
        return this.isTouched;
    }

    public void UpdateSensorValues()
    {
        //nothing to do
    }
    private void OnTriggerStay(Collider other)
    {
        this.isTouched = true;
        //Debug.Log("Pressed");
    }
    private void OnTriggerExit(Collider other)
    {
        this.isTouched = false;
        //Debug.Log("NotPressed");
    }
}
