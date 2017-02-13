using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class FileManagment
{
    public static string Encrypting(SaveData mySaveData)
    {
        BinaryFormatter bf = new BinaryFormatter();

        MemoryStream stream = new MemoryStream();
        bf.Serialize(stream, mySaveData);
        stream.Seek(0, SeekOrigin.Begin);

        var sr = new StreamReader(stream);

        var s = sr.ReadToEnd();

        var ctab = s.ToCharArray();
        for (int i = 0; i < s.Length; i++) // Adding 2 for every char in ascii
        {
            ctab[i] += (char)2;
        }
        return new string(ctab);
    }

    public static T Decrypting<T>(string s)
    {
        var ctab = s.ToCharArray();

        for (int i = 0; i < s.Length; i++) // Subtraction 2 for every char in ascii
        {
            ctab[i] -= (char)2;
        }

        BinaryFormatter bf = new BinaryFormatter();

        var stream = new MemoryStream();
        stream.Write(Encoding.ASCII.GetBytes(ctab), 0, ctab.Length);
        stream.Seek(0, SeekOrigin.Begin);

        return (T)bf.Deserialize(stream);
    }


    public static T ReadFile<T>(string fileName)
    {

        string path = GetPath(fileName);

        FileStream fileStream = File.Open(path, FileMode.Open);

        StreamReader reader = new StreamReader(fileStream);

        var data = reader.ReadToEnd();

        T loadedSave = Decrypting<T>(data);
        fileStream.Close();
        reader.Close();
        return loadedSave;
    }

    public static void WriteFile(string fileName, SaveData mySaveData)
    {
        FileChecking();

        var fileData = Encrypting(mySaveData);

        string path = GetPath(fileName);      

        File.WriteAllText(path,fileData);   
    }

    /// <summary>
    /// Checks if the directory for saves exists
    /// </summary>
    public static void FileChecking()
    {
        List<string> pathElements = new List<string>
        {
            "My Games", "Wonziu", "Saves" 
        };

        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        while (pathElements.Count > 0)
        {
            path = Path.Combine(path, pathElements[0]);
            pathElements.RemoveAt(0);
            // Debug.Log(path);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }

    public static string GetPath(string fileName)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games\\Wonziu\\Saves\\" + fileName);
    }
}