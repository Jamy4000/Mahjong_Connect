using UnityEngine;

[CreateAssetMenu()]
public class Level : ScriptableObject
{
    public string Name;
    public string LayoutFileName;
    public bool IsDone = false;
    public int Score = 0;
}