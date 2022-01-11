// FPS ��忡�� Camera JoyStick�� ���� Script.
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

    // �ش� Script�� ��ü �ʱ�ȭ.
    void Start()
    {
        radius = rectBackground.rect.width / 2;
    }

    /* �������̽� ���� */
    // ��ġ�� ���۵��� ��.
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    // ��ġ�� ������� ��.
    public void OnPointerUp(PointerEventData eventData)
    {
        rectCamstick.localPosition = Vector3.zero;
        isTouch = false;
    }

    // ��ġ �� �巡�� ��Ȳ�� ��.
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

