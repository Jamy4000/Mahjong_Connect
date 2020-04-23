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
    public void LoadLevel(int index) 
    {
        foreach (var level in _levels) 
        {
            if (index == level.Index) 
            {
                GameManager.CurrentLevel = level;
                break;
            }
        }

        if (GameManager.CurrentLevel == null)
        {
            Debug.LogErrorFormat("Couldn't find level with index {0}. Be sure it was added to the LevelLoader list.", index);
            return;
        }

        SceneManager.LoadSceneAsync(_gameSceneName);
    }

    public Level GetLevel(int index)
    {
        foreach (var level in _levels)
        {
            if (index == level.Index)
                return level;
        }

        Debug.LogErrorFormat("Couldn't find level with index {0}. Be sure it was added to the LevelLoader list.", index);
        return null;
    }
}
