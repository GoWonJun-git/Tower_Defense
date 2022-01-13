// Camera JoyStick의 정보를 바탕으로 화면을 조절하는 Script.
using UnityEngine;
using System.Collections;

public class MouseLookScript : MonoBehaviour
{
	[HideInInspector]
	public Transform myCamera;

	[Header("Z Rotation Camera")]
	private float timer;
	private int int_timer;
	private float zRotation;
	private float wantedZ;
	private float timeSpeed = 2;
	private float timerToRotateZ;

	// 마우스 감도 및 무기 속성.
	public float mouseSensitvity = 0;
	public float mouseSensitvity_notAiming = 30;
	public float mouseSensitvity_aiming = 5;

	// 카메라 회전이 마우스 움직임보다 느린 정도.
	public float yRotationSpeed, xCameraSpeed;
	private float rotationYVelocity, cameraXVelocity;

	[HideInInspector]
	public float wantedYRotation;
	public float currentYRotation;
	public float wantedCameraXRotation;
	public float currentCameraXRotation;

	public float topAngleView = 60;     // 상단 카메라 앵글.
	public float bottomAngleView = -45; // 하단 카메라 앵글.

	public Joystick_Camera fps_joy;
	public GunScript gunScript;

	// 화면 이동 구현.
	void Update()
	{
		if (fps_joy.isTouch)
			MouseInputMovement();

		if (GetComponent<PlayerMovementScript>().currentSpeed > 1)
			HeadMovement();
	}

	// 카메라에 Z회전을 적용.
	void HeadMovement()
	{
		timer += timeSpeed * Time.deltaTime;
		int_timer = Mathf.RoundToInt(timer);

		if (int_timer % 2 == 0)
			wantedZ = -1;
		else
			wantedZ = 1;

		zRotation = Mathf.Lerp(zRotation, wantedZ, Time.deltaTime * timerToRotateZ);
	}

	// 조준 시 마우스 감도 변경.
	void FixedUpdate()
	{
		if (gunScript.is_LookOn)
			mouseSensitvity = mouseSensitvity_aiming;
		else
			mouseSensitvity = mouseSensitvity_notAiming;

		ApplyingStuff();
	}

	// 마우스 이동시 원하는 값이 증가/감소. 
	// 카메라 회전 X를 상단 및 하단 각도로 고정. 
	void MouseInputMovement()
	{
		wantedYRotation += fps_joy.Vx * mouseSensitvity;
		wantedCameraXRotation += fps_joy.Vy * mouseSensitvity;
		wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);
	}

	// 원하는 움직임을 부드럽게 변경.
	// 카메라가 원하는 회전을 변환에 적용. 
	void ApplyingStuff()
	{
		currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
		currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);

		transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
		myCamera.localRotation = Quaternion.Euler(currentCameraXRotation, 0, zRotation);
	}

}