using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private int backTimer = 200;
    public GameInput GI;
    [SerializeField] private float BackA = 0.5f;

    private void OnEnable()
    {
        if(JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[2])
            gameObject.SetActive(false); 
        
        ActivateItems(Objects.instance.tutorialTextObjectsList, false);

        if(JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[0] == false)
            StartCoroutine(ShowFirstTouchInfo());
        if(JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[1] == false)
            StartCoroutine(ShowControlInfo());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.LogError(col.gameObject);
    }

    public IEnumerator ShowFirstTouchInfo()
    {
        Debug.LogWarning("Tutorial started");
        //Time.timeScale = 0;
        //Debug.LogError(gameObject.transform.GetChild(0).gameObject);
        //Debug.LogError(gameObject.transform.GetChild(0).gameObject.GetComponentsInChildren<Transform>()[1]);

        Objects.instance.tutorialTextObjectsList[1].GetComponent<TMP_Text>().text = JsonController
            .LoadLocalizationfromRes(JSONTest.UserLocalDataObject.LocalUserInfo.language).localText["FirstTutorialText"].ToString();
        ActivateItems(Objects.instance.tutorialTextObjectsList, true);
        StartCoroutine(TransformItems(Objects.instance.tutorialTextObjectsList, gameObject, new Tuple<float, float>[] { new(0, BackA), new(0, 1) }));

        while (JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[0] == false)
        {
            yield return null;
        }

        StartCoroutine(TransformItems(Objects.instance.tutorialTextObjectsList, gameObject, new Tuple<float, float>[] { new(BackA, 0), new(1, 0) }));
        ActivateItems(Objects.instance.tutorialTextObjectsList, false);

        //yield return new WaitForSeconds(1);

        //yield return ShowControlInfo();

        //Time.timeScale = 1;
        
    }

    IEnumerator ShowControlInfo()
    {
        while (JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[0] == false)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1);

        Debug.LogWarning("Tutorial started");

        Objects.instance.tutorialTextObjectsList[1].GetComponent<TMP_Text>().text = JsonController
            .LoadLocalizationfromRes(JSONTest.UserLocalDataObject.LocalUserInfo.language).localText["ControlTutorialText"].ToString();
        ActivateItems(Objects.instance.tutorialTextObjectsList, true);
        StartCoroutine(TransformItems(Objects.instance.tutorialTextObjectsList, gameObject, new Tuple<float, float>[] { new(0, BackA), new(0, 1) }));

        while (JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[1] == false)
        {
            if (GI.ShipControl() != 0)
            {
                JSONTest.UserLocalDataObject.LocalUserInfo.tutorComplete[1] = true;
            }

            yield return null;
        }

        StartCoroutine(TransformItems(Objects.instance.tutorialTextObjectsList, gameObject, new Tuple<float, float>[] { new(BackA, 0), new(1, 0) }));
        ActivateItems(Objects.instance.tutorialTextObjectsList, false);

        Debug.LogWarning("Tutorial ended");
    }
    
    public void ActivateItems(Transform[] objects, bool activate)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].gameObject.SetActive(activate);
        }
    }
    
    IEnumerator TransformItems(Transform[] objects, GameObject back, Tuple<float,float>[] pairs)
    {
        for (int i = 0; i <= backTimer; i++)
        {
            back.GetComponent<Image>().color =
                new Color(0, 0, 0, Mathf.Lerp(pairs[0].Item1, pairs[0].Item2, (float)i / backTimer));
            for (int j = 0; j < objects.Length; j++)
            {
                objects[j].GetComponent<TMP_Text>().color =
                    new Color(1, 1, 1, Mathf.Lerp(pairs[1].Item1, pairs[1].Item2, (float)i / backTimer));
            }
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

}
