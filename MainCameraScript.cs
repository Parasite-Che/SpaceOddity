using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public Camera mainCamera; // ������ �� ������� ������
    public GameObject backgroundPrefab; // ������ ������ ����
    public float backgroundOffset = 10f; // ����������, �� ������� ��������� ����� ��� ����� �������
    public float backgroundHeight = 10f; // ������ ���� (���������� �������� ��������)

    private GameObject currentBackground; // ������� ���

    private void Start()
    {
        // ������� ������ ���
        currentBackground = Instantiate(backgroundPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void Update()
    {
        // ��������� ������� ������
        if (mainCamera.transform.position.y + backgroundOffset > currentBackground.transform.position.y + backgroundHeight / 2)
        {
            // ������� ����� ��� ����� �������
            Vector3 newPosition = new Vector3(0, currentBackground.transform.position.y + backgroundHeight, 0);
            GameObject newBackground = Instantiate(backgroundPrefab, newPosition, Quaternion.identity);

            // ������� ������ ���
            Destroy(currentBackground);

            // ��������� ������� ���
            currentBackground = newBackground;
        }
    }
}