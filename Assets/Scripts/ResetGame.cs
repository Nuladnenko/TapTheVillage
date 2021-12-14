using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ResGame);
    }
    private void ResGame()
    {
        File.Delete(Application.persistentDataPath + "/savefile.json");

        SceneManager.LoadScene(0);
    }
}
