using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBulletPool : ObjectPool
{
    public static AOEBulletPool instance;
    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
