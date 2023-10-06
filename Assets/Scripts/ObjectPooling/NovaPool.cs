using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaPool : ObjectPool
{
    public static NovaPool instance;
    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
