// Player JoyStick의 정보를 바탕으로 Player을 조절하는 Script.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementScript : MonoBehaviour
{
	public float currentSpeed;     // 플레이어의 이동속도.
	public Transform cameraMain;   // 플레이어의 카메라.
	public float jumpForce = 500;  // 점프 높이.
	public Vector3 cameraPosition; // 플레이어의 내부 카메라 위치.

	public int maxSpeed = 5;                   // 플레이어의 최대 이동속도.
	public float deaccelerationSpeed = 15.0f;  // 플레이어가 바로 멈추는 정도.
	public float accelerationSpeed = 50000.0f; // 이동시에 작용하는 힘.
	public bool grounded;                      // 플레이어의 접촉 여부.

	// BulletSpawn 게임 오브젝트를 기입.
	[HideInInspector]
	public Transform bulletSpawn;
	public bool been_to_meele_anim = false;

	[Header("BloodForMelleAttaacks")]
	public GameObject bloodEffect; // 탄환 적중 시의 효과.

	[Header("Player SOUNDS")]
	public AudioSource _freakingZombiesSound; // 재장전 효과음.

}