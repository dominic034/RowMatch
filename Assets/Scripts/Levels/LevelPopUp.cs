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
        }

        private void OnDestroy()
        {
            exitButton.OnClick.RemoveAllListeners();
        }

        private void OnEachCellCreated(ScrollerCell cell, int index)
        {
            var levelCell = (LevelCell) cell;
            levelCell.SetView(LevelLoader.Instance.GetLevelAtIndex(index));
        }
        
        private void OnOpenLevelsPopUp()
        {
            
        }

        private void OnClickedExitButton()
        {
            gameObject.SetActive(false);
        }
    }
}
