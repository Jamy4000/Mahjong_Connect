using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    [SerializeField] private string _mainMenuName = "MainMenu";
    [SerializeField] private Animator _validationPanel;

    public void MenuButtonCallback()
    {
        _validationPanel.SetTrigger("ShowPanel");
    }

    public void OnUserValidate()
    {
        SceneManager.LoadSceneAsync(_mainMenuName);
    }

    public void OnUserCancel()
    {
        _validationPanel.SetTrigger("HidePanel");
    }
}
