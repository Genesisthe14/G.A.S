using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadService : MonoBehaviour
{
    private const string FILENAME = "knock_it_rocket.save";

    //Saves the game to a file called FILENAME
    public static void SaveGame()
    {
        try
        {
            SaveData data = new SaveData();

            // Pfad aus Standardpfad und Dateiname zusammensetzen
            string path = Application.persistentDataPath + "/" + FILENAME;

            string jsonSaveData = JsonUtility.ToJson(data);

            File.WriteAllText(path, jsonSaveData);
            Debug.Log("Game was saved!");
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
            Debug.LogError("File was not saved!");
        }
    }

    //Loads the game from a file called FILENAME
    public static void LoadGame()
    {
        try
        {
            // Pfad aus Standardpfad und Dateiname zusammensetzen:
            string path = Application.persistentDataPath+ "/" + FILENAME;
            
            //falls keine zuvor gespeicherte Datei gefunden werden kann return ohne etwas zu laden
            if (!File.Exists(path))
            {
                Debug.LogWarning("There is no save file to load.");
                return;
            }

            string saveFileContent = File.ReadAllText(path);

            SaveData loadedData = JsonUtility.FromJson<SaveData>(saveFileContent);
            loadedData.SetData();

            Debug.Log("Game was succesfully loaded");
            return;
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
            // Im Fehlerfall leeren Datensatz zurückliefern:
            Debug.LogError("Game was not loaded!");
            return;
        }
    }
}
