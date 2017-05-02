using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public UnityAction PickUpItem;

    public GameObject InventoryUI;
    private bool isMoving = false;
    int layerMask = 1 << 8;
    public PlayerData PlayerData;


    //  private Vector3 miniCameraPos;
    private bool IsOnExit;
    public int CurrentHP;
    public int CurrentMP;

    private MinimapController minimapController;

    //    private float miniCameraSize;

    void Start()
    {
        minimapController = GetComponent<MinimapController>();
        CurrentHP = PlayerData.MaxHP * PlayerData.Str;
        CurrentMP = PlayerData.MaxMP * PlayerData.Int;
    }

    void Update()
    {
        if (IsOnExit && Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("You Win");
        }
        if (Input.GetKeyDown(KeyCode.M) && !minimapController.animMap)
        {
            StartCoroutine(!minimapController.minimapIsActive ? minimapController.LerpOutMinimap() : minimapController.LerpInMinimap());
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUI.SetActive(!InventoryUI.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (PickUpItem != null) 
                PickUpItem.Invoke();
        }
    }

    //    private void CameraToMinimap()
    //    {
    //        if (!minimapIsActive)
    //        {
    //            Debug.Log(minimapIsActive);
    //            StartCoroutine(LerpOutMinimap());
    //
    //            Debug.Log("MiniSize " + LevelGenerator.instance.MinimapCameraOrthographicSize + " mainSize " + mainCameraSize);
    //            //            if (Math.Abs(main.orthographicSize - LevelGenerator.instance.MinimapCameraOrthographicSize) < 0.5f)
    //            //            {
    //            //                Debug.Log("anim ended");
    //            //                mapAnim = false;
    //            //                minimapIsActive = true;
    //            //            }
    //        }
    //        else
    //        {
    //            Debug.Log(minimapIsActive);
    //            main.transform.position = Vector3.Lerp(LevelGenerator.instance.MinimapCameraPosition, Vector3.zero, Time.deltaTime * 4);
    //            main.orthographicSize = Mathf.Lerp(LevelGenerator.instance.MinimapCameraOrthographicSize, 3, Time.deltaTime * 4);
    //
    //            Debug.Log("MiniSize " + LevelGenerator.instance.MinimapCameraOrthographicSize + " mainSize " + mainCameraSize);
    //
    //            if (Math.Abs(main.orthographicSize - mainCameraSize) < 0.1f)
    //            {
    //                mapAnim = false;
    //                minimapIsActive = false;
    //            }
    //        }
    //
    //    }

    public bool Movement()
    {
        if (minimapController.minimapIsActive)
            return false;

        if (InventoryUI.activeSelf)
        {
            GetComponent<Inventory>().DoIfActive();
            return false;
        }

        CreatingLight();

        foreach (var ControlScheme in GameManager.instance.ControlSchemes)
        {
            if (Move(ControlScheme.Left, Vector3.left)) return true;
            if (Move(ControlScheme.Right, Vector3.right)) return true;
            if (Move(ControlScheme.Up, Vector3.up)) return true;
            if (Move(ControlScheme.Down, Vector3.down)) return true;
        }
        return false;
    }

    private bool Move(KeyCode kc, Vector3 dir)
    {
        if (Input.GetKeyDown(kc))
        {
            if (!CanMove(dir) || isMoving)
                return false;
            isMoving = true;
            StartCoroutine(MoveAnim(dir));
            return true;
        }
        return false;
    }

    private bool CanMove(Vector3 myVector)
    {
        if (Physics2D.OverlapPoint(transform.localPosition + (myVector), layerMask))
            return false;
        return true;
    }

    public UnityEvent OnEnemyKilled; // Do dźwięków :) i do różnych innych rzeczy które chcę wywołać w prosty sposób

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hearth")
        {
            CurrentHP += 50;
            if (CurrentHP > PlayerData.MaxHP)
                CurrentHP = PlayerData.MaxHP * PlayerData.Str;
            other.gameObject.SetActive(false);
        }

        else if (other.tag == "Enemy")
        {
            other.gameObject.SetActive(false);
            OnEnemyKilled.Invoke();
            CurrentHP -= 30;
            if (CurrentHP < 0)
            {
                // Debug.Log("- 30");
            }
        }
        else if (other.tag == "Exit")
        {
            IsOnExit = true;
        }

        else if (other.tag == "Item")
        {
            var x = other.GetComponent<ItemHolder>().ItemProperties;
            PickUpItem = () =>
            {
                GetComponent<Inventory>().AddItem(x);
                other.gameObject.SetActive(false);
                PickUpItem = null;
            };
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            IsOnExit = false;
        }

        else if (other.tag == "Item")
        {
            PickUpItem = null;
        }
    }

    private void CreatingLight()
    {

        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                Collider2D[] hitColliders =
                   Physics2D.OverlapPointAll(new Vector3(transform.position.x - x,
                       transform.position.y - y,
                       0));
    
                if (hitColliders != null)
                {
                    
                    foreach (var d in hitColliders)
                    {                       
                        d.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
    }

    private IEnumerator MoveAnim(Vector3 dir)
    {
        var time = 0f;
        var startPos = transform.position;
        var endPos = transform.position += dir;

        while (time < 1)
        {
            time += Time.deltaTime * 5;
            transform.position = Vector3.Lerp(startPos, endPos, time);
            yield return new WaitForEndOfFrame();
        }
        isMoving = false;
    }
}