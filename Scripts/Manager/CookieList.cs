﻿using System.Collections.Generic;
using UnityEngine;

public class CookieList : MonoBehaviour
{
    public List<GameObject> cookie;
    public Material[] bodyColors;
    
    private void Start()
    {
        cookie.Add(GameObject.FindGameObjectWithTag("Player"));
    }
}
