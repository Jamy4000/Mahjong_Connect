using UnityEngine;

[CreateAssetMenu()]
public class Level : ScriptableObject
{
    public int Index;
    public string Name;
    public string LayoutFileName;

    public bool IsDone = false;
    public int Score = 0;

    // x
    public int Height;
    
    // y
    public int Length;
}