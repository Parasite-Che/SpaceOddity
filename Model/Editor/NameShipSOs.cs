using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using Unity;
using UnityEditor;

[ExecuteInEditMode]
public class NameShipSOs : EditorWindow
{
    [MenuItem("Custom/GenerateShipLabels")]
    public static void ShowWindow()
    {
        GetWindow<NameShipSOs>("Edit Mode Functions");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Run Function"))
        {
            SetLabelsInGroup();
        }
    }

    public void SetLabelsInGroup()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        List<string> list = new List<string>();
        List<string> labels = settings.GetLabels();

        AddressableAssetGroup g = settings.FindGroup("Ships");
        System.Collections.Generic.ICollection<AddressableAssetEntry> entries = g.entries;
        Debug.Log(entries.Count);
        foreach (AddressableAssetEntry entry in entries)
        {
            string name;

            if (!entry.IsFolder)
            {
                name = entry.address;

                if (!labels.Contains(name))
                {
                    settings.AddLabel(name);
                }

                entry.SetLabel(name, true);
            }

        }
    }

}
