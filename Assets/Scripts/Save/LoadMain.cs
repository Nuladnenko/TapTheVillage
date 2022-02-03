using UnityEngine;
using System.IO;

public class LoadMain : MonoBehaviour
{
    [SerializeField] protected int index;
    protected bool[] isBought;
    protected void Start()
    {
        if(File.Exists(Application.persistentDataPath + "/savefile.json"))
        {
            Load();
            Activate();
        }
        else
            gameObject.SetActive(false);
    }

    protected void Activate()
    {
        if(isBought.Length!=0 && isBought.Length>index && isBought[index])      //we dont check isBought == null, because Activate is called after Load
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
    protected virtual void Load()
    {
        SaveData data = SaveSystem.Load(Application.persistentDataPath + "/savefile.json");
    }
}
