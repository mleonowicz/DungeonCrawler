using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerPlatform : MonoBehaviour
{
    private PlayerPlatform myPlayerPlatform;

    private void Start()
    {
        myPlayerPlatform = transform.parent.GetComponent<PlayerPlatform>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            coll.SendMessageUpwards("TakeDamage", myPlayerPlatform.MyPlayerStats.Damage);
        }
    }
}
