using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootController : MonoBehaviour
{
    // Start is called before the first frame update
    public event Action OnFire;

    public void Fire()
    {
        OnFire?.Invoke();
    }
}
