#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationMerger : EditorWindow
{

    public AnimatorController animator;
    public List<AnimationClip> animations;
    [MenuItem("Window/AnimationMerger")]
    static void Init()
    {
        AnimationMerger window = (AnimationMerger)EditorWindow.GetWindow(typeof(AnimationMerger));
        window.animations = new List<AnimationClip>();

        window.Show();
    }

    void OnGUI()
    {

        animator = EditorGUILayout.ObjectField(animator, typeof(AnimatorController), false) as AnimatorController;

        if (GUILayout.Button("Add"))
        {
            animations.Add(null);

        }
        for (int i = 0; i < animations.Count; i++)
        {
            animations[i] = EditorGUILayout.ObjectField(animations[i], typeof(AnimationClip), false) as AnimationClip;
        }

        if (GUILayout.Button("Merge"))
        {
            foreach (AnimationClip clip in animations)
            {
                var obj = Instantiate(clip);
                obj.name = clip.name;
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(clip));
                AssetDatabase.AddObjectToAsset(obj, AssetDatabase.GetAssetPath(animator));
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }

}
#endif
