using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuElement;
    
    public void OpenClose()
    {
        if(menuElement.activeSelf==false)
            menuElement.SetActive(true);
        else
            menuElement.SetActive(false);
    }
}
