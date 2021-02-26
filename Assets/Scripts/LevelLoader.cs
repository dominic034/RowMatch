using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LevelLoader : MonoBehaviour
{
    private readonly LevelData _blankLevelData = new LevelData(-1, -1, -1, -1, new CellType[0,0]);

    private const string OfflineLevelsPath = "Levels";
    private const string DownloadedLevelsPath = "DownloadedLevels";
    private const string LevelUrlsPath = "LevelUrls";
    private const string LevelPrefKey = "Level_{0}";
    private const string LevelDataPrefValue = "{0}:{1}";
    private const string DefaultLevelData = "false:0";
    private const string FirstLunchPrefKey = "HasPlay";
    private const string LevelUrlHead = "https://row-match.s3.amazonaws.com/levels/";
    
    private List<LevelData> _levels = new List<LevelData>();
    private bool _hasPlayed;
    
    public static LevelLoader Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _hasPlayed = Convert.ToBoolean(PlayerPrefs.GetInt(FirstLunchPrefKey, 0));
        
        if(!_hasPlayed)
            OnFirstLunch();
        else
        {
            LoadOfflineLevels();
            LoadDownloadedLevels();
        }
    }

    private void Start()
    {
        GameManager.Instance.OnLevelResultEvent.AddListener(OnLevelResult);
        GameManager.Instance.OnPlayLevelButtonEvent.AddListener(OnClickedPlayLevel);
    }

    private void OnFirstLunch()
    {
        _hasPlayed = true;
        PlayerPrefs.SetInt(FirstLunchPrefKey, 1);

        string path = Path.Combine(Application.streamingAssetsPath, DownloadedLevelsPath);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        LoadOfflineLevels();
        
        _levels[0].ChangeLockStatus(true);
        StartCoroutine(DownloadAllLevels());
    }
    
    private void OnLevelResult(int score, int currentLevel)
    {
        _levels[currentLevel].ChangeLockStatus(true);
        _levels[currentLevel -1].SetHighScore(score);
        
        PlayerPrefs.SetString(string.Format(LevelPrefKey, currentLevel), string.Format(LevelDataPrefValue, true.ToString(), score.ToString()));
        PlayerPrefs.SetString(string.Format(LevelPrefKey, currentLevel+1), string.Format(LevelDataPrefValue, true.ToString(), "0"));
    }

    private void OnClickedPlayLevel(int no)
    {
        GameManager.Instance.OnInitializeLevelEvent.Invoke(GetLevelAtIndex(no - 1));
    }
    
    public LevelData GetLevelAtIndex(int index)
    {
        if (index < 0 || index >= _levels.Count)
            return _blankLevelData;

        return _levels[index];
    }

    public int GetLevelsCount()
    {
        return _levels.Count;
    }
    
    private void LoadOfflineLevels()
    {
        string path = Path.Combine(Application.streamingAssetsPath, OfflineLevelsPath);
        string[] offlineLevels = Directory.GetFiles(path);

        foreach (var level in offlineLevels)
        {
            if(level.Contains(".meta"))
                continue;
            
            string[] lines = File.ReadAllLines(level);
            ReadAllLines(lines);
        }
        _levels.Sort();
    }

    private void LoadDownloadedLevels()
    {
        string path = Path.Combine(Application.streamingAssetsPath, DownloadedLevelsPath);
        string[] levels = Directory.GetFiles(path);

        foreach (var level in levels)
        {
            if(level.Contains(".meta"))
                continue;

            string[] lines = File.ReadAllLines(level);
            ReadAllLines(lines);
        }
    }

    private IEnumerator DownloadAllLevels()
    {
        string path = Path.Combine(Application.streamingAssetsPath, LevelUrlsPath);
        string[] allLevels = File.ReadAllLines(path);
        foreach (var level in allLevels)
        {
            if(level.Contains(".meta"))
                continue;
            
            yield return DownloadLevel(level);
        }
    }
    
    private IEnumerator DownloadLevel(string url)
    {
        string nm = url.Replace(LevelUrlHead, String.Empty);
        UnityWebRequest request = new UnityWebRequest(url) {downloadHandler = new DownloadHandlerBuffer()};
        yield return request.SendWebRequest();
        
        if (!request.isDone)
        {
            Debug.Log("error");
            request.Dispose();
            yield break;
        }
        
        string filePath = Path.Combine(Application.streamingAssetsPath, DownloadedLevelsPath, nm);
        if (!File.Exists(filePath))
            File.Create(filePath).Close();

        File.WriteAllText(filePath, request.downloadHandler.text);
        
        StringReader reader = new StringReader(request.downloadHandler.text);
        List<string> lines = new List<string>();
        string line = "";
        while ((line = reader.ReadLine()) != null)
            lines.Add(line);

        ReadAllLines(lines.ToArray());
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

        string prefData = PlayerPrefs.GetString(string.Format(LevelPrefKey, level), DefaultLevelData);
        string[] vals = prefData.Split(':');

        LevelData levelData = new LevelData(level, width, height, move, grid, Convert.ToBoolean(vals[0]), Convert.ToInt32(vals[1])); 
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

public class LevelData : IComparable<LevelData>
{
    public int LevelNumber { get; private set; }
    public int Width { get; private set; } 
    public int Height { get; private set; }
    public int MoveCount { get; private set; }
    public CellType[,] Grid { get; private set; }
    public bool IsLocked { get; private set; }
    public int HighScore { get; private set; }

    public LevelData(int levelNumber, int width, int height, int moveCount, CellType[,] grid, bool isLocked = false, int highScore = 0)
    {
        LevelNumber = levelNumber;
        Width = width;
        Height = height;
        MoveCount = moveCount;
        Grid = grid;
        IsLocked = isLocked;
        HighScore = highScore;
    }

    public void ChangeLockStatus(bool state)
    {
        // Debug.Log($"{LevelNumber} staus changed");
        IsLocked = state;
    }

    public void SetHighScore(int highScore)
    {
        HighScore = highScore;
    }

    public int CompareTo(LevelData other)
    {
        if (other == null)
            return 1;

        return LevelNumber.CompareTo(other.LevelNumber);
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
