using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;

// ��������:
// 1. �������� ������ ����� �������, ����� ��� ��������� � ������� ������ ���� ����� ���������
// 2. �������� ���� � ����������� �� �������������� �������� ������
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CinemachineVirtualCamera followPlayer;
    [SerializeField] private CinemachineVirtualCamera lookAtPlanet;

    private CinemachineConfiner2D confiner;

    // 100 - ������� � ����� �����
    // 0 - ������� ����� � ������ ������
    // -100 - ������� � ����� ����
    [SerializeField] private float planetYOffsetPercentage;

    public float PlanetYOffsetUnits { get; private set; }


    //private RectTransform background;
    //private const float backgroundScaleConst = 0.09375f;


    public void Initialize()
    {
        confiner = followPlayer.GetComponent<CinemachineConfiner2D>();

        // �������� ����� ����, ��� �������� ���������, ������������� �� ������ ������ � ������
        CameraConfiner.onPolygonChange += UpdateConfiner;

        FocusOnPlanet(PlanetManager.instance.firstPlanet);
        // � ������ ���� ��������� � �������, ������ �������������� ������
        //FocusOnPlanet(PlanetManager.instance.currentPlanet);

        // ���������� ������, ������������� � ���������
        InitializeCameraOffsetFromPlanet(planetYOffsetPercentage);
    }

    void Start()
    {
        Events.onOrbitLeft += FocusOnPlayer;
        Events.onOrbitEntered += FocusOnPlanet;
        //Events.onOrbitEntered += GetNextPlanetInfo;
    }

    private void FocusOnPlanet(Planet planet)
    {
        followPlayer.Priority = 1;
        followPlayer.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 1f;
        lookAtPlanet.Priority = 0;
        followPlayer.Follow = planet.transform;

    }

    private void FocusOnPlayer(Planet planet)
    {
        followPlayer.Priority = 1;
        followPlayer.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 1f;
        lookAtPlanet.Priority = 0;
        followPlayer.Follow = player;
    }

    public void UpdateConfiner()
    {
        confiner.InvalidateCache();
    }

    private void InitializeCameraOffsetFromPlanet(float offsetPercentage)
    {
        float screenHeightInUnits = Utils.GetScreenSizeInUnits().y;
        float offsetY = offsetPercentage / 100 * screenHeightInUnits * 0.5f * -1;
        lookAtPlanet.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = offsetY;
        PlanetYOffsetUnits = offsetY;
    }


    // ��� ��� ������ �������� ������, ����� ��� ����������

    //public async void GetNextPlanetInfo(Planet planet)
    //{
    //    float y1 = planet.transform.position.y - planet.planet.orbitDiameter * 0.5f;
    //    await Task.Delay(1);
    //    float y2 = SpawnerForPlanet.Instance.GetNextPlanet().transform.position.y;
    //    y2 += 60;
    //    Debug.Log("y1 = " + y1 + ", y2 = " + y2);
    //    UpdateZoomOutValues(y1, y2);
    //}

    //public void UpdateZoomOutValues(float y1, float y2)
    //{
    //    zoomOutTarget = (y2 - y1) * 0.5f;
    //    //_lookAtPlanet.m_Lens.OrthographicSize = zoomOutTarget;
    //    Events.onCameraScaling?.Invoke(zoomOutTarget);
    //}
}
