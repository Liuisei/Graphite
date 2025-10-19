using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;

public class LiteButton : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Action onEnter;
    private Action onExit;
    private Action onDown;
    private Action onUp;
    private Action onClick;
    private Action onBeginDrag;
    private Action onDrag;
    private Action onEndDrag;
    public TextMeshProUGUI textMesh;
    public Image image;

    // Add メソッド
    public void AddOnEnter(Action callback) => onEnter += callback;
    public void AddOnExit(Action callback) => onExit += callback;
    public void AddOnDown(Action callback) => onDown += callback;
    public void AddOnUp(Action callback) => onUp += callback;
    public void AddOnClick(Action callback) => onClick += callback;
    public void AddOnBeginDrag(Action callback) => onBeginDrag += callback;
    public void AddOnDrag(Action callback) => onDrag += callback;
    public void AddOnEndDrag(Action callback) => onEndDrag += callback;

    // Remove メソッド
    public void RemoveOnEnter(Action callback) => onEnter -= callback;
    public void RemoveOnExit(Action callback) => onExit -= callback;
    public void RemoveOnDown(Action callback) => onDown -= callback;
    public void RemoveOnUp(Action callback) => onUp -= callback;
    public void RemoveOnClick(Action callback) => onClick -= callback;
    public void RemoveOnBeginDrag(Action callback) => onBeginDrag -= callback;
    public void RemoveOnDrag(Action callback) => onDrag -= callback;
    public void RemoveOnEndDrag(Action callback) => onEndDrag -= callback;

    // イベント発火
    public void OnPointerEnter(PointerEventData eventData) => onEnter?.Invoke();
    public void OnPointerExit(PointerEventData eventData) => onExit?.Invoke();
    public void OnPointerDown(PointerEventData eventData) => onDown?.Invoke();
    public void OnPointerUp(PointerEventData eventData) => onUp?.Invoke();
    public void OnPointerClick(PointerEventData eventData) => onClick?.Invoke();
    public void OnBeginDrag(PointerEventData eventData) => onBeginDrag?.Invoke();
    public void OnDrag(PointerEventData eventData) => onDrag?.Invoke();
    public void OnEndDrag(PointerEventData eventData) => onEndDrag?.Invoke();

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
    }
}
