using System.Collections;
using UI;
using UnityEngine;

namespace Levels
{
    public class LevelPopUp : PopUp
    {
        [SerializeField] private VerticalScroller verticalScroller;
        
        private void Awake()
        {
            exitButton.OnClick.AddListener(OnClickedExitButton);
            verticalScroller.OnEachCellCreated = OnEachCellCreated;
        }

        private void Start()
        {
            GameManager.Instance.OnOpenLevelsPopUp.AddListener(OnOpenLevelsPopUp);
            GameManager.Instance.OnPlayLevelButtonEvent.AddListener(OnClickedPlayLevel);
        }

        private void OnDestroy()
        {
            exitButton.OnClick.RemoveAllListeners();
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

        private IEnumerator Wait()
        {
            yield return new WaitForEndOfFrame();
            verticalScroller.ReloadScroller(LevelLoader.Instance.GetLevelsCount());
        }

        private void OnClickedExitButton()
        {
            panel.SetActive(false);
        }
    }
}
