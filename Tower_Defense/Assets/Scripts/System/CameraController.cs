/* 플레이어의 Camera 담당 Script.
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 30f;           // 카메라 이동 속도.
    public float scrollSeppd = 5f;         // 카메라 줌 속도.
    public float panBorderThickness = 10f; // 마우스와 화면 끝과의 간격.
    private bool doMovement = false;       // 이동 여부 확인 키.

    // WSAD키와 마우스 이동으로 카메라 이동.
    void Update()
    {
        // Game Over인 경우 카메라 기능 중지.
        if (GameManager.GameIsOvers)
        {
            // 카메라의 모든 요소를 비활성화.
            this.enabled = false;
            // 이후 Update 함수 정지.
            return;
        }
        
        // ESC 키 입력 시 카메라 이동 On / Off
        if (Input.GetKeyDown(KeyCode.P))
            doMovement = !doMovement;

        // 카메라 이동이 Off일 시 움직이지 않음.
        if (!doMovement)
            return;
        
        // W키 입력 시, 마우스가 화면 위에 있을 시 작동.
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            // 위로 이동.
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        // S키 입력 시, 마우스가 화면 아래에 있을 시 작동.
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            // 뒤로 이동.
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        // D키 입력 시, 마우스가 화면 우측에 있을 시 작동.
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            // 우측으로 이동.
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        // A키 입력 시, 마우스가 화면 좌측에 있을 시 작동.
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            // 좌측으로 이동.
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        // scroll 변수를 마우스 휠 이동 값으로 초기화.
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // pos 변수를 현재 카메라 위치로 초기화.
        Vector3 pos = transform.position;

        // pos 변수의 y축 값을 변경.
        pos.y -= scroll * 1000 * scrollSeppd * Time.deltaTime;

        // 현재 카메라 위치의 최소, 최대 값을 지정.
        pos.y = Mathf.Clamp(pos.y, 30, 80);
        pos.x = Mathf.Clamp(pos.x, 30, 100);
        pos.z = Mathf.Clamp(pos.z, -67, -7);

        // 변경된 pos의 값으로 카메라를 이동.
        transform.position = pos;
    }

}
*/