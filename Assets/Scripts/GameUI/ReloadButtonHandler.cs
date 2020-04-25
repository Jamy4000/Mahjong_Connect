using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadButtonHandler : MonoBehaviour
{
    [SerializeField] private Animator _validationPanel;

    public void RestartButtonCallback()
    {
        Debug.Log("TODO ANIMATION");
        _validationPanel.gameObject.SetActive(true);
        _validationPanel.SetTrigger("ShowPanel");
    }

    public void OnUserValidate() 
    {
        SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
    }

    public void OnUserCancel()
    {
        Debug.Log("TODO ANIMATION");
        _validationPanel.gameObject.SetActive(false);
        //_validationPanel.SetTrigger("HidePanel");
    }
}
