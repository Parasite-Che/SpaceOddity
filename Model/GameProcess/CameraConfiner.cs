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

    // получить координаты в world space углов экрана
    private void UpdateScreenCornersCoordinatesInWorldSpace()
    {
        _screenBottomLeft = _camera.ScreenToWorldPoint(new Vector3(-_screenOffsetInPixels, -_screenOffsetInPixels, _camera.farClipPlane));
        _screenBottomRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width + _screenOffsetInPixels, -_screenOffsetInPixels, _camera.farClipPlane));
        _screenTopLeft = _camera.ScreenToWorldPoint(new Vector3(-_screenOffsetInPixels, Screen.height + _screenOffsetInPixels, _camera.farClipPlane));
        _screenTopRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width + _screenOffsetInPixels, Screen.height + _screenOffsetInPixels, _camera.farClipPlane));
    }

    //private void ComparePolygonCornersToScreenCorners()
    //{
    //    // экран левее, двигаем границу влево
    //    if (_screenBottomLeft.x < _bottomLeft.x)
    //    {
    //        _bottomLeft.x = _screenBottomLeft.x;
    //        _topLeft.x = _screenBottomLeft.x;
    //    }
    //    // экран правее, двигаем границу вправо
    //    if (_screenBottomRight.x > _bottomRight.x)
    //    {
    //        _bottomRight.x = _screenBottomRight.x;
    //        _topRight.x = _screenTopRight.x;
    //    }
    //    // экран выше, двигаем границу выше
    //    if (_screenTopLeft.y > _topLeft.y)
    //    {
    //        _topLeft.y = _screenTopLeft.y;
    //        _topRight.y = _screenTopRight.y;
    //    }
    //    // экран ниже, двигаем границу вниз
    //    if (_screenBottomLeft.y < _bottomLeft.y)
    //    {
    //        _bottomLeft.y = _screenBottomLeft.y;
    //        _bottomRight.y = _screenBottomLeft.y;
    //    }
    //}

    private void SetPolygonCornersToScreenSize()
    {
        // лева€ граница
        _bottomLeft.x = _screenBottomLeft.x;
        _topLeft.x = _screenBottomLeft.x;
        // права€ граница
        _bottomRight.x = _screenBottomRight.x;
        _topRight.x = _screenTopRight.x;
        // верхн€€ граница
        _topLeft.y = _screenTopLeft.y;
        _topRight.y = _screenTopRight.y;
        // нижн€€ граница
        _bottomLeft.y = _screenBottomLeft.y;
        _bottomRight.y = _screenBottomLeft.y;
    }

    // сравнить границы коллайдера с границами экрана, если экран больше, то увеличить
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
        // получаем новые позиции по х с учЄтом оффсета
        Vector2 xMinMax = PlanetManager.instance.GetXBordersFromSpawnedPlanets();
        Vector2 yMinMax = PlanetManager.instance.GetYBordersFromSpawnedPlabets();

        // сдвигаем позиции с учЄтом оффсета, чтобы граница не рпоходила пр€мо по планете, а была чуть дальше)
        // левее
        xMinMax.x -= (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100);
        // правее
        xMinMax.y += (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100);
        // ниже
        yMinMax.x -= (_yScreenSize * _yLowerOffsetAsPercentageOfScreenHeight / 100);
        // выше
        yMinMax.y += (_yScreenSize * _yUpperOffsetAsPercentageOfScreenHeight / 100);

        // экран левее, двигаем границу влево
        if (xMinMax.x < _bottomLeft.x)
        {
            _bottomLeft.x = xMinMax.x;
            _topLeft.x = xMinMax.x;
        }
        // экран правее, двигаем границу вправо
        if (xMinMax.y > _bottomRight.x)
        {
            _bottomRight.x = xMinMax.y;
            _topRight.x = xMinMax.y;
        }
        // экран выше, двигаем границу выше
        if (yMinMax.y > _topLeft.y)
        {
            _topLeft.y = yMinMax.y;
            _topRight.y = yMinMax.y;
        }
        // экран ниже, двигаем границу вниз
        if (yMinMax.x < _bottomLeft.y)
        {
            _bottomLeft.y = yMinMax.x;
            _bottomRight.y = yMinMax.x;
        }
    }

    // вызывать каждый раз при спавне планеты, чтобы обновить границы коллайдера
    // обновить, чтобы учитываль оффсеты
    private void UpdatePolygonPathOnPlanetSpawn(Planet planet)
    {
        // получаем новые позиции по х с учЄтом оффсета
        Vector2 xCorners = new Vector2
            (planet.transform.position.x - (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100),
            planet.transform.position.x + (_xScreenSize * _xOffsetAsPercentageOfScreenWidth / 100));

        // получаем новую позицию по y с учЄтом оффсета (верхн€€ граница должна быть выше самой планеты, чтобы камера не застревала
        float y = planet.transform.position.y + _yScreenSize * _yUpperOffsetAsPercentageOfScreenHeight / 100;

        // если нова€ лева€ граница левее текущей, то двигаем текущую
        if (xCorners.x < _bottomLeft.x)
        {
            _bottomLeft.x = xCorners.x;
            _topLeft.x = xCorners.x;
        }

        // если нова€ права€ граница правее текущей, то двигаем текущую
        if (xCorners.y > _bottomRight.x)
        {
            _bottomRight.x = xCorners.y;
            _topRight.x = xCorners.y;
        }

        // нова€ планета всегда выше старых, поэтому проверка не нужна
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

        // если после удалени€ осталось пустое место слева, то двигаем границу вправо
        if (xCorners.x > _bottomLeft.x)
        {
            _bottomLeft.x = xCorners.x;
            _topLeft.x = xCorners.x;
        }

        // если после удалени€ осталось пустое место справа, то двигаем границу влево
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


    // игрок вылетел за границы экрана
    public void ActOnTriggerExit(GameObject player)
    {
        Debug.Log("¬ышли за границу камеры. »вент на смерт игрока");
        Events.onDeathEvent?.Invoke();
    }
    #endregion
}
