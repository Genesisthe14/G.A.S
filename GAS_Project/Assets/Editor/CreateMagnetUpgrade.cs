using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateMagnetUpgrade : MonoBehaviour
{
    //Definition eines neuen Buttons mit der dazugehoerigen Methode
    [MenuItem("Assets/Create/MagnetUpgrade")]
    static void CreateAsset()
    {
        //Gibt es einen Unterordner "Inventory Items" im Project Browser?
        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/PermanentUpgrades/MagnetUpgrades"))
        {
            //Wenn nicht, dann erstellen wir diesen
            AssetDatabase.CreateFolder("Assets/ScriptableObjects/PermanentUpgrades", "MagnetUpgrades");
        }

        //Neue Instanz von InventoryItem erstellen
        ScriptableObject asset = ScriptableObject.CreateInstance(typeof(MagnetUpgrade));

        //Aus der erstellten Instanz ein Asset im Project Browser erstellen
        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/PermanentUpgrades/MagnetUpgrades/" +
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
