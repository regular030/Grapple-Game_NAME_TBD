using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class PauseMenu : MonoBehaviour {
    public string Main;
    // Changed to GameObject because only the game object of the menu needs to be accessed, you can 
    // change this to any class that inherits MonoBehaviour

    public GameObject GUI;
    public GameObject Pause;
    public string NewGameScene;
    private bool paused = false; 
    


    void Start ()
    {
        GUI.SetActive(true);
        Pause.SetActive(false);
        paused = false;
    }
    // Update is called once per frame
    void Update () {
        // Reverse the active state every time escape is pressed
        
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            //set pause menu to active
            if (Pause.activeSelf == false)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;

                paused = true;
                GUI.SetActive(false);
                Pause.SetActive(true);

                GetComponent<PlayerMovementGrappling>().enabled = false;
                GetComponent<PlayerCam>().enabled = false;
                GetComponent<SwingingDone>().enabled = false;
                GetComponent<Grappling>().enabled = false;

            }

            //set GUI to active
            else
            {
                Cursor.lockState = CursorLockMode.Locked;

                paused = false;
                Pause.SetActive(false);
                GUI.SetActive(true);
                Cursor.visible = false;

                GetComponent<PlayerMovementGrappling>().enabled = true;
                GetComponent<PlayerCam>().enabled = true;
                GetComponent<SwingingDone>().enabled = true;
                GetComponent<Grappling>().enabled = true;
            }
        }
    }

}