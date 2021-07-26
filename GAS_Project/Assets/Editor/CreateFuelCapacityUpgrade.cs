using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateFuelCapacityUpgrade : MonoBehaviour
{
    //Definition eines neuen Buttons mit der dazugehoerigen Methode
    [MenuItem("Assets/Create/FuelCapacityUpgrade")]
    static void CreateAsset()
    {
        //Gibt es einen Unterordner "Inventory Items" im Project Browser?
        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/PermanentUpgrades/FuelCapacityUpgrades"))
        {
            //Wenn nicht, dann erstellen wir diesen
            AssetDatabase.CreateFolder("Assets/ScriptableObjects/PermanentUpgrades", "FuelCapacityUpgrades");
        }

        //Neue Instanz von InventoryItem erstellen
        ScriptableObject asset = ScriptableObject.CreateInstance(typeof(FuelCapacityUpgrade));

        //Aus der erstellten Instanz ein Asset im Project Browser erstellen
        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/PermanentUpgrades/FuelCapacityUpgrades/" +
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
