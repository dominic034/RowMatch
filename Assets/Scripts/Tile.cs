using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField] private CellType type;
    [SerializeField] private Vector2Int pos;
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

    [ContextMenu("Test")]
    private void Test()
    {
        _onSwipeEvent.Invoke(Vector2Int.up, Vector2Int.zero);
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
        evnt.AddListener(SetTileInteractable);
    }

    public void SetPositionImmediately(Vector2 vec)
    {
        transform.localPosition = vec;
    }

    private void OnStartSwipeEvent()
    {
        _isInteractable = false;
    }

    private void OnEndSwipeEvent()
    {
        _isInteractable = true;
    }

    private void SetTileInteractable(bool state)
    {
        _isInteractable = state;
    }
}