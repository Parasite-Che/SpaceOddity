using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesTest : MonoBehaviour
{
    public PlanetDatabase planetDatabase;

    public void PackDownloadTest(string key)
    {
        StartCoroutine(DownloadAddressableByKey(key));
    }

    public IEnumerator DownloadAddressableByKey(string key)    
    {

        // �������� ���
        // ������������, ����� ����������� �������� �������� ��� ������� �� ���������
        // ����� ������� ����� �������� ������ ���� ��� � ����� ����� ������� �� ����
        Addressables.ClearDependencyCacheAsync(key);

        // ������� � ������� ������ ����������� ������
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
        yield return getDownloadSize;
        Debug.Log($"Download Size of asset is: {getDownloadSize.Result}");

        // ��������� ������ �� ����� �������������
        // �� ��������� �������� ����� ������ ����� PopulatePlanetDatabase
        var addressableObjectsList = Addressables.LoadAssetsAsync<PlanetScriptableObject>(key, PopulatePlanetDatabase);
    
        // ������� � ������� ������� ��������
        while (!addressableObjectsList.IsDone && addressableObjectsList.IsValid())
        {
            DownloadStatus downloadStatus = addressableObjectsList.GetDownloadStatus();
            Debug.Log(downloadStatus.Percent);
            yield return null;
        }
        
        // ��� ��������� ��������
        yield return addressableObjectsList;
        Debug.Log("Downloaded assets");
    }

    public void PopulatePlanetDatabase(PlanetScriptableObject planetType)
    {
        planetDatabase.planets.Add(planetType);
    }

}
