using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Device.SystemInfo;

public class DeviceName : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Text>().text = "Device model: " + deviceModel+
                                               "\nDevise name " + deviceName+
                                               "\nDevise type " + deviceType;
    }
}
