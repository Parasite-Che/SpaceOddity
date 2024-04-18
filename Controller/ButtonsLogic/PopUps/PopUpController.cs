using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.ComponentModel;

public class PopUpController : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject SecondPanel;
    [SerializeField] private GameObject Back;
    [SerializeField] private string whereFrom = "right";
    [SerializeField] private bool activatePopUp = true;
    [SerializeField] private bool popUpIsMoving = true;
    [SerializeField] private bool complexPopUp = false;
    [SerializeField] private Transform[] objs;

    private void Awake()
    {
        if (popUpIsMoving)
            gameObject.GetComponent<Button>().onClick.AddListener(MovingPopUp);
        else if (complexPopUp)
            gameObject.GetComponent<Button>().onClick.AddListener(ComplexStaticPopUp);
        else
            gameObject.GetComponent<Button>().onClick.AddListener(StaticPopUp);
    }

    private void MovingPopUp()
    {
        if (whereFrom == "right")
        {
            Panel.transform.localPosition = new Vector3(750, 0, -2); 
        }
        else if (whereFrom == "left")
        {
            Panel.transform.localPosition = new Vector3(-750, 0, -2);
        }
        else if (whereFrom == "bot")
        {
            Panel.transform.localPosition = new Vector3(0, -1600, -2);
        }
        else if (whereFrom == "top")
        {
            Panel.transform.localPosition = new Vector3(0, 1600, -2);
        }
        else if (whereFrom == "0")
        {
            Panel.transform.localPosition = new Vector3(0, 0, -2);
        }
        else
        {
            Debug.Log("whereFrom is incorrect");
        }
        

        StartCoroutine(LoadAsyncBackGround(5));
        StartCoroutine(Panel.GetComponent<PopUpLogic>().LoadAsync());
    }

    private void ComplexStaticPopUp()
    {
        byte resultPanelColor;
        byte resultBackColor;
        byte startPanelColor;
        byte startBackColor;
        objs = Panel.GetComponentsInChildren<Transform>();
        
        if (activatePopUp)
        {
            resultPanelColor = 255;
            resultBackColor = 102;
            startPanelColor = 0;
            startBackColor = 0;
            Panel.SetActive(activatePopUp);
        }
        else
        {
            resultPanelColor = 0;
            resultBackColor = 0;
            startPanelColor = 255;
            startBackColor = 102;
        }

        Color32 pColor = Panel.GetComponent<Image>().color;
        Panel.GetComponent<Image>().color = new Color32(pColor.r, pColor.g, pColor.b, startBackColor);
        
        for(int i = 1; i < objs.Length; i++)
        {
            Color32 oColor = objs[i].GetComponent<Image>().color;
            objs[i].GetComponent<Image>().color = new Color32(oColor.r, oColor.g, oColor.b, startPanelColor);
            StartCoroutine(Panel.GetComponent<PopUpLogic>().SmoothActivationObj(objs[i].gameObject, 
                objs[i].GetComponent<Image>().color, new Color32(oColor.r, oColor.g, oColor.b, resultPanelColor), 
                15));
        }
        Color32 backColor = Panel.GetComponent<Image>().color;
        StartCoroutine(Panel.GetComponent<PopUpLogic>().SmoothActivationObj(Panel, 
            Panel.GetComponent<Image>().color, new Color32(backColor.r, backColor.g, backColor.b, resultBackColor), 
            15));

        if (!activatePopUp)
            StartCoroutine(Panel.GetComponent<PopUpLogic>().DeactivatePanel(Panel));
    }
    
    public void StaticPopUp()
    {
        LoadAsyncBackGround(5);
        Panel.SetActive(activatePopUp);
        if (SecondPanel != null)
        {
            SecondPanel.SetActive(!activatePopUp);
        }
    }

    IEnumerator LoadAsyncBackGround(byte _changingTransparency)
    {
        if (activatePopUp)
        {
            Back.gameObject.SetActive(activatePopUp);
            
            while (Back.GetComponent<Image>().color.a <= 0.5)
            {
                Back.GetComponent<Image>().color += new Color32(0, 0, 0, _changingTransparency);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (Back.GetComponent<Image>().color.a >= 0)
            {
                Back.GetComponent<Image>().color -= new Color32(0, 0, 0, _changingTransparency);
                yield return new WaitForFixedUpdate();
            }
            
            Back.gameObject.SetActive(activatePopUp);
        }
    }
}
