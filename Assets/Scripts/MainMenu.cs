using UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SpriteButton levelsButton;

    private void Awake()
    {
        levelsButton.OnClick.AddListener(OnClickedLevelsButton);
        panel.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.OnPlayLevelButtonEvent.AddListener(OnClickedPlayLevel);
        GameManager.Instance.OnLevelCompletedEvent.AddListener(OnLevelCompleted);
        GameManager.Instance.OnReturnMainMenu.AddListener(OnReturnMainMenu);
        GameManager.Instance.OnOpenLevelsPopUp.AddListener(OnOpenLevelsPopUp);
        GameManager.Instance.OnLevelsAreLoaded.AddListener(OnLevelsAreLoaded);
    }

    private void OnLevelsAreLoaded()
    {
        panel.SetActive(true);
    }
    
    private void OnLevelCompleted(CompleteType type)
    {
        if(type != CompleteType.None)
            return;
        
        panel.SetActive(true); 
    }

    private void OnOpenLevelsPopUp()
    {
        panel.SetActive(false);
    }

    private void OnReturnMainMenu()
    {
        panel.SetActive(true);
    }
    
    private void OnClickedPlayLevel(int no)
    {
        panel.SetActive(false);
    }
    
    private void OnDestroy()
    {
        levelsButton.OnClick.RemoveAllListeners();
    }

    private void OnClickedLevelsButton()
    {
        GameManager.Instance.OnOpenLevelsPopUp.Invoke();
    }
}
