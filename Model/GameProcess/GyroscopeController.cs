using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.UI;
using static MovementController;

public class GyroscopeController : MonoBehaviour
{
    public static Gyroscope gyroscope;

    public void GyroOn()
    {
        gyroscope = Input.gyro;
        gyroscope.enabled = true;
    }
}
