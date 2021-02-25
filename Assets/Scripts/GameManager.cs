using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private const string HighestScorePrefKey = "HighestScore";
    private const string LastUnlockedLevelPrefKey = "LastUnlockedLevel";
    
    private int _lastUnlockedLevel;
    private int _highestScore;
    
    public LevelCompletedEvent OnLevelCompletedEvent { get; private set; } = new LevelCompletedEvent();
    public UnityEvent OnOpenLevelsPopUp { get; private set; } = new UnityEvent();
    
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
            OnLevelCompletedEvent.Invoke(CompleteType.None, score);
            return;
        }

        _highestScore = score;
        _lastUnlockedLevel += 1;
        
        PlayerPrefs.SetInt(HighestScorePrefKey, _highestScore);
        PlayerPrefs.SetInt(LastUnlockedLevelPrefKey, _lastUnlockedLevel);
        OnLevelCompletedEvent.Invoke(CompleteType.NewScore, score);
    }

    private void OnLevelCompleted(CompleteType type, int score)
    {
        if(type == CompleteType.None)
            return;
        
        OnOpenLevelsPopUp.Invoke();
    }
}

public class LevelCompletedEvent : UnityEvent<CompleteType, int>
{
    
}

public enum CompleteType
{
    None,
    NewScore,
}