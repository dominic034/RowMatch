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
    
    private void Awake()
    {
        Instance = this;
        _lastUnlockedLevel = PlayerPrefs.GetInt(LastUnlockedLevelPrefKey, 1);
        _highestScore = PlayerPrefs.GetInt(HighestScorePrefKey, 0);
    }

    public void CheckScore(int score)
    {
        if (score <= _highestScore)
        {
            OnLevelCompletedEvent.Invoke(CompleteType.None);
            return;
        }

        _highestScore = score;
        _lastUnlockedLevel += 1;
        
        PlayerPrefs.SetInt(HighestScorePrefKey, _highestScore);
        PlayerPrefs.SetInt(LastUnlockedLevelPrefKey, _lastUnlockedLevel);
        OnLevelCompletedEvent.Invoke(CompleteType.NewScore);
    }
}

public class LevelCompletedEvent : UnityEvent<CompleteType>
{
    
}

public enum CompleteType
{
    None,
    NewScore,
}