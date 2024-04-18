using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangingControlButton : MonoBehaviour
{
    private TMP_Text _buttonText;
    private Localization _localization;

    private void Start()
    {
        StartCoroutine(Initializer());
    }

    IEnumerator Initializer()
    {
        while (JSONTest.UserLocalDataObject == null)
        {
            yield return null;
        }
        
        while (JSONTest.UserLocalDataObject.LocalUserInfo == null)
        {
            yield return null;
        }

        while (JSONTest.UserLocalDataObject.LocalUserInfo.language == "")
        {
            yield return null;
        }
        
        _localization = JsonController.LoadLocalizationfromRes(JSONTest.UserLocalDataObject.LocalUserInfo.language);
        _buttonText = GetComponentInChildren<TMP_Text>();
        if (!_buttonText) Debug.LogError("Не нашли текст у кнопки смены схемы управления");

        UpdateButton();

        Events.onControlSchemeChange += UpdateButton;
        Debug.LogError("Initialized");
    }
    
    public void ButtonClick()
    {
        if (JSONTest.UserLocalDataObject.LocalUserInfo.control_type == "touch")
            JSONTest.UserLocalDataObject.LocalUserInfo.control_type = "gyro";
        else
            JSONTest.UserLocalDataObject.LocalUserInfo.control_type = "touch";

        Debug.Log(JSONTest.UserLocalDataObject.LocalUserInfo.control_type);
        Debug.Log("Clicked");
        Events.onControlSchemeChange?.Invoke();
    }

    private void UpdateButton() {
        if (JSONTest.UserLocalDataObject.LocalUserInfo.control_type == "touch")
        {
            _buttonText.text = _localization.localText["ControlTypeTouchText"].ToString();
        }
        else if(JSONTest.UserLocalDataObject.LocalUserInfo.control_type == "gyro")
        {
            _buttonText.text = _localization.localText["ControlTypeGyroText"].ToString();
        }
        else
        {
            Debug.LogError("Wrong Control");
        }
    }

}
