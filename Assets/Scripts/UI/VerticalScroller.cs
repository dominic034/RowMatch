using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform), typeof(SpriteMask))]
    public class VerticalScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform viewPort;
        private float _lastMousePos;
        private float _beginingYPos;

        private RectTransform RectTransform
        {
            get { return GetComponent<RectTransform>(); }
        }

        private bool IsScrollable
        {
            get { return viewPort.rect != RectTransform.rect; }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _lastMousePos = GetScreenPosition();
            _beginingYPos = viewPort.anchoredPosition.y;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsScrollable)
                return;
            
            var currPos = GetScreenPosition();
            float difference =  currPos - _lastMousePos;
            //
            // if(Mathf.Abs(difference) < .1f)
            //     return;

            float position = _beginingYPos + difference;

            if (position < 0)
                position = 0;
            
            if (position > .88f)
                position = .88f;

            viewPort.anchoredPosition = new Vector2(0, position);
            // viewPort.DOAnchorPosY(position, 0);

            _lastMousePos = currPos;
            _beginingYPos = viewPort.anchoredPosition.y;
        }

        private float GetScreenPosition()
        {
            return Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
        }
    }
}
