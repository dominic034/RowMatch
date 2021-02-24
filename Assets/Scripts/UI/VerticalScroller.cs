using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(RectTransform), typeof(SpriteMask))]
    public class VerticalScroller : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private float space = 0;
        
        private float _lastMousePos;
        private float _beginingYPos;
        private float _maxScrollPos;
        private List<ScrollerCell> _cells;
        
        private readonly Vector2 minAnchors = new Vector2(0, 1);
        private readonly Vector2 maxAcnchors = new Vector2(1, 1);
        private readonly Vector2 pivot = new Vector2(.5f, 1);
        

        private RectTransform RectTransform
        {
            get { return GetComponent<RectTransform>(); }
        }

        private VerticalLayoutGroup VerticalLayoutGroup
        {
            get
            {
                if (content.GetComponent<VerticalLayoutGroup>() == null)
                    content.gameObject.AddComponent<VerticalLayoutGroup>();

                return content.GetComponent<VerticalLayoutGroup>();
            }
        }

        private bool IsScrollable
        {
            get { return content.rect.height > RectTransform.rect.height;  }
        }

        private void Awake()
        {
            InitializeContent();
            ReloadScroller();
        }

        public void SetData(List<ScrollerCell> data)
        {
            if(_cells == null)
                _cells = new List<ScrollerCell>();
            
            _cells.Clear();
            _cells = data;
        }

        public void ReloadScroller()
        {
            VerticalLayoutGroup.spacing = (space / transform.localScale.y) * .1f;
            float cellHeight = .2f;
            float spacing = VerticalLayoutGroup.spacing;
            int totalCellCount = 10;

            float totalHeight = totalCellCount * cellHeight + spacing * (totalCellCount - 1);
            content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
            _maxScrollPos = content.sizeDelta.y - RectTransform.sizeDelta.y;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _lastMousePos = GetScreenPosition();
            _beginingYPos = content.anchoredPosition.y;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsScrollable)
                return;
            
            var currPos = GetScreenPosition();
            float difference =  currPos - _lastMousePos;
            float position = _beginingYPos + difference;

            if (position < 0)
                position = 0;
            
            if (position > _maxScrollPos)
                position = _maxScrollPos;

            content.anchoredPosition = new Vector2(0, position);
            _lastMousePos = currPos;
            _beginingYPos = content.anchoredPosition.y;
        }

        private float GetScreenPosition()
        {
            return Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
        }

        private void InitializeContent()
        {
            content.anchorMin = minAnchors;
            content.anchorMax = maxAcnchors;
            content.pivot = pivot;
            content.offsetMin = new Vector2(0, content.offsetMin.y);
            content.offsetMax = new Vector2(0, content.offsetMax.y);
            content.anchoredPosition = Vector2.zero;
        }
    }
}
