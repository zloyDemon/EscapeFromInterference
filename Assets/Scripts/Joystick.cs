using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform bgJoysick;
    [SerializeField] private RectTransform joysick;

    public event Action<Vector2> JoystickDragging = v => { };
    
    private bool isDraging;
    private bool isCanDragging;
    private Vector2 joystickValue;

    public Vector2 JoystickValue => joystickValue;

    private void Start()
    {
        GameplayManager.Instance.PauseGame += PauseGame;
        isCanDragging = true;
    }

    private void OnDestroy()
    {
        GameplayManager.Instance.PauseGame += PauseGame;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isCanDragging) return;
        SetJoystickPosition(bgJoysick, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isCanDragging) return;
        isDraging = false;
        joysick.anchoredPosition = Vector2.zero;
        joystickValue = Vector2.zero;
        JoystickDragging(joystickValue);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isCanDragging) return;
        isDraging = true;
        SetJoystickPosition(bgJoysick, eventData);
    }

    private void SetJoystickPosition(RectTransform draggingTransform, PointerEventData eventData)
    {

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(draggingTransform, eventData.position,
            eventData.pressEventCamera, out Vector2 newPosition))
        {
            joysick.anchoredPosition = newPosition.normalized * Mathf.Clamp(newPosition.magnitude, 0, bgJoysick.rect.width / 2);
            joystickValue = joysick.anchoredPosition / (bgJoysick.rect.size / 2);
            JoystickDragging(joystickValue);
        }    
    }

    private void PauseGame(bool isPause) => isCanDragging = !isPause;
}
