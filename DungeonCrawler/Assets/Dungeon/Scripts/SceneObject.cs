using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{
    public GameManager MyGameManager;

    void OnEnable()
    {
        StartCoroutine(MyGameManager.FadeInAnimation());
    }
}