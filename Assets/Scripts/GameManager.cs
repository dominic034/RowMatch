using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private const string HighestScorePrefKey = "HighestScore";
    
    private int _highestScore;
    
    public LevelCompletedEvent OnLevelCompletedEvent { get; private set; } = new LevelCompletedEvent();
    public UnityEvent OnOpenLevelsPopUp { get; private set; } = new UnityEvent();
    public PlayLevelButtonEvent OnPlayLevelButtonEvent { get; private set; } = new PlayLevelButtonEvent();
    public InitializeLevelEvent OnInitializeLevelEvent { get; private set; } = new InitializeLevelEvent();
    public LevelResultEvent OnLevelResultEvent { get; private set; } = new LevelResultEvent();
    
    private void Awake()
    {
        Instance = this;
        _highestScore = PlayerPrefs.GetInt(HighestScorePrefKey, 0);
        
        OnLevelCompletedEvent.AddListener(OnLevelCompleted);
        OnLevelResultEvent.AddListener(OnLevelResult);
    }

    private void OnDestroy()
    {
        OnLevelCompletedEvent.RemoveAllListeners();
        OnLevelCompletedEvent.RemoveAllListeners();
    }

    private void OnLevelResult(int score, int levelNo)
    {
        if (score <= _highestScore)
        {
            OnLevelCompletedEvent.Invoke(CompleteType.None);
            return;
        }

        _highestScore = score;
        
        PlayerPrefs.SetInt(HighestScorePrefKey, _highestScore);
        OnLevelCompletedEvent.Invoke(CompleteType.NewScore);
    }

    private void OnLevelCompleted(CompleteType type)
    {
        if(type == CompleteType.None)
            return;
        
        OnOpenLevelsPopUp.Invoke();
    }
}

public class LevelCompletedEvent : UnityEvent<CompleteType>
{
    
}

public class LevelResultEvent : UnityEvent<int, int>
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