using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load the main menu when the user request it
/// </summary>
public class MainMenuLoader : MonoBehaviour
{
    [SerializeField] private string _mainMenuName = "MainMenu";
    [SerializeField] private Animator _validationPanel;

    /// <summary>
    /// Callback for the "Menu" button, open a validation panel
    /// </summary>
    public void MenuButtonCallback()
    {
        _validationPanel.SetTrigger("ShowPanel");
    }

    /// <summary>
    /// When the user is validating its choice in the validation panel
    /// </summary>
    public void OnUserValidate()
    {
        SceneManager.LoadSceneAsync(_mainMenuName);
    }

    /// <summary>
    /// When the user want ot come back to the game
    /// </summary>
    public void OnUserCancel()
    {
        _validationPanel.SetTrigger("HidePanel");
    }
}
