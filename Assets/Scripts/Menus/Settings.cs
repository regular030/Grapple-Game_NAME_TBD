using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    //public PlayerMovementGrappling pm;
    public KeyCode SwingKeyy = KeyCode.Mouse0;
    public KeyCode JumpKey = KeyCode.Space; // key to press when you want to jump
    public KeyCode SprintKey = KeyCode.LeftShift; // key to press when you want to sprint 
    public KeyCode CrouchKey = KeyCode.LeftControl; // key to press when you want to crouch

    void Update()
    {
        CrouchKey = KeyCode.LeftControl;
        SprintKey = KeyCode.LeftShift;
        JumpKey = KeyCode.Space;
        SwingKeyy = KeyCode.Mouse0;
       
    }
}
