using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : ObjectPool
{
    public static BulletPool instance;
    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
