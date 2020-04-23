using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    [SerializeField] private string _mainMenuName = "MainMenu";

    public void LoadMainMenu() 
    {
        SceneManager.LoadSceneAsync(_mainMenuName);
    }
}
