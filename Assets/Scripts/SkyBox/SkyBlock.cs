//this is the script that happens when you die
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkyBlock : MonoBehaviour
{
    public Transform teleportTarget; // the player
    public GameObject tp; // where will you respawn
    public float death; // how many times has the player died
    public TextMeshProUGUI text_death; // the display for death counter

    void OnTriggerEnter(Collider other)
    {
        tp.transform.position = teleportTarget.transform.position; // the teleport function 
        death = death + 1; // add +1 death everytime you die
        text_death.SetText("Death Count:" +  death); // now display the new death counter 
    }
}