using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateHeadstartLengthUpgrade : MonoBehaviour
{
    //Definition eines neuen Buttons mit der dazugehoerigen Methode
    [MenuItem("Assets/Create/HeadstartLengthUpgrade")]
    static void CreateAsset()
    {
        //Gibt es einen Unterordner "Inventory Items" im Project Browser?
        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/PermanentUpgrades/HeadstartLengthUpgrades"))
        {
            //Wenn nicht, dann erstellen wir diesen
            AssetDatabase.CreateFolder("Assets/ScriptableObjects/PermanentUpgrades", "HeadstartLengthUpgrades");
        }

        //Neue Instanz von InventoryItem erstellen
        ScriptableObject asset = ScriptableObject.CreateInstance(typeof(HeadstartLengthUpgrade));

        //Aus der erstellten Instanz ein Asset im Project Browser erstellen
        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/PermanentUpgrades/HeadstartLengthUpgrades/" +
        "New InventoryItem " + System.Guid.NewGuid() + ".asset");

        //Alle ungesicherten Asset-Aenderungen speichern
        AssetDatabase.SaveAssets();
        //Alle Aenderungen neuladen
        AssetDatabase.Refresh();
        //Den Fokus auf den Project Browser legen
        EditorUtility.FocusProjectWindow();
        //Das neue Asset im Project Browser selektieren
        Selection.activeObject = asset;
    }
}
