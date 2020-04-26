using UnityEngine;
using UnityEngine.UI;

public class LevelButtonInstantiater : MonoBehaviour
{
    [SerializeField] private GameObject _buttonsLine;

    [SerializeField] private LevelsHolder _levelHolder;
    
    [SerializeField] private GameObject _levelButton;

    private void Start()
    {
        InstantiateLevelButtons();
    }

    private void InstantiateLevelButtons()
    {
        Transform currentLine = null;

        for (int i = 0; i < _levelHolder.Levels.Length; i++)
        {
            if (i % 2 == 0)
                currentLine = Instantiate(_buttonsLine, transform).transform;

            Instantiate(_levelButton, currentLine).GetComponent<LevelButtonHandler>().Init(_levelHolder.Levels[i]);
        }
    }
}
