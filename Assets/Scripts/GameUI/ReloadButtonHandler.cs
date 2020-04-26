using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadButtonHandler : MonoBehaviour
{
    [SerializeField] private Animator _validationPanel;

    public void RestartButtonCallback()
    {
        _validationPanel.SetTrigger("ShowPanel");
    }

    public void OnUserValidate() 
    {
        SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
    }

    public void OnUserCancel()
    {
        _validationPanel.SetTrigger("HidePanel");
    }
}
