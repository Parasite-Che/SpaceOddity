using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    private Toggle toggle;
    private IToggleController toggleController;

    private void Awake()
    {
        
        toggle = gameObject.GetComponent<Toggle>();
        toggleController = gameObject.GetComponent<IToggleController>();
        
        toggle.onValueChanged.AddListener(delegate {
            if (toggle.isOn)
            {
                toggleController.OnToggleOn();
            }
            else
                toggleController.OnToggleOff();
        });
    }
}

public interface IToggleController
{
    public void OnToggleOn();

    public void OnToggleOff();
    

}
