using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Provide some generals events when a tile is hovered.
/// To enable debug mode, simply turn on the "DebugHelper" panel in the MainGame Canvas
/// </summary>
public class TileHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Tile _thisTile;
    public bool _isBeingHovered = true;

    private void Awake()
    {
        OnUserValideAnswer.Listeners += OnTileDisappear;
    }

    private void OnDestroy()
    {
        OnUserValideAnswer.Listeners -= OnTileDisappear;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_thisTile == null)
            _thisTile = GetComponent<TileClickHandler>().ThisTile;

        _isBeingHovered = true;
        new OnTileHovered(_thisTile);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isBeingHovered = false;
        new OnTileUnhovered();
    }

    private void OnTileDisappear(OnUserValideAnswer info) 
    {
        if (info.FirstTile == _thisTile || info.SecondTile == _thisTile)
        {
            if (_isBeingHovered)
                OnPointerExit(null);
            Destroy(this);
        }
    }
}
