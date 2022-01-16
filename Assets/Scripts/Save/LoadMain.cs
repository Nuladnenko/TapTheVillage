using UnityEngine;
using System.IO;

public class LoadMain : MonoBehaviour
{
    [SerializeField] protected int index;
    protected bool[] isActivated;
    protected void Start()
    {
        if(File.Exists(Application.persistentDataPath + "/savefile.json"))
        {
            Debug.Log("is loaded");
            Load();
            Activate();
        }
        else
            gameObject.SetActive(false);
    }

    protected void Activate()
    {
        if(isActivated[index])
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
    protected virtual void Load()
    {
        SaveData data = SaveSystem.Load(Application.persistentDataPath + "/savefile.json");
    }
}
