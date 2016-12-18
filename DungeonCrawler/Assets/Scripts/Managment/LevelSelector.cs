using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class LevelSelector : EditorWindow
{

    public List<LevelGenaratorSettings> levels = new List<LevelGenaratorSettings>();
    [MenuItem("Window/LevelSelector")]
    static void Init()
    {
        LevelSelector window = (LevelSelector)EditorWindow.GetWindow(typeof(LevelSelector));

        window.Show();
    }

    public  void OnGUI()
    {


        SerializedObject so = new SerializedObject(this);
        SerializedProperty stringsProperty = so.FindProperty("levels");

        EditorGUILayout.PropertyField(stringsProperty, true); ////        levels = EditorGUILayout.PropertyField(levels, typeof(LevelGenaratorSettings[]), false) as LevelGenaratorSettings[];
        so.ApplyModifiedProperties(); 

        GUILayout.BeginVertical();
        foreach (var levelGenaratorSettingse in levels)
        {
            if(levelGenaratorSettingse == null)continue;
            if (GUILayout.Button(levelGenaratorSettingse.name))
            {
                FindObjectOfType<LevelGenerator>().Settings = levelGenaratorSettingse;
            }
        }
        GUILayout.EndVertical();


    }
}
