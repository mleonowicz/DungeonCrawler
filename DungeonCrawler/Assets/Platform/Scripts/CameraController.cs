using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerPlatform MyPlayer;
    public Camera MyCamera;

    public float LevelWidth;
    private float screenAspect;
    private float screenWidth;

    void Start()
    {
        screenAspect = (float) Camera.main.pixelWidth / Camera.main.pixelHeight;
        screenWidth = MyCamera.orthographicSize*screenAspect;
    }

    void Update ()
    {
        float PlayerPosX = MyPlayer.transform.position.x;

        if (PlayerPosX > LevelWidth - screenWidth  + 0.75f)
            MyCamera.transform.localPosition = new Vector3(LevelWidth - screenWidth + 0.75f, 0, 0);
        else if (PlayerPosX < screenWidth - 0.75f)
            MyCamera.transform.localPosition = new Vector3(screenWidth - 0.75f, 0, 0);
        else MyCamera.transform.localPosition = new Vector3(PlayerPosX, 0, 0);
    }
}
