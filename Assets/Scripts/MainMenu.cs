using UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SpriteButton levelsButton;

    private void Awake()
    {
        levelsButton.OnClick.AddListener(OnClickedLevelsButton);    
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
