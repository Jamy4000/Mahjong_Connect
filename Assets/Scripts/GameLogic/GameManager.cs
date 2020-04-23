using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Level CurrentLevel;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("A game manager already exist. Destroying this one.");
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
