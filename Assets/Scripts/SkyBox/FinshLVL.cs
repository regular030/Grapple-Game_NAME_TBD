using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinshLVL : MonoBehaviour
{
    public string NewGameSceneBackToMain;
    public TextMeshProUGUI done;
    public float delay;
    public float timer;
    public bool yes = false;
    private string m;


    void Start()
    {
        //StartCoroutine(WaitForFunction());
    }
    // Update is called once per frame


    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(NewGameSceneBackToMain);
    }

}
