using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class SpriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private bool interactable = true;
        [SerializeField] private TextMeshPro title;
        [SerializeField] private ColorBlock colors;
        [SerializeField] private Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();

        private SpriteRenderer _spriteRenderer;
        private Material _rendererMaterial;
        private bool _isPointerOn;
        private bool _isPointerDown;

        public Button.ButtonClickedEvent OnClick
        {
            get { return onClick; }
            set { onClick = value; }
        }
        
        public InteractableChangedEvent OnInteractableChangedEvent { get; private set; } = new InteractableChangedEvent();

        public bool Interactable
        {
            get { return interactable; }
            set
            {
                interactable = value; 
                EvaluatePointerState();
            }
        }

        private Material RendererMaterial
        {
            get
            {
                if (_rendererMaterial == null)
                {
                    _rendererMaterial = SpriteRenderer.material;
                    _rendererMaterial.name = $"{_rendererMaterial.name} ({gameObject.name})";
                }

                return _rendererMaterial;
            }
        }
        
        private SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();

                return _spriteRenderer;
            }
        }

        private void OnEnable()
        {
            EvaluatePointerState();
        }

        private bool IsActive()
        {
            return gameObject.activeSelf && gameObject.activeInHierarchy;
        }

        private void EvaluatePointerState()
        {
            SelectionState state = SelectionState.Normal;

            if (!interactable)
            {
                state = SelectionState.Disabled;
                DoColorTransition(state);
                return;
            }

            if (!_isPointerDown || !_isPointerOn)
                state = SelectionState.Normal;

            if (_isPointerOn)
                state = SelectionState.Highlighted;

            if (_isPointerDown)
                state = SelectionState.Pressed;

            switch (state)
            {
                case SelectionState.Normal:
                    break;
                case SelectionState.Highlighted:
                    break;
                case SelectionState.Pressed:
                    onClick.Invoke();
                    _isPointerDown = false;
                    break;
            }
            
            DoColorTransition(state);
        }
        
        private void DoColorTransition(SelectionState state)
        {
            if (!gameObject.activeInHierarchy)
                return;
            
            Color color = Color.white;
            switch (state)
            {
                case SelectionState.Normal:
                    color = colors.normalColor;
                    break;
                case SelectionState.Highlighted:
                    color = colors.highlightedColor;
                    break;
                case SelectionState.Pressed:
                    color = colors.pressedColor;
                    break;
                case SelectionState.Disabled:
                    color = colors.disabledColor;
                    break;
            }
            RendererMaterial.color = color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!IsActive() || !interactable)
                return;
            
            _isPointerOn = true;
            EvaluatePointerState();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!IsActive() || !interactable)
                return;

            _isPointerOn = false;
            _isPointerDown = false;
            EvaluatePointerState();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if(!IsActive() || !interactable)
                return;

            if(eventData.button != PointerEventData.InputButton.Left)
                return;
            
            if(EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            _isPointerDown = true;
            EvaluatePointerState();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(!IsActive() || !interactable)
                return;

            if(eventData.button != PointerEventData.InputButton.Left)
                return;

            _isPointerDown = false;
            EvaluatePointerState();
        }     
        
        private enum SelectionState
        {
            Normal,
            Highlighted,
            Pressed,
            Disabled,
        }

        public class InteractableChangedEvent : UnityEvent<bool>
        {
            
        }
    }
}
