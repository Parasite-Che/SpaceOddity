using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraConfiner : MonoBehaviour, IPlayerCollision
{
    [SerializeField] private Camera _camera;
    [SerializeField] private PolygonCollider2D _cameraCollider;

    [SerializeField] private float _screenOffsetInPixels;
    [SerializeField] private float _xOffsetAsPercentageOfScreenWidth;
    [SerializeField] private float _yLowerOffsetAsPercentageOfScreenHeight;
    [SerializeField] private float _yUpperOffsetAsPercentageOfScreenHeight;
    private float _xScreenSize;
    private float _yScreenSize;

    private Vector2 _screenBottomLeft;
    private Vector2 _screenBottomRight;
    private Vector2 _screenTopLeft;
    private Vector2 _screenTopRight;

    private Vector2 _bottomLeft;
    private Vector2 _bottomRight;
    private Vector2 _topLeft;
    private Vector2 _topRight;

    public static event Action onPolygonChange;

    public void Initialize()
    {
        Events.onPlanetSpawned += UpdatePolygonPath;
        Events.onPlanetDeleted += UpdatePolygonPath;

        _xScreenSize = Utils.GetScreenSizeInUnits().x;
        _yScreenSize = Utils.GetScreenSizeInUnits().y;
    }

    // �������� ���������� � world space ����� ������
    private void UpdateScreenCornersCoordinatesInWorldSpace()
    {
        _screenBottomLeft = _camera.ScreenToWorldPoint(new Vector3(-_screenOffsetInPixels, -_screenOffsetInPixels, _camera.farClipPlane));
        _screenBottomRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width + _screenOffsetInPixels, -_screenOffsetInPixels, _camera.farClipPlane));
        _screenTopLeft = _camera.ScreenToWorldPoint(new Vector3(-_screenOffsetInPixels, Screen.height + _screenOffsetInPixels, _camera.farClipPlane));
        _screenTopRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width + _screenOffsetInPixels, Screen.height + _screenOffsetInPixels, _camera.farClipPlane));
    }

    //private void ComparePolygonCornersToScreenCorners()
    //{
    //    // ����� �����, ������� ������� �����
    //    if (_screenBottomLeft.x < _bottomLeft.x)
    //    {
    //        _bottomLeft.x = _screenBottomLeft.x;
    //        _topLeft.x = _screenBottomLeft.x;
    //    }
    //    // ����� ������, ������� ������� ������
    //    if (_screenBottomRight.x > _bottomRight.x)
    //    {
    //        _bottomRight.x = _screenBottomRight.x;
    //        _topRight.x = _screenTopRight.x;
    //    }
    //    // ����� ����, ������� ������� ����
    //    if (_screenTopLeft.y > _topLeft.y)
    //    {
    //        _topLeft.y = _screenTopLeft.y;
    //        _topRight.y = _screenTopRight.y;
    //    }
    //    // ����� ����, ������� ������� ����
    //    if (_screenBottomLeft.y < _bottomLeft.y)
    //    {
    //        _bottomLeft.y = _screenBottomLeft.y;
    //        _bottomRight.y = _screenBottomLeft.y;
    //    }
    //}

    private void SetPolygonCornersToScreenSize()
    {
        // ����� �������
        _bottomLeft.x = _screenBottomLeft.x;
        _topLeft.x = _screenBottomLeft.x;
        // ������ �������
        _bottomRight.x = _screenBottomRight.x;
        _topRight.x = _screenTopRight.x;
        // ������� �������
        _topLeft.y = _screenTopLeft.y;
        _topRight.y = _screenTopRight.y;
        // ������ �������
        _bottomLeft.y = _screenBottomLeft.y;
        _bottomRight.y = _screenBottomLeft.y;
    }

    // �������� ������� ���������� � ��������� ������, ���� ����� ������, �� ���������
    private void UpdatePolygonPath(Planet planet)
    {
        UpdateScreenCornersCoordinatesInWorldSpace();
        SetPolygonCornersToScreenSize();

        UpdatePolygonPathOnChangeInPlanets();

        Vector2[] path = { _bottomLeft, _topLeft, _topRight, _bottomRight };
        _cameraCollider.SetPath(0, path);

        onPolygonChange?.Invoke();
    }

    private void UpdatePolygonPath()
    {
        UpdateScreenCornersCoordinatesInWorldSpace();
        SetPolygonCornersToScreenSize();

        UpdatePolygonPathOnChangeInPlanets();

        Vector2[] path = { _bottomLeft, _topLeft, _topRight, _bottomRight };
        _cameraCollider.SetPath(0, path);

        onPolygonChange?.Invoke();
    }

    private void UpdatePolygonPathOnChangeInPlanets()
    {
        // �������� ����� ������� �� � � ������ �������
        Vector2 xMinMax = PlanetManager.instance.GetXBordersFromSpawnedPlanets();
        Vector2 yMinMax = PlanetManager.instance.GetYBordersFromSpawnedPlabets();

        // �������� ������� � ������ �������, ����� ������� �� ��������� ����� �� �������, � ���� ���� ������)
        // �����
        xMinMax.x -= (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100);
        // ������
        xMinMax.y += (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100);
        // ����
        yMinMax.x -= (_yScreenSize * _yLowerOffsetAsPercentageOfScreenHeight / 100);
        // ����
        yMinMax.y += (_yScreenSize * _yUpperOffsetAsPercentageOfScreenHeight / 100);

        // ����� �����, ������� ������� �����
        if (xMinMax.x < _bottomLeft.x)
        {
            _bottomLeft.x = xMinMax.x;
            _topLeft.x = xMinMax.x;
        }
        // ����� ������, ������� ������� ������
        if (xMinMax.y > _bottomRight.x)
        {
            _bottomRight.x = xMinMax.y;
            _topRight.x = xMinMax.y;
        }
        // ����� ����, ������� ������� ����
        if (yMinMax.y > _topLeft.y)
        {
            _topLeft.y = yMinMax.y;
            _topRight.y = yMinMax.y;
        }
        // ����� ����, ������� ������� ����
        if (yMinMax.x < _bottomLeft.y)
        {
            _bottomLeft.y = yMinMax.x;
            _bottomRight.y = yMinMax.x;
        }
    }

    // �������� ������ ��� ��� ������ �������, ����� �������� ������� ����������
    // ��������, ����� ��������� �������
    private void UpdatePolygonPathOnPlanetSpawn(Planet planet)
    {
        // �������� ����� ������� �� � � ������ �������
        Vector2 xCorners = new Vector2
            (planet.transform.position.x - (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100),
            planet.transform.position.x + (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100));

        // �������� ����� ������� �� y � ������ ������� (������� ������� ������ ���� ���� ����� �������, ����� ������ �� ����������
        float y = planet.transform.position.y + _yScreenSize * _yUpperOffsetAsPercentageOfScreenHeight / 100;

        // ���� ����� ����� ������� ����� �������, �� ������� �������
        if (xCorners.x < _bottomLeft.x)
        {
            _bottomLeft.x = xCorners.x;
            _topLeft.x = xCorners.x;
        }

        // ���� ����� ������ ������� ������ �������, �� ������� �������
        if (xCorners.y > _bottomRight.x)
        {
            _bottomRight.x = xCorners.y;
            _topRight.x = xCorners.y;
        }

        // ����� ������� ������ ���� ������, ������� �������� �� �����
        _topLeft.y = y;
        _topRight.y = y;

        //UpdatePolygonPath();
        onPolygonChange?.Invoke();
    }

    private void UpdatePolygonPathOnPlanetDeletion()
    {
        float minY = PlanetManager.instance.firstPlanet.transform.position.y;

        minY = minY - _yScreenSize * _yLowerOffsetAsPercentageOfScreenHeight / 100;

        Vector2 xCorners = PlanetManager.instance.GetXBordersFromSpawnedPlanets();
        xCorners = new Vector2
            (xCorners.x - (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100),
            xCorners.x + (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100));

        // ���� ����� �������� �������� ������ ����� �����, �� ������� ������� ������
        if (xCorners.x > _bottomLeft.x)
        {
            _bottomLeft.x = xCorners.x;
            _topLeft.x = xCorners.x;
        }

        // ���� ����� �������� �������� ������ ����� ������, �� ������� ������� �����
        if (xCorners.y < _bottomRight.x)
        {
            _bottomRight.x = xCorners.y;
            _topRight.x = xCorners.y;
        }

        _bottomLeft.y = minY;
        _bottomRight.y = minY;

        UpdatePolygonPath();
        onPolygonChange?.Invoke();
    }

    #region IPlayerCollision
    public void ActOnTriggerEnter(GameObject player)
    {
        ;
    }


    // ����� ������� �� ������� ������
    public void ActOnTriggerExit(GameObject player)
    {
        Debug.Log("����� �� ������� ������. ����� �� ����� ������");
        Events.onDeathEvent?.Invoke();
    }
    #endregion
}
