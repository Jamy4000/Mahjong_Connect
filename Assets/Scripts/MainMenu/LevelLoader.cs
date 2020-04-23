using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Level[] _levels = new Level[5];
    [SerializeField] private string _gameSceneName = "MainGame";

    /// <summary>
    /// Method called by the MainMenu buttons to set the level to load and load the game scene
    /// </summary>
    /// <param name="name"></param>
    public void LoadLevel(string name) 
    {
        foreach (var level in _levels) 
        {
            if (string.Equals(name, level.Name)) 
            {
                GameManager.CurrentLevel = level;
                break;
            }
        }

        if (GameManager.CurrentLevel == null)
        {
            Debug.LogError("Couldn't find level with name {0}. Be sure it was added to the LevelLoader list.", gameObject);
            return;
        }

        SceneManager.LoadSceneAsync(_gameSceneName);
    }
}
