using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Timmer : MonoBehaviour
{
    public float startTime;
    public TextMeshProUGUI currentTimeText;
    public bool yep;
    //private bool finnished = false;
    public TextMeshProUGUI done;

    void Start()
    {
        startTime = Time.time;
    }
    void Update () 
    {
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString(); 
        string seconds = (t % 60).ToString("f2");

        currentTimeText.text = minutes + ":" + seconds;
    }

}
