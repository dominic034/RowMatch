using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform), typeof(SpriteMask))]
    public class VerticalScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler
    {
        [SerializeField] private RectTransform viewPort;
        private Vector2 _lastMousePosition;

        private float _viewPortStartYPos;

        // private float ViewPortLocalYPosition
        // {
        //     get { return viewPort.transform.localPosition.y; }
        // }
        //
        // private Vector2 GetMousePosition()
        // {
        //     return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewPort, eventData.position,
                eventData.pressEventCamera, out _lastMousePosition);
            _viewPortStartYPos = viewPort.anchoredPosition.y;
        }

        public void OnEndDrag(PointerEventData eventData)
        {


        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localCursor;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewPort, eventData.position,
                eventData.pressEventCamera, out localCursor); // GetMousePosition();

            var pointerDelta = localCursor.y - _viewPortStartYPos;
            float position = _viewPortStartYPos + pointerDelta;
            viewPort.DOAnchorPosY(position, 0);

            // Vector2 _dragPosition = GetMousePosition();
            //
            // var resultY = _dragPosition.y - _lastMousePosition.y;
            // viewPort.transform.DOLocalMoveY(ViewPortLocalYPosition + resultY, 0);
            // _lastMousePosition = _dragPosition;
        }

        public void OnScroll(PointerEventData eventData)
        {
        }
    }
}
