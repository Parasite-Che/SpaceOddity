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

        // Очистить кэш
        // используется, чтобы тестировать загрузку контента при запуске из редактора
        // иначе контент будет загружен только один раз и далее будет браться из кэша
        Addressables.ClearDependencyCacheAsync(key);

        // Вывести в консоль размер загружаемых файлов
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
        yield return getDownloadSize;
        Debug.Log($"Download Size of asset is: {getDownloadSize.Result}");

        // Загрузить ассеты со всеми зависимостями
        // по окончании загрузки будет вызван метод PopulatePlanetDatabase
        var addressableObjectsList = Addressables.LoadAssetsAsync<PlanetScriptableObject>(key, PopulatePlanetDatabase);
    
        // Вывести в консоль процент загрузки
        while (!addressableObjectsList.IsDone && addressableObjectsList.IsValid())
        {
            DownloadStatus downloadStatus = addressableObjectsList.GetDownloadStatus();
            Debug.Log(downloadStatus.Percent);
            yield return null;
        }
        
        // Ждём окончания загрузки
        yield return addressableObjectsList;
        Debug.Log("Downloaded assets");
    }

    public void PopulatePlanetDatabase(PlanetScriptableObject planetType)
    {
        planetDatabase.planets.Add(planetType);
    }

}
