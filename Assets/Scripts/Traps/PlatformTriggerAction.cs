using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlatformTriggerAction: MonoBehaviour
{
    Platform platform;
    Transform playerParent;
    private void Awake()
    {
        platform = GetComponentInParent<Platform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!platform.IsGray)
        {
            platform.OnPlatform = true;

        }
        playerParent = collision.transform.parent.parent;
        collision.transform.parent.parent = transform;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!platform.IsGray)
        {
            platform.OnPlatform = false;
            
        }
        collision.transform.parent.parent = playerParent;
    }
}
