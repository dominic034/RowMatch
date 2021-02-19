using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
    private static BoardManager _instance;
    public static BoardManager Instance
    {
        get { return _instance; }
    }
    
    [SerializeField] private TileBackground tileBackgroundPrefab;
    [SerializeField] private Tile tilePrefab;

    private SwipeStartEvent OnSwipeStartEvent { get; set; } = new SwipeStartEvent();
    private SwipeEndEvent OnSwipeEndEvent { get; set; } = new SwipeEndEvent();
    private TileInteractableEvent OnTileInteractableEvent { get; set; } = new TileInteractableEvent();

    private LevelData _currentLevel;
    private TileBackground[,] _allTileBackgrounds;

    [SerializeField] private TileBackground currentTileBackground;
    [SerializeField] private TileBackground targetTileBackground;
    [SerializeField] private Tile currentTile;
    [SerializeField] private Tile targetTile;
    
    private void Awake()
    {
        _instance = this;
    }

    private void SwipeTiles(Vector2Int current, Vector2Int target)
    {
        Debug.Log("Start SwipeTiles");
        OnSwipeStartEvent.Invoke();
        
        currentTileBackground = GetTileBackground(current);
        targetTileBackground = GetTileBackground(target);
        
        currentTile = currentTileBackground.GetTile();
        targetTile = targetTileBackground.GetTile();

        currentTileBackground.SetTile(targetTile, target);
        targetTileBackground.SetTile(currentTile, current);
        
        currentTile.SetParent(targetTileBackground.transform);
        targetTile.SetParent(currentTileBackground.transform);
        
        currentTile.SetPositionImmediately(Vector2.zero);
        targetTile.SetPositionImmediately(Vector2.zero);

        OnSwipeEndEvent.Invoke();
        Debug.Log("End SwipeTiles");
    }
    
    private void CreateBoard(int width, int height)
    {
        _allTileBackgrounds = new TileBackground[width, height];
        var offset = tileBackgroundPrefab.BoundsSize;
        var startPos = transform.position;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++ )
            {
                Vector2 pos = new Vector2(startPos.x + (offset.x * x), startPos.y + (offset.y * y));
                TileBackground instantiated = Instantiate(tileBackgroundPrefab, pos, tileBackgroundPrefab.transform.rotation, transform);
                Tile tile = InstantiateTile(_currentLevel.Grid[x, y], instantiated.transform);
                instantiated.SetTile(tile, x, y);
                _allTileBackgrounds[x, y] = instantiated;
            }
        }
        
        UpdateBoardPosition();
    }

    private void UpdateBoardPosition()
    {
        Vector2 current = transform.position;
        Vector2 size = tileBackgroundPrefab.BoundsSize;
        float x = _currentLevel.Width * .5f * size.x - size.x * .5f;
        float y = _currentLevel.Height * .5f * size.y - size.y * .5f;
        
        transform.position = new Vector3(current.x - x, current.y - y, 0);
    }

    private TileBackground GetTileBackground(Vector2Int pos)
    {
        if (pos.x >= _allTileBackgrounds.GetLength(0) || pos.y > _allTileBackgrounds.GetLength(1))
            return null;

        return _allTileBackgrounds[pos.x, pos.y];
    }

    private Tile InstantiateTile(CellType type, Transform parent)
    {
        Tile instantiated = Instantiate(tilePrefab, new Vector2(0,0), Quaternion.identity, parent); 
        instantiated.SetType(type);
        instantiated.SetPositionImmediately(new Vector2(0, 0));
        instantiated.SetSwipeStartEvent(OnSwipeStartEvent);
        instantiated.SetSwipeEndEvent(OnSwipeEndEvent);
        instantiated.SetTileInteractableEvent(OnTileInteractableEvent);
        instantiated.SetSwipeEvent(SwipeTiles);
        return instantiated;
    }
    
    [ContextMenu("StartFirstLevel")]
    private void StartFirstLevel()
    {
        _currentLevel = LevelLoader.Instance.GetLevelAtIndex(0);
        if(_currentLevel.LevelNumber <= 0)
            return;
        
        CreateBoard(_currentLevel.Width, _currentLevel.Height);
    }
}

public class SwipeStartEvent : UnityEvent
{
    
}

public class SwipeEndEvent : UnityEvent
{
    
}

public class TileInteractableEvent : UnityEvent<bool>
{
    
}