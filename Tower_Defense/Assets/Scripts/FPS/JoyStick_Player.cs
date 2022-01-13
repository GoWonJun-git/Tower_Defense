// FPS 모드에서 Player JoyStick에 관한 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick_Player : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // 움직이는 범위를 제한하기 위해서 선언.
    public RectTransform rect_Background;
    public RectTransform rect_Joystick;

    private float radius; // 백그라운드의 반지름의 범위를 저장시킬 변수.

    public Transform Player; // 화면에서 움직일 플레이어.
    private Rigidbody rigid; // 플레이어의 Rigidbody Component
    public float moveSpeed;  // 플레이어의 이동 속도.
    Vector3 moveDir;         // 플레이어의 이동 방향.

    private bool isTouch = false; // 조이스틱 터치 여부.
    public AudioSource _runSound; // 달리기 효과음.

    // 해당 Script의 객체 초기화.
    void Start()
    {
        radius = rect_Background.rect.width * 0.5f;
        rigid = Player.GetComponent<Rigidbody>();
    }
    
    // 이동 구현.
    void Update()
    {
        if (isTouch)
            Player.Translate(moveDir * moveSpeed * Time.deltaTime);
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
        isTouch = false;

        // 조이스틱을 원위치로 이동.
        rect_Joystick.localPosition = Vector3.zero;

        // 이동 Sound 중지.
        _runSound.Stop();
    }

    // 터치 중 드래그 상황일 때.
    public void OnDrag(PointerEventData eventData)
    {
        // 이동 Sound 시작.
        _runSound.Play();

        // 마우스 좌표에서 검은색 백그라운드 좌표값을 뺀 값만큼 조이스틱(흰 동그라미)를 이동.
        Vector2 value = eventData.position - (Vector2)rect_Background.position;

        // 조이스틱의 이동 범위 제한.
        value = Vector2.ClampMagnitude(value, radius);

        // 거리에 따른 스피드를 다르게 변환.
        float distance = Vector2.Distance(rect_Background.position, rect_Joystick.position) / radius;

        // 부모객체(백그라운드) 기준으로 떨어질 상대적인 좌표값 대입.
        rect_Joystick.localPosition = value;

        // value의 방향값 계산.
        value = value.normalized;
        moveDir = Vector3.forward * value.y + Vector3.right * value.x;
    }

    // Jump 버튼.
    public void Jump()
    {
        rigid.AddForce(transform.up * 5);
    }

}
