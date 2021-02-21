using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CustomButton : Selectable, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();

        public Button.ButtonClickedEvent OnClick
        {
            get { return onClick; }
        }

        public TextMeshProUGUI Title
        {
            get { return title; }
        }
        
        public void SetTitle(string text)
        {
            title.text = text;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!IsActive() || !IsInteractable())
                return;
            
            onClick.Invoke();
        }
    }
}
