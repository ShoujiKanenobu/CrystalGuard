using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEPool : ObjectPool
{
    public static AOEPool instance;
    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
}
