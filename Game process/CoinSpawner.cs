using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// изменил названия переменных в соответствии с правилами -Д
public class CoinSpawner : MonoBehaviour
{
    public GameObject[] coins = new GameObject[4];

    void Start()
    {
        Events.onPlanetSpawned += AddCoin;
    }

    // мы решили писать комментарии на русском -Д
    // This function is used to add new coin near the planet
    void AddCoin(Planet planet)
    {
        // Calculate the radius from center of Planet to new coin position
        float radius = planet.transform.localScale.y + Random.Range(8, 10);
        // Calculate the x coordinate
        float posX = planet.transform.position.x + Random.Range(radius - 2, radius + 2);
        // Calculate the y coordinate
        float posY = planet.transform.position.y + Mathf.Sqrt(radius * radius - posX * posX);
        //change position of spawn point
        transform.position = new Vector3(posX, posY, planet.transform.position.z);
        // create on new place a coin
        Instantiate(getCoin(), transform.position, transform.rotation);
    }

    public GameObject getCoin()
    {
        int coef = Random.Range(0, 100);
        if (coef >= 40 && coef < 70)
            return coins[1];
        else if (coef >= 70 && coef < 90)
            return coins[2];
        else if (coef >= 90 && coef < 100)
            return coins[3];
        else return coins[0];
    }
}
