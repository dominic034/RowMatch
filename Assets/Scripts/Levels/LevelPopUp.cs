using System.Collections;
using UI;
using UnityEngine;

namespace Levels
{
    public class LevelPopUp : PopUp
    {
        [SerializeField] private VerticalScroller verticalScroller;
        [SerializeField] private float newScoreDelay;

        private void Awake()
        {
            exitButton.OnClick.AddListener(OnClickedExitButton);
            verticalScroller.OnEachCellCreated = OnEachCellCreated;
        }

        private void Start()
        {
            GameManager.Instance.OnLevelCompletedEvent.AddListener(OnLevelCompleted);
            GameManager.Instance.OnOpenLevelsPopUp.AddListener(OnOpenLevelsPopUp);
            GameManager.Instance.OnPlayLevelButtonEvent.AddListener(OnClickedPlayLevel);
        }

        private void OnDestroy()
        {
            exitButton.OnClick.RemoveAllListeners();
        }

        private void OnLevelCompleted(CompleteType type)
        {
            if (type == CompleteType.NewScore)
                StartCoroutine(DelayForOpen(newScoreDelay));
        }
        
        private void OnClickedPlayLevel(int no)
        {
            panel.SetActive(false);
        }
        
        private void OnEachCellCreated(ScrollerCell cell, int index)
        {
            var levelCell = (LevelCell) cell;
            levelCell.SetView(LevelLoader.Instance.GetLevelAtIndex(index));
        }
        
        [ContextMenu("Open")]
        private void OnOpenLevelsPopUp()
        {
            panel.SetActive(true);
            StartCoroutine(Wait());
        }
        
        private IEnumerator DelayForOpen(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameManager.Instance.OnOpenLevelsPopUp.Invoke();
        }
        
        private IEnumerator Wait()
        {
            yield return new WaitForEndOfFrame();
            verticalScroller.ReloadScroller(LevelLoader.Instance.GetLevelsCount());
        }

        private void OnClickedExitButton()
        {
            GameManager.Instance.OnReturnMainMenu.Invoke();
            panel.SetActive(false);
        }
    }
}
