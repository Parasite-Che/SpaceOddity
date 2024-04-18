using UnityEngine;

public class DifficultyAndScoreControler: MonoBehaviour
{
    private const float _divider = 200;
    private void Start()
    {
        Events.onOrbitEntered += RecalculateDifficulty;
        Events.onDeathEvent += DropNumberOfPlanets;
        Events.onCoinPickedUp += AddScoresFromMoney;
    }
    private void RecalculateDifficulty(Planet planet)
    {
        Objects.instance.quantityOfPlanets += 1;
        Objects.instance.difficultyMultiplier =
            Objects.instance.levelOfDifficulty + Objects.instance.quantityOfPlanets / _divider;
        
        //JSONTest.UserLocalDataObject.LocalUserInfo.scores = JSONTest.UserLocalDataObject.LocalUserInfo.scores +
            //1 * (int)Objects.instance.difficultyMultiplier + 100 / (int)planet.planet.planetDiameter;
        
        PlayerPrefs.SetInt("score",
            PlayerPrefs.GetInt("score") +
            1 * (int)Objects.instance.difficultyMultiplier + 100 / (int)planet.planet.planetDiameter);
        SceneTextController.TextListUpdater(Objects.instance.sessionScoreText, PlayerPrefs.GetInt("score").ToString());
        //Debug.LogError(1 * (int)Objects.instance.difficultyMultiplier + 100 / (int)planet.planet.planetDiameter);

        
        Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$ "+ JSONTest.UserLocalDataObject.LocalUserInfo.scores);
    }
    private void DropNumberOfPlanets()
    {
        Objects.instance.quantityOfPlanets = 0;
    }

    private void AddScoresFromMoney(Coin coin)
    {
        //JSONTest.UserLocalDataObject.LocalUserInfo.scores = JSONTest.UserLocalDataObject.LocalUserInfo.scores + coin.reward*coin.reward+2*coin.reward;
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + coin.reward * coin.reward + 2 * coin.reward);
        SceneTextController.TextListUpdater(Objects.instance.sessionScoreText, PlayerPrefs.GetInt("score").ToString());
        
        Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$ "+JSONTest.UserLocalDataObject.LocalUserInfo.scores);
    }
}
