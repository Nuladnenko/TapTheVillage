using System;
using System.Linq;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }
    public void Save()
    {
        SaveSystem.SaveGame(gameManager);
    }
#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        Save();
    }
#else
    private void OnApplicationPause()
    {
        Save();
    }
#endif
}
