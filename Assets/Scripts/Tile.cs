using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private CellType type;
    [SerializeField] private Vector2Int pos;
    [SerializeField] private Vector2Int targetPos;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private bool _isInteractable = false;
    [SerializeField] private bool _isCompleted = false;
    private Action<Vector2Int, Vector2Int> _onSwipeEvent;
    private Vector3 _initialMousePos = Vector3.zero;

    public bool IsCompleted
    {
        get { return _isCompleted; }
    }
    
    #region PUBLIC
    public void SetType(CellType type)
    {
        this.type = type;
    }

    public void SetColor(Color clr)
    {
        spriteRenderer.color = clr;
    }

    public void SetCompleted(bool state)
    {
        _isCompleted = state;
        _isInteractable = !state;
    }
    
    public void SetArrayIndex(int x, int y)
    {
        pos.x = x;
        pos.y = y;
    }

    public void SetPositionImmediately(Vector2 vec)
    {
        transform.localPosition = vec;
    }
    
    public void DoMove(float time)
    {
        transform.DOLocalMove(Vector3.zero, time);
    }
    
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public CellType GetTileType()
    {
        return type;
    }

    public void Reset()
    {
        _isCompleted = false;
        _isInteractable = false;
        _onSwipeEvent = null;
        type = CellType.None;
        
        PoolManager.Instance.GiveObjectToPool(gameObject);
    }
    #endregion PUBLIC

    #region ADD EVENTS
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
    
    public void SetTileInteractableEvent(TileInteractableChangedEvent evnt)
    {
        evnt.AddListener(OnTileInteractableEvent);
    }    
    #endregion ADD EVENTS

    #region EVENTS
    private void OnStartSwipeEvent()
    {
    }

    private void OnEndSwipeEvent()
    {
        targetPos = Vector2Int.zero;
        _initialMousePos = Vector3.zero;
    }

    private void OnTileInteractableEvent(bool state)
    {
        if(_isCompleted)
            return;
        
        _isInteractable = state;
    }
    #endregion EVENTS
    
    #region DRAG CONTROL
    public void OnDrag(PointerEventData eventData)
    {
        if(!_isInteractable)
            return;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!_isInteractable)
            return;
        
        targetPos = Vector2Int.zero;
        _initialMousePos = Input.mousePosition;
        // Debug.Log("BEGIN DRAG : " + _initialMousePos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!_isInteractable)
            return;
        
        targetPos = Vector2Int.zero;
        Vector3 finalMousePos = Input.mousePosition;
        SwipeDirection direction = SwipeDirection.None;
        // Debug.Log("END DRAG : " + finalMousePos);
        
        float distance =
            Vector3.Distance(ScreenToWorldPosition(finalMousePos), ScreenToWorldPosition(_initialMousePos));
        // Debug.Log("DISTANCE: " + distance);
        if(distance < transform.localScale.x * .25f)
        {
            // Debug.Log("DIRECTION : " + direction);
            return;
        }
        
        Vector3 res = finalMousePos - _initialMousePos;
        bool isVertical = Mathf.Abs(res.y) > Mathf.Abs(res.x);
        // Debug.Log("RESULT : " + res);
        // Debug.Log($"IS VERTICAL : y: {Mathf.Abs(res.y)} > x: {Mathf.Abs(res.x)} = {isVertical}");
        
        if (!isVertical)
        {
            direction = res.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            direction = res.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
        // Debug.Log("DIRECTION : " + direction);

        targetPos = pos + GetDirectionPosition(direction);
        _onSwipeEvent(pos, targetPos);
    }
    #endregion DRAG CONTROL
    
    private Vector2Int GetDirectionPosition(SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.Left:
                return Vector2Int.left; 
            case SwipeDirection.Right:
                return Vector2Int.right;
            case SwipeDirection.Up:
                return Vector2Int.up;
            case SwipeDirection.Down:
                return Vector2Int.down;
            case SwipeDirection.None:
                return Vector2Int.zero;
            default:
                return Vector2Int.zero;
        }
    }

    private Vector3 ScreenToWorldPosition(Vector3 mousePosition)
    {
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}

public enum SwipeDirection
{
    None,
    Up,
    Down,
    Left,
    Right
}

public enum MouseState
{
    None,
    Down,
    Up
}