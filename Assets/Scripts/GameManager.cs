using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _lastUnlockedLevel;
    private int _highestScore;
    
    private void Awake()
    {
        _lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);
        _highestScore = PlayerPrefs.GetInt("HighestScore", 0);
    }
}
