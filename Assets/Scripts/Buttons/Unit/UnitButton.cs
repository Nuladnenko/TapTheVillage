using UnityEngine;
using System.IO;

public class UnitButton : MainButton
{
    public int Level { get; protected set; }

    protected override void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(savePath))
            Load();
        base.Awake();

        button.onClick.AddListener(Click);
    }

    protected virtual void Click()
    {
        UpLevel();
        UpCost();
    }
    protected override void AssignText()
    {
        base.AssignText();
        textsTMP[3].text = "lv: " + Level.ToString();
    }
    protected void UpLevel()
    {
        Level++;
        textsTMP[3].text = "lv: " + Level.ToString();
    }

    protected virtual void Load()       //need to load data before Awake method from MainButton
    {
    }
}
