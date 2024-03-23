using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouAreOverSomething : MonoBehaviour
{
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }
}
