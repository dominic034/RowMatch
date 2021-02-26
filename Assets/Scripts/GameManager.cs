using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private const string HighestScorePrefKey = "HighestScore";
    
    [SerializeField] private int _highestScore;
    
    public LevelCompletedEvent OnLevelCompletedEvent { get; private set; } = new LevelCompletedEvent();
    public UnityEvent OnOpenLevelsPopUp { get; private set; } = new UnityEvent();
    public PlayLevelButtonEvent OnPlayLevelButtonEvent { get; private set; } = new PlayLevelButtonEvent();
    public InitializeLevelEvent OnInitializeLevelEvent { get; private set; } = new InitializeLevelEvent();
    public LevelResultEvent OnLevelResultEvent { get; private set; } = new LevelResultEvent();
    public UnityEvent OnReturnMainMenu { get; private set; } = new UnityEvent();
    public GameHudUpdateEvent OnGameHudUpdateEvent { get; private set; } = new GameHudUpdateEvent();
    public UnityEvent OnLevelsAreLoaded { get; private set; } = new UnityEvent();
    
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
        OnOpenLevelsPopUp.RemoveAllListeners();
        OnPlayLevelButtonEvent.RemoveAllListeners();
        OnInitializeLevelEvent.RemoveAllListeners();
        OnLevelResultEvent.RemoveAllListeners();
        OnReturnMainMenu.RemoveAllListeners();
        OnGameHudUpdateEvent.RemoveAllListeners();
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
        {
            OnReturnMainMenu.Invoke();
            return;
        }
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

public class GameHudUpdateEvent : UnityEvent<int, int>
{
    
}

public enum CompleteType
{
    None,
    NewScore,
}