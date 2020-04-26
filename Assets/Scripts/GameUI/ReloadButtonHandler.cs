using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// restart the level when the user request it
/// </summary>
public class ReloadButtonHandler : MonoBehaviour
{
    [SerializeField] private Animator _validationPanel;

    /// <summary>
    /// Callback for the "Restart" button, open a validation panel
    /// </summary>
    public void RestartButtonCallback()
    {
        _validationPanel.SetTrigger("ShowPanel");
    }

    /// <summary>
    /// When the user is validating its choice in the validation panel
    /// </summary>
    public void OnUserValidate() 
    {
        SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
    }

    /// <summary>
    /// When the user want ot come back to the game
    /// </summary>
    public void OnUserCancel()
    {
        _validationPanel.SetTrigger("HidePanel");
    }
}
