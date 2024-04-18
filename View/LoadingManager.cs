using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LoadingManager : MonoBehaviour
{
    public Slider Bar;
    public GameObject PreLoadingPanel;
    public GameObject LoadingPanel;

    public GameObject BigVioletCircle;
    public GameObject LitleVioletCircle;
    public GameObject LitleBlackCircle;
    public GameObject FigureToCover;
    public GameObject SO;

    private void Awake()
    {
        //StartCoroutine(LoadingGameAsync(PreLoadingPanel, LoadingPanel, Bar));
    }

    void LerpingPosition(GameObject obj, Vector3 firstPos, Vector3 secondPos, float time)
    {
        obj.transform.localPosition = Vector3.Lerp(firstPos, secondPos, time);
    }

    void LerpingScaling(GameObject obj, Vector3 firstScale, Vector3 secondScale, float time)
    {
        obj.transform.localScale = Vector3.Lerp(firstScale, secondScale, time);
    }

    public IEnumerator StartScreenProcess()
    {
        PreLoadingPanel.SetActive(true);

        float timer = 0;
        while (timer <= 1)
        {
            LerpingPosition(LitleBlackCircle, new Vector3(448, 753, 0), new Vector3(-225, 345, 0), timer);
            LerpingPosition(LitleVioletCircle, new Vector3(566, -497, 0), new Vector3(-137, 63, 0), timer);
            LerpingPosition(BigVioletCircle, new Vector3(-636, -698, 0), new Vector3(97, 0, 0), timer);
            LerpingScaling(SO, new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0), timer);
            timer += 0.05f;
            yield return new WaitForSeconds(0.001f);
        }

        timer = 0;

        while (timer <= 1)
        {
            LerpingPosition(LitleBlackCircle, new Vector3(-225, 345, 0), new Vector3(-48, 221, 0), timer);
            LerpingPosition(LitleVioletCircle, new Vector3(-137, 63, 0), new Vector3(-50, 215, 0), timer);
            LerpingPosition(BigVioletCircle, new Vector3(97, 0, 0), new Vector3(-50, 215, 0), timer);
            LerpingScaling(SO, new Vector3(0.5f, 0.5f, 0), new Vector3(1, 1, 0), timer);
            timer += 0.05f;
            yield return new WaitForSeconds(0.001f);
        }

        timer = 0;

        while (timer <= 1)
        {
            LerpingPosition(FigureToCover, new Vector3(-1524.5f, 0, 0), new Vector3(0, 0, 0), timer);
            timer += 0.025f;
            yield return new WaitForSeconds(0.001f);
        }

        
        PreLoadingPanel.SetActive(false);

        /*
        LoadingPanel.SetActive(true);
        Scene scene = SceneManager.GetActiveScene();

        float i = 0; 
        timer = 0;
        while (!scene.isLoaded || timer < 20)
        {
            Bar.value = (i / 10) % 10; //async.progress;
            timer += 0.1f;
            i += 0.1f;
            yield return null;
        }

        LoadingPanel.SetActive(false);
        */
    }

    public IEnumerator LoadingProcess()
    {
        Scene scene = SceneManager.GetActiveScene();

        float i = 0;
        float timer = 0;
        while (!scene.isLoaded || timer < 20)
        {
            Bar.value = (i / 10) % 10; //async.progress;
            timer += 0.1f;
            i += 0.1f;
            yield return null;
        }
    }
    
}
