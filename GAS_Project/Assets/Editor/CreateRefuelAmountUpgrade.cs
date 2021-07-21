using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateRefuelAmountUpgrade : MonoBehaviour
{
    //Definition eines neuen Buttons mit der dazugehoerigen Methode
    [MenuItem("Assets/Create/RefuelAmountUpgrade")]
    static void CreateAsset()
    {
        //Gibt es einen Unterordner "Inventory Items" im Project Browser?
        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/RefuelAmountUpgrades"))
        {
            //Wenn nicht, dann erstellen wir diesen
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "RefuelAmountUpgrades");
        }

        //Neue Instanz von InventoryItem erstellen
        ScriptableObject asset = ScriptableObject.CreateInstance(typeof(RefuelAmountUpgrade));

        //Aus der erstellten Instanz ein Asset im Project Browser erstellen
        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/RefuelAmountUpgrades/" +
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
