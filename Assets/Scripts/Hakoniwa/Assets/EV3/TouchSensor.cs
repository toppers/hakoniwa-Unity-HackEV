using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hakoniwa.Assets;
using UnityEngine.EventSystems;

public class TouchSensor : MonoBehaviour, IRobotTouchSensor, IPointerDownHandler, IPointerUpHandler
{
    private GameObject touchSensor;
    public bool isTouched;

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


    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        this.isTouched = true;
        //Debug.Log("Pressed");
    }


    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        this.isTouched = false;
        //Debug.Log("NotPressed");
    }
}
