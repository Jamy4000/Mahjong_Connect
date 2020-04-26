using UnityEngine;

public class LevelDataUpdater : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        OnGameEnded.Listeners += GameIsDone;
    }

    private void OnDestroy()
    {
        OnGameEnded.Listeners -= GameIsDone;
    }


    private void GameIsDone(OnGameEnded info)
    {
        if (info.HasWon)
        {
            var currentLevel = GameManager.CurrentLevel;
            currentLevel.IsDone = true;
            if (_gameManager.CurrentScore > currentLevel.Score)
            {
                currentLevel.Score = _gameManager.CurrentScore;
                PlayerPrefs.SetInt(UserDataFetcher.USER_LEVEL_SCORE + currentLevel.Index, currentLevel.Score);
            }
        }
    }
}
