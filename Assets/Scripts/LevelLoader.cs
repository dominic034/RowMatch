using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private readonly LevelData _blankLevelData = new LevelData(-1, -1, -1, -1, new CellType[0,0]);
    private static LevelLoader _instance;
    
    private const string LevelPath = "Levels/RM_A{0}";
    private List<LevelData> _levels = new List<LevelData>();

    public static LevelLoader Instance
    {
        get
        {
            return _instance;
        }
    }
    
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        LoadFirst10Level();
    }

    public LevelData GetLevelAtIndex(int index)
    {
        if (index < 0 || index >= _levels.Count)
            return _blankLevelData;

        return _levels[index];
    }
    
    private void LoadFirst10Level()
    {
        string path = "";
        for (int i = 1; i <= 10; i++)
        {
            path = Path.Combine(Application.streamingAssetsPath, string.Format(LevelPath, i));
            string[] lines = File.ReadAllLines(path);
            ReadAllLines(lines);
        }    
    }

    private void ReadAllLines(string[] lines)
    {
        int level = 0;
        int width = 0;
        int height = 0;
        int move = 0;
        CellType[,] grid = new CellType[0,0];

        foreach (var line in lines)
        {
            string[] splitted = line.Trim().Split(':');
            switch (splitted[0])
            {
                case "level_number":
                    level = Convert.ToInt32(splitted[1].Trim());
                    break;
                case "grid_width":
                    width = Convert.ToInt32(splitted[1].Trim());
                    break;
                case "grid_height":
                    height = Convert.ToInt32(splitted[1].Trim());
                    break;
                case "move_count":
                    move = Convert.ToInt32(splitted[1].Trim());
                    break;
                case "grid":
                    string[] cells = splitted[1].Trim().Split(',');
                    grid = GetGrid(width, height, cells);
                    break;
            }
        }

        LevelData levelData = new LevelData(level, width, height, move, grid); 
        Debug.Log(levelData);
        _levels.Add(levelData);
    }

    private CellType[,] GetGrid(int width, int height, string[] cells)
    {
        CellType[,] grid = new CellType[width, height];
        int row = 0;
        int column = 0;
        foreach(var cell in cells)
        {
            grid[column, row] = StringToCellType(cell);
            column++;

            if (column == width)
            {
                column = 0;
                row++;
            }
        }
        return grid;
    }

    private CellType StringToCellType(string cell)
    {
        switch (cell)
        {
            case "r":
                return CellType.Red;
            case "g":
                return CellType.Green;
            case "b":
                return CellType.Blue;
            case "y":
                return CellType.Yellow;
            default:
                return CellType.None;
        }
    }
}

public struct LevelData
{
    public int LevelNumber { get; private set; }
    public int Width { get; private set; } 
    public int Height { get; private set; }
    public int MoveCount { get; private set; }
    public CellType[,] Grid { get; private set; }

    public LevelData(int levelNumber, int width, int height, int moveCount, CellType[,] grid)
    {
        LevelNumber = levelNumber;
        Width = width;
        Height = height;
        MoveCount = moveCount;
        Grid = grid;
    }

    public override string ToString()
    {
        string str = $"level: {LevelNumber}\n";
        str += $"width: {Width}, height: {Height}, move: {MoveCount}\n";
        str += $"Grid.x: {Grid.GetLength(0)}, Grid.y: {Grid.GetLength(1)}\n";
        for (int r = 0; r < Grid.GetLength(1); r++)
        {
            for (int c = 0; c < Grid.GetLength(0); c++)
            {
                str += Grid[c, r] + " ";
            }

            str += "\n";
        }
        
        return str;
    }
}

public enum CellType
{
    None = -1,
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3,
}
