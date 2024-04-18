using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public Camera mainCamera; // ссылка на главную камеру
    public GameObject backgroundPrefab; // префаб нового фона
    public float backgroundOffset = 10f; // расстояние, на котором создается новый фон перед камерой
    public float backgroundHeight = 10f; // высота фона (установите реальные значения)

    private GameObject currentBackground; // текущий фон

    private void Start()
    {
        // Создаем первый фон
        currentBackground = Instantiate(backgroundPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void Update()
    {
        // Проверяем позицию камеры
        if (mainCamera.transform.position.y + backgroundOffset > currentBackground.transform.position.y + backgroundHeight / 2)
        {
            // Создаем новый фон перед камерой
            Vector3 newPosition = new Vector3(0, currentBackground.transform.position.y + backgroundHeight, 0);
            GameObject newBackground = Instantiate(backgroundPrefab, newPosition, Quaternion.identity);

            // Удаляем старый фон
            Destroy(currentBackground);

            // Обновляем текущий фон
            currentBackground = newBackground;
        }
    }
}