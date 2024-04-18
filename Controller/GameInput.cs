using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// убрать отсюда изменение топлива, здесь только инпт от пользователя
public class GameInput : MonoBehaviour
{
    [SerializeField] PlayerController pc;

    public float ShipControl()
    {
        if (!pc.canSteer) return 0;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.D))
        {
            PlayerController.ChangeFuel(-15f * Time.deltaTime);
            return -1;
            //PlayerController.ChangeFuel(-2f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            PlayerController.ChangeFuel(-15f * Time.deltaTime);
            return 1;
            //PlayerController.ChangeFuel(-2f * Time.deltaTime);
            
        }
        else
        {
            return 0;
        }
        // на телефоне
#elif UNITY_ANDROID
        if (JSONTest.UserLocalDataObject.LocalUserInfo.control_type == "gyro")
        {
            if (GyroscopeController.gyroscope.gravity.x > 0.1)
            {
                PlayerController.ChangeFuel(-15f * Time.deltaTime);
                return -GyroscopeController.gyroscope.gravity.x;
            }
            else if (GyroscopeController.gyroscope.gravity.x < -0.1)
            {
                PlayerController.ChangeFuel(-15f * Time.deltaTime);
                return -GyroscopeController.gyroscope.gravity.x;
            }
            else
            {
                return 0;
            }
        }
        else if (JSONTest.UserLocalDataObject.LocalUserInfo.control_type == "touch")
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).position.x > Screen.width / 2/* && Input.GetTouch(0).phase == TouchPhase.Stationary*/)
                {
                    PlayerController.ChangeFuel(-15f * Time.deltaTime);
                    return -1;
                }
                else if (Input.GetTouch(0).position.x < Screen.width / 2/* && Input.GetTouch(0).phase == TouchPhase.Stationary*/)
                {
                    PlayerController.ChangeFuel(-15f * Time.deltaTime);
                    return 1;
                }
                else return 0;
            }
            else
            {
                return 0;
            }
        }
        else return 0;
#else
        return 0;
#endif
    }
}
