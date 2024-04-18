using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgreementToggle : MonoBehaviour, IToggleController
{
    [SerializeField] private Button ContinueButton;
    
    public void OnToggleOn()
    {
        ContinueButton.interactable = true;
        JSONTest.UserLocalDataObject.LocalUserInfo.isAgreePrivacy = true;
    }

    public void OnToggleOff()
    {
        ContinueButton.interactable = false;
        JSONTest.UserLocalDataObject.LocalUserInfo.isAgreePrivacy = false;
    }
    
}
