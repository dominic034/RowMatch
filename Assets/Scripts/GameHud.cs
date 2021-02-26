using TMPro;
using UnityEngine;

public class GameHud : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshPro moveCountText;
    [SerializeField] private TextMeshPro scoreText;

    private void Start()
    {
        GameManager.Instance.OnGameHudUpdateEvent.AddListener(OnGameHudUpdate);
        GameManager.Instance.OnInitializeLevelEvent.AddListener(OnInitializeLevel);
        GameManager.Instance.OnReturnMainMenu.AddListener(OnReturnMainMenu);
    }

    private void OnGameHudUpdate(int moveCount, int score)
    {
        SetMoveCount(moveCount);
        SetScore(score);
    }

    private void OnInitializeLevel(LevelData data)
    {
        panel.SetActive(true);
        SetMoveCount(data.MoveCount);
        SetScore(0);
    }

    private void OnReturnMainMenu()
    {
        panel.SetActive(false);
    }

    private void SetMoveCount(int moveCount)
    {
        moveCountText.text = $"move: {moveCount}";
    }

    private void SetScore(int score)
    {
        scoreText.text = $"score: {score}";
    }
}
