// FPS 모드에서 Camera JoyStick에 관한 Script.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick_Camera : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform rectBackground;
    [SerializeField] private RectTransform rectCamstick;

    private float radius;

    [SerializeField] private float rotateSpeed;

    public bool isTouch;

    public float Vx = 0f;
    public float Vy = 0f;

    // 해당 Script의 객체 초기화.
    void Start()
    {
        radius = rectBackground.rect.width / 2;
    }

    /* 인터페이스 구현 */
    // 터치가 시작됐을 때.
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    // 터치가 종료됐을 때.
    public void OnPointerUp(PointerEventData eventData)
    {
        rectCamstick.localPosition = Vector3.zero;
        isTouch = false;
    }

    // 터치 중 드래그 상황일 때.
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rectBackground.position;

        value = Vector2.ClampMagnitude(value, radius);

        rectCamstick.localPosition = value;

        float distance = Vector2.Distance(rectBackground.position, rectCamstick.position) / radius;

        var direction = value.normalized;

        Vx = direction.x * distance * rotateSpeed;
        Vy = direction.y * distance * (rotateSpeed / 2) * - 1;
    }

}

