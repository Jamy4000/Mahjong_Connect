using UnityEngine;
using UnityEngine.EventSystems;

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
