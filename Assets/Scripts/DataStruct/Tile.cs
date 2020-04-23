using UnityEngine;

public class Tile
{
    public UnityEngine.Sprite Icon;
    public string ID;

    public bool IsEmpty = false;
    public UnityEngine.Vector2 Coordinates;

    public GameObject GameObjectRepresentation;

    public Tile(string iconName, UnityEngine.Sprite icon) 
    {
        ID = iconName;
        Icon = icon;
    }

    public Tile()
    {
        IsEmpty = true;
    }
}
