using System;
using UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SpriteButton levelsButton;

    private void Awake()
    {
        levelsButton.OnClick.AddListener(OnClickedLevelsButton);
    }

    private void Start()
    {
        GameManager.Instance.OnPlayLevelButtonEvent.AddListener(OnClickedPlayLevel);
        GameManager.Instance.OnLevelCompletedEvent.AddListener(OnLevelCompleted);
    }

    private void OnLevelCompleted(CompleteType type)
    {
        if(type != CompleteType.None)
            return;
        
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
        Debug.Log("main menu button");
        GameManager.Instance.OnOpenLevelsPopUp.Invoke();
    }
}
