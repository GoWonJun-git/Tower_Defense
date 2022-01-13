// FPS ��忡�� Player JoyStick�� ���� Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick_Player : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // �����̴� ������ �����ϱ� ���ؼ� ����.
    public RectTransform rect_Background;
    public RectTransform rect_Joystick;

    private float radius; // ��׶����� �������� ������ �����ų ����.

    public Transform Player; // ȭ�鿡�� ������ �÷��̾�.
    private Rigidbody rigid; // �÷��̾��� Rigidbody Component
    public float moveSpeed;  // �÷��̾��� �̵� �ӵ�.
    Vector3 moveDir;         // �÷��̾��� �̵� ����.

    private bool isTouch = false; // ���̽�ƽ ��ġ ����.
    public AudioSource _runSound; // �޸��� ȿ����.

    // �ش� Script�� ��ü �ʱ�ȭ.
    void Start()
    {
        radius = rect_Background.rect.width * 0.5f;
        rigid = Player.GetComponent<Rigidbody>();
    }
    
    // �̵� ����.
    void Update()
    {
        if (isTouch)
            Player.Translate(moveDir * moveSpeed * Time.deltaTime);
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
        isTouch = false;

        // ���̽�ƽ�� ����ġ�� �̵�.
        rect_Joystick.localPosition = Vector3.zero;

        // �̵� Sound ����.
        _runSound.Stop();
    }

    // ��ġ �� �巡�� ��Ȳ�� ��.
    public void OnDrag(PointerEventData eventData)
    {
        // �̵� Sound ����.
        _runSound.Play();

        // ���콺 ��ǥ���� ������ ��׶��� ��ǥ���� �� ����ŭ ���̽�ƽ(�� ���׶��)�� �̵�.
        Vector2 value = eventData.position - (Vector2)rect_Background.position;

        // ���̽�ƽ�� �̵� ���� ����.
        value = Vector2.ClampMagnitude(value, radius);

        // �Ÿ��� ���� ���ǵ带 �ٸ��� ��ȯ.
        float distance = Vector2.Distance(rect_Background.position, rect_Joystick.position) / radius;

        // �θ�ü(��׶���) �������� ������ ������� ��ǥ�� ����.
        rect_Joystick.localPosition = value;

        // value�� ���Ⱚ ���.
        value = value.normalized;
        moveDir = Vector3.forward * value.y + Vector3.right * value.x;
    }

    // Jump ��ư.
    public void Jump()
    {
        rigid.AddForce(transform.up * 5);
    }

}
