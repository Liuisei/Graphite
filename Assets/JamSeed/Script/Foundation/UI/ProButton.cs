using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ProButton : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // private Actions
    private Action onEnter;
    private Action onExit;
    private Action onDown;
    private Action onUp;
    private Action onClick;
    private Action onBeginDrag;
    private Action onDrag;
    private Action onEndDrag;

    private Action onLongPress;
    private Action onDoubleClick;
    private Action onHold;
    private Action<Vector2> onPointerMove;
    private Action<int> onMultiTouch;

    [SerializeField] private float longPressThreshold = 0.5f;
    [SerializeField] private float doubleClickThreshold = 0.3f;

    private bool isPointerDown = false;
    private float pointerDownTime = 0f;
    private float lastClickTime = -1f;
    private bool longPressTriggered = false;
    private bool pointerInside = false;

    // Add メソッド
    public void AddOnEnter(Action callback) => onEnter += callback;
    public void AddOnExit(Action callback) => onExit += callback;
    public void AddOnDown(Action callback) => onDown += callback;
    public void AddOnUp(Action callback) => onUp += callback;
    public void AddOnClick(Action callback) => onClick += callback;
    public void AddOnBeginDrag(Action callback) => onBeginDrag += callback;
    public void AddOnDrag(Action callback) => onDrag += callback;
    public void AddOnEndDrag(Action callback) => onEndDrag += callback;
    public void AddOnLongPress(Action callback) => onLongPress += callback;
    public void AddOnDoubleClick(Action callback) => onDoubleClick += callback;
    public void AddOnHold(Action callback) => onHold += callback;
    public void AddOnPointerMove(Action<Vector2> callback) => onPointerMove += callback;
    public void AddOnMultiTouch(Action<int> callback) => onMultiTouch += callback;

    // Remove メソッド（必要に応じて追加）
    public void RemoveOnEnter(Action callback) => onEnter -= callback;
    public void RemoveOnExit(Action callback) => onExit -= callback;
    public void RemoveOnDown(Action callback) => onDown -= callback;
    public void RemoveOnUp(Action callback) => onUp -= callback;
    public void RemoveOnClick(Action callback) => onClick -= callback;
    public void RemoveOnBeginDrag(Action callback) => onBeginDrag -= callback;
    public void RemoveOnDrag(Action callback) => onDrag -= callback;
    public void RemoveOnEndDrag(Action callback) => onEndDrag -= callback;
    public void RemoveOnLongPress(Action callback) => onLongPress -= callback;
    public void RemoveOnDoubleClick(Action callback) => onDoubleClick -= callback;
    public void RemoveOnHold(Action callback) => onHold -= callback;
    public void RemoveOnPointerMove(Action<Vector2> callback) => onPointerMove -= callback;
    public void RemoveOnMultiTouch(Action<int> callback) => onMultiTouch -= callback;

    // イベント発火
    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerInside = true;
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerInside = false;
        onExit?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        longPressTriggered = false;
        pointerDownTime = Time.time;
        onDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        onUp?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();

        if (Time.time - lastClickTime <= doubleClickThreshold)
        {
            onDoubleClick?.Invoke();
            lastClickTime = -1f;
        }
        else
        {
            lastClickTime = Time.time;
        }
    }

    public void OnBeginDrag(PointerEventData eventData) => onBeginDrag?.Invoke();
    public void OnDrag(PointerEventData eventData) => onDrag?.Invoke();
    public void OnEndDrag(PointerEventData eventData) => onEndDrag?.Invoke();

    void Update()
    {
        if (pointerInside)
            onPointerMove?.Invoke(Input.mousePosition);

        if (isPointerDown)
        {
            onHold?.Invoke();

            if (!longPressTriggered && Time.time - pointerDownTime >= longPressThreshold)
            {
                longPressTriggered = true;
                onLongPress?.Invoke();
            }
        }

        if (Input.touchCount > 1)
        {
            onMultiTouch?.Invoke(Input.touchCount);
        }
    }

    private void OnDestroy()
    {
        onEnter = null;
        onExit = null;
        onDown = null;
        onUp = null;
        onClick = null;
        onBeginDrag = null;
        onDrag = null;
        onEndDrag = null;
        onLongPress = null;
        onDoubleClick = null;
        onHold = null;
        onPointerMove = null;
        onMultiTouch = null;
    }
}
