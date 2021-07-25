using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Class responsible for saving and loading 

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
            Debug.LogError(e.Message);
            Debug.LogError(e.GetType());
            Debug.LogError(e.StackTrace);
            Debug.LogError("Game was not loaded!");
            return;
        }
    }

    //Deletes the current save file
    public static void DeleteSaveFile()
    {
        try
        {
            // Pfad aus Standardpfad und Dateiname zusammensetzen:
            string path = Application.persistentDataPath + "/" + FILENAME;

            //falls keine zuvor gespeicherte Datei gefunden werden kann return ohne etwas zu laden
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Save File deleted");
                return;
            }
            else
            {
                Debug.Log("There is no file to delete");
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Debug.LogError(e.GetType());
            Debug.LogError(e.StackTrace);
            Debug.LogError("Save file wasn't deleted");
            return;
        }
    }
}
