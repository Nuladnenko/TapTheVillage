using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenu : MonoBehaviour
{
    void OnGUI()
    {
        if (Application.platform == RuntimePlatform.Android)
            if (Input.GetKeyUp(KeyCode.Escape))
                gameObject.SetActive(false); 
    }
}
