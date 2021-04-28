﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementBadge : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  OnMouseOver() {
       
       gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.15f, 1.15f);
    }

    public void  OnMouseExit() {
       gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
    }
}