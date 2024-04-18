using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenameButton : MonoBehaviour
{
    [SerializeField] private Text GreetingsText;
    [SerializeField] private InputField field;
    private InputField TextFromInputField;

    [SerializeField] private PopUpController PUC;


    public void Renaming()
    {
        if (field.text != "")
        {
            PlayerPrefs.SetString("PlayerName", field.text);
            Debug.Log(field.text);
            field.text = "";
            PUC.StaticPopUp();
        }
    }

}
