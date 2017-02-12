using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Threading;

public class MinimapController : MonoBehaviour
{

    public bool animMap = false;
    public bool minimapIsActive;
    public Camera main;

    public float mainCameraSize;

    public Vector3 MainCameraPos = new Vector3(0, 0, -10);

    // Use this for initialization
    void Start()
    {
        mainCameraSize = main.orthographicSize;

        minimapIsActive = false;
    }

    public IEnumerator LerpOutMinimap()
    {
        var time = 0f;
        minimapIsActive = true;
        animMap = true;

        while (time < 1f)
        {
            time += Time.deltaTime * 2;

            main.orthographicSize = Mathf.Lerp(mainCameraSize, LevelGenerator.instance.MinimapCameraOrthographicSize, time);
            main.transform.position = Vector3.Lerp(transform.position + MainCameraPos, LevelGenerator.instance.MinimapCameraPosition, time);

            yield return new WaitForEndOfFrame();
        }

        animMap = false;
    }

    public IEnumerator LerpInMinimap()
    {
        var time = 0f;
        animMap = true;
        while (time < 1f)
        {
            time += Time.deltaTime * 2;

            main.orthographicSize = Mathf.Lerp(LevelGenerator.instance.MinimapCameraOrthographicSize, mainCameraSize, time);
            main.transform.position = Vector3.Lerp(LevelGenerator.instance.MinimapCameraPosition, transform.position + MainCameraPos, time);

            yield return new WaitForEndOfFrame();
        }
        minimapIsActive = false;
        animMap = false;
    }
}
