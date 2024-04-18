using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationButtons : MonoBehaviour
{
    [SerializeField] private string str;
    [SerializeField] private int countOfParents;
    private GameObject Panel;

    public void ChangeLocalization()
    {
        JSONTest.UserLocalDataObject.LocalUserInfo.language = str;
    }

    public void CheckAndChangeLocalization(LocalizationController localizationController)
    {
        Panel = gameObject;
        if (countOfParents != 0)
        {
            for (int i = 0; i < countOfParents; i++)
            {
                Panel = Panel.transform.parent.gameObject;
            }
        }

        //if (ServerManager.ExistingCheck("/storage/localization/" + str + ".json"))
        
        Panel.SetActive(true);
        
        StartCoroutine(CheckingAndChangingLocalization(localizationController, Panel));
        
        Debug.LogWarning("/storage/localization/" + str + ".json");
        
        ChangeLocalization();

    }

    private IEnumerator CheckingAndChangingLocalization(LocalizationController localizationController, GameObject panel)
    {
        yield return StartCoroutine(ServerManager.LoadLocalizationFromServer(
            "/storage/localization/" + str + ".json", str));


        yield return StartCoroutine(localizationController.LocalizeText(
            localizationController.FindTheObjects("LocalizationText"),
            JsonController.LoadLocalizationfromRes(str),
            null, null));
            
        panel.SetActive(false);
    }
}
