using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField] private CellType type;
    [SerializeField] private Vector2Int pos;
    [SerializeField] private Vector2Int targetPos;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private bool _isInteractable = false;
    private Action<Vector2Int, Vector2Int> _onSwipeEvent;
    
    public void SetType(CellType type)
    {
        this.type = type;
    }

    public void SetPos(int x, int y)
    {
        pos.x = x;
        pos.y = y;
    }

    public void SetPositionImmediately(Vector2 vec)
    {
        transform.localPosition = vec;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    
    public void SetSwipeEvent(Action<Vector2Int, Vector2Int> evnt)
    {
        _onSwipeEvent = evnt;
    }
    
    public void SetSwipeStartEvent(SwipeStartEvent start)
    {
        start.AddListener(OnStartSwipeEvent);
    }

    public void SetSwipeEndEvent(SwipeEndEvent end)
    {
        end.AddListener(OnEndSwipeEvent);
    }

    public void SetTileInteractableEvent(TileInteractableEvent evnt)
    {
        evnt.AddListener(OnTileInteractableEvent);
    }
    
    private void OnStartSwipeEvent()
    {
        _isInteractable = false;
    }

    private void OnEndSwipeEvent()
    {
        _isInteractable = true;
    }

    private void OnTileInteractableEvent(bool state)
    {
        _isInteractable = state;
    }

    [ContextMenu("Swipe")]
    private void Test_Swipe()
    {
        _onSwipeEvent.Invoke(pos, targetPos);
    }
}
