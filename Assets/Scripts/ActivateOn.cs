using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateOn : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().enabled = true;
    }
}
