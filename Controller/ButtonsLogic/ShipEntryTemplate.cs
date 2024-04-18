using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ShipEntryTemplate : MonoBehaviour
{
    [SerializeField] private ShipThumbnailSO ship;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text shipName;
    private bool isShipDownloading = false;

    private void Start()
    {
        Events.onShipSelected += SelectShip;
    }

    public void SetEntryValues(ShipThumbnailSO ship_)
    {
        ship = ship_;
        //cost.text = ship.cost.ToString();
        buttonText.text = PlayerPrefs.GetString(ship.key, "") != "" ? "Select" : ship.cost.ToString();
        image.sprite = ship.sprite;
        shipName.text = ship.key;
        if (ship.cost == 0 && PlayerPrefs.GetString(ship.key, "") == "")
        {
            BuyShip();
        }
    }

    private void UpdateValues()
    {
        SetEntryValues(ship);
    }

    public void DoAction()
    {
        if (PlayerPrefs.GetString(ship.key, "") == "")
        {
            BuyShip();
        }
        else
        {
            Events.onShipSelected?.Invoke(ship);
        }
    }

    private void BuyShip()
    {
        if (ship.cost > GameManager.Instance.coins) return;
        
        PlayerPrefs.SetString(ship.key, "purchased");
        UpdateValues();
        Debug.Log("Buy ship");
        Events.onShipPurchased?.Invoke(ship);

        Objects.instance.debugText.text = $"{ship.key} bought";
    }

    private void SelectShip(ShipThumbnailSO ship)
    {
        if (ship == this.ship && PlayerPrefs.GetString(this.ship.key, "") == "purchased")
        {
            Debug.Log("Select ship");
            Objects.instance.debugText.text = $"{ship.key} selected";
            buttonText.text = "Selected";
        }
        else if(PlayerPrefs.GetString(this.ship.key, "") == "purchased")
        {
            buttonText.text = "Select";
        }
    }

    //public IEnumerator DownloadShip(string key)
    //{
    //    // Вывести в консоль размер загружаемых файлов
    //    AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
    //    yield return getDownloadSize;
    //    Debug.Log($"Download Size of asset is: {getDownloadSize.Result}");

    //    // Загрузить ассеты со всеми зависимостями
    //    // по окончании загрузки будет вызван метод PopulatePlanetDatabase
    //    var addressableObjectsList = Addressables.LoadAssetsAsync<ShipScriptableObject>(key, NotifyShipDatabase);

    //    // Вывести в консоль процент загрузки
    //    while (!addressableObjectsList.IsDone && addressableObjectsList.IsValid())
    //    {
    //        DownloadStatus downloadStatus = addressableObjectsList.GetDownloadStatus();
    //        Debug.Log(downloadStatus.Percent);
    //        yield return null;
    //    }

    //    // Ждём окончания загрузки
    //    yield return addressableObjectsList;
    //    Debug.Log("Downloaded assets");
    //}

    //// почему-то вызывается дважды
    //public void NotifyShipDatabase(ShipScriptableObject ship)
    //{
    //    Events.onShipDownloaded?.Invoke(ship);
    //}
}
