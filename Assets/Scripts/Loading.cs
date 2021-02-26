using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Start()
    {
        panel.SetActive(true);
        GameManager.Instance.OnLevelsAreLoaded.AddListener(OnLevelsAreLoaded);
    }

    private void OnLevelsAreLoaded()
    {
        panel.SetActive(false);
    }
}
