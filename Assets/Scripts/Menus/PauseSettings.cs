using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSettings : MonoBehaviour
{
    public PlayerMovementGrappling pm;
    public PauseMenu Pause;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSettings()
    {
        Pause.GUI.SetActive(false);
        Pause.Pause.SetActive(false);
        Pause.Settings.SetActive(true);
    }

    public void BackToLast()
    {
        Pause.GUI.SetActive(false);
        Pause.Pause.SetActive(true);
        Pause.Settings.SetActive(false);
    }
}
