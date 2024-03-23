using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class PauseMenu : MonoBehaviour {
    public string Main;
    // Changed to GameObject because only the game object of the menu needs to be accessed, you can 
    // change this to any class that inherits MonoBehaviour
    public GameObject optionsMenu;
    public string NewGameScene;
    
    // Update is called once per frame
    void Update () {
        // Reverse the active state every time escape is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(Main);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(Main);
        }
    }

}