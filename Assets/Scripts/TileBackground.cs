using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileBackground : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Tile tile;
    
    public Vector2 BoundsSize
    {
        get
        {
            return spriteRenderer.bounds.size;
        }
    }

    public void SetTile(Tile tile, int x, int y)
    {
        this.tile = tile;
        this.tile.SetPos(x, y);
    }

    public void SetTile(Tile tile, Vector2Int pos)
    {
        SetTile(tile, pos.x, pos.y);
    }

    public Tile GetTile()
    {
        return tile;
    }
}
