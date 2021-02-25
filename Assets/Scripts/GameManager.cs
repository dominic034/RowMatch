using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private const string HighestScorePrefKey = "HighestScore";
    private const string LastUnlockedLevelPrefKey = "LastUnlockedLevel";
    
    private int _lastUnlockedLevel;
    private int _highestScore;
    private int _currentLevel;
    
    public LevelCompletedEvent OnLevelCompletedEvent { get; private set; } = new LevelCompletedEvent();
    public UnityEvent OnOpenLevelsPopUp { get; private set; } = new UnityEvent();
    public PlayLevelButtonEvent OnPlayLevelButtonEvent { get; private set; } = new PlayLevelButtonEvent();
    public InitializeLevelEvent OnInitializeLevelEvent { get; private set; } = new InitializeLevelEvent();
    
    private void Awake()
    {
        Instance = this;
        _lastUnlockedLevel = PlayerPrefs.GetInt(LastUnlockedLevelPrefKey, 1);
        _highestScore = PlayerPrefs.GetInt(HighestScorePrefKey, 0);
        
        OnLevelCompletedEvent.AddListener(OnLevelCompleted);
    }

    private void OnDestroy()
    {
        OnLevelCompletedEvent.RemoveAllListeners();
        OnLevelCompletedEvent.RemoveAllListeners();
    }

    public void CheckScore(int score)
    {
        if (score <= _highestScore)
        {
            OnLevelCompletedEvent.Invoke(CompleteType.None, score, _currentLevel);
            return;
        }

        _highestScore = score;
        _lastUnlockedLevel += 1;
        
        PlayerPrefs.SetInt(HighestScorePrefKey, _highestScore);
        PlayerPrefs.SetInt(LastUnlockedLevelPrefKey, _lastUnlockedLevel);
        OnLevelCompletedEvent.Invoke(CompleteType.NewScore, score, _currentLevel);
    }

    private void OnLevelCompleted(CompleteType type, int score, int currentLevel)
    {
        if(type == CompleteType.None)
            return;
        
        OnOpenLevelsPopUp.Invoke();
    }
}

public class LevelCompletedEvent : UnityEvent<CompleteType, int, int>
{
    
}

public class InitializeLevelEvent : UnityEvent<LevelData>
{
    
}

public class PlayLevelButtonEvent : UnityEvent<int>
{
    
}

public enum CompleteType
{
    None,
    NewScore,
}