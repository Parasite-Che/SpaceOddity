using UnityEngine;

public class PlayButton : MonoBehaviour
{
    //public GameManager gameManager;
    public GameObject MainMenuUI;
    public GameObject GameplayUI;

    public void PressTheButton()
    {
        Events.onGameStart?.Invoke();
        StartCoroutine(SceneTextController.TextCounterInitializer());
        MainMenuUI.SetActive(false);
        //gameManager.SetInMainMenu(false);
        GameplayUI.SetActive(true);
    }
}
