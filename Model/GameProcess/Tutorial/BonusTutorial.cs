using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BonusTutorial : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    private bool wasEntered;
    
    private void Awake()
    {
        StartCoroutine(Initialize());
    }

    private void OnDisable()
    {
        
        //JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[2] = true;
    }

    private IEnumerator Initialize()
    {
        while (JSONTest.UserLocalDataObject == null)
        {
            yield return null;
        }
        
        while (JSONTest.UserLocalDataObject.LocalUserInfo == null)
        {
            yield return null;
        }
        
        //JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[2] = false;

        if (JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[2] == false)
        {
            StartCoroutine(BonusTutorCheck());
        }

        
    }

    private IEnumerator BonusTutorCheck()
    {

        while (true)
        {
            if (CheckBonusInData())
                break;
            yield return null;
        }

        Objects.instance.tutorialTextObjectsList[0].GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
        Objects.instance.tutorialTextObjectsList[1].GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
        Objects.instance.tutorialTextObjectsList[2].GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);

        
        while (JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[2] == false)
        {
            position = gameObject.transform.position - Objects.instance.allCanvas.transform.position;

            if (position.x > -50 && position.x < 50 && position.y < 81 && position.y > -81)
            {
                wasEntered = true;

                position = new Vector3(position.x, position.y,
                    Objects.instance.tutorialTextObjectsList[0].parent.localPosition.z);

                Objects.instance.tutorialTextObjectsList[0].parent.localPosition =
                    new Vector3((position.x / 62) * 400, (position.y / 110) * 711, position.z);
                if (position.x > 0)
                {
                    Objects.instance.tutorialTextObjectsList[0].gameObject.SetActive(false);
                    Objects.instance.tutorialTextObjectsList[1].gameObject.SetActive(true);
                    Objects.instance.tutorialTextObjectsList[2].gameObject.SetActive(true);
                    Objects.instance.tutorialTextObjectsList[0].parent.localPosition -= new Vector3(220, 0, 0);
                }
                else
                {
                    Objects.instance.tutorialTextObjectsList[0].gameObject.SetActive(true);
                    Objects.instance.tutorialTextObjectsList[1].gameObject.SetActive(true);
                    Objects.instance.tutorialTextObjectsList[2].gameObject.SetActive(false);
                    Objects.instance.tutorialTextObjectsList[0].parent.localPosition += new Vector3(220, 0, 0);
                }
            }
            else if (wasEntered)
            {
                wasEntered = false;
                Objects.instance.tutorialTextObjectsList[0].gameObject.SetActive(false);
                Objects.instance.tutorialTextObjectsList[1].gameObject.SetActive(false);
                Objects.instance.tutorialTextObjectsList[2].gameObject.SetActive(false);
            }

            yield return new WaitForFixedUpdate();
        }
        Objects.instance.tutorialTextObjectsList[0].gameObject.SetActive(false);
        Objects.instance.tutorialTextObjectsList[1].gameObject.SetActive(false);
        Objects.instance.tutorialTextObjectsList[2].gameObject.SetActive(false);

    }

    private bool CheckBonusInData()
    {
        if (JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[0] &&
            JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[1] &&
            JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[2] == false)
        {
            Objects.instance.tutorialTextObjectsList[1].GetComponent<TMP_Text>().text = JsonController
                .LoadLocalizationfromRes(JSONTest.UserLocalDataObject.LocalUserInfo.language).localText["BonusTutorialText"].ToString();
            return true;
        }
        else
        {
            return false;
        }
    }
}
