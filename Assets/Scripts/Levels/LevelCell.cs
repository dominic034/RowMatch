using TMPro;
using UI;
using UnityEngine;

namespace Levels
{
    public class LevelCell : ScrollerCell
    {
        [SerializeField] private TextMeshPro headerText;
        [SerializeField] private TextMeshPro highScore;
        [SerializeField] private SpriteButton playButton;

        private int _levelNumber;
        private void Awake()
        {
            playButton.OnClick.AddListener(OnClickedPlayButton);
        }

        private void OnDestroy()
        {
            playButton.OnClick.RemoveAllListeners();
        }

        public void SetView(LevelData data)
        {
            headerText.text = $"Level {data.LevelNumber} - Move {data.MoveCount}";
            highScore.text = data.HighScore == 0 ? "No Score" : $"High Score: {data.HighScore}";
            playButton.Interactable = data.IsLocked;
            _levelNumber = data.HighScore;
        }

        public void OnClickedPlayButton()
        {
            GameManager.Instance.OnPlayLevelButtonEvent.Invoke(_levelNumber);
        }

    }
}
