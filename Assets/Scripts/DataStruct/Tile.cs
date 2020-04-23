public class Tile
{
    public UnityEngine.Sprite Icon;
    public string ID;

    public bool IsSelected = false;

    public Tile(string iconName, UnityEngine.Sprite icon) 
    {
        ID = iconName;
        Icon = icon;
    }
}
