// FPS 모드에서 총기에 관한 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunStyles{
	nonautomatic,automatic
}

public class GunScript : MonoBehaviour 
{
	public GunStyles currentStyle; // 무기의 종류.

	[HideInInspector]
	public MouseLookScript mls;

	[Header("Player movement properties")]
	public int walkingSpeed = 8;  // 걷기 속도.
	public int runningSpeed = 10; // 대쉬 속도.

	[Header("Bullet properties")]
	public float bulletsIHave = 999;          // 전체 탄환 수.
	public float bulletsInTheGun = 50;        // 한번에 사용가능한 탄환 수.
	public float amountOfBulletsPerLoad = 50; // 재장전 시 변경되는 최대 탄환 수.

	private Transform player;
	private Camera cameraComponent;

	private PlayerMovementScript pmS;

	[HideInInspector]
	public Vector3 currentGunPosition;

	[Header("Gun Positioning")]
	public Vector3 restPlacePosition; // 플레이어 설정의 Vector3 위치.
	public Vector3 aimPlacePosition;  // 플레이어 SetUp의 Vector3 위치.
	public float gunAimTime = 0.1f;   // 조준자세를 취하는 시간.

	[HideInInspector]
	public bool reloading;

	private Vector3 gunPosVelocity;
	private float cameraZoomVelocity;
	private float secondCameraZoomVelocity;

	[HideInInspector]
	public float recoilAmount_z = 0.5f;
	public float recoilAmount_x = 0.5f;
	public float recoilAmount_y = 0.5f;

	[Header("Recoil Not Aiming")]
	public float recoilAmount_x_non = 0.5f; // 비조준 상태에서의 x축 반동량.
	public float recoilAmount_y_non = 0.5f; // 비조준 상태에서의 y축 반동량.
	public float recoilAmount_z_non = 0.5f; // 비조준 상태에서의 z축 반동량.

	[Header("Recoil Aiming")]
	public float recoilAmount_x_ = 0.5f; // 조준 상태에서의 x축 반동량.
	public float recoilAmount_y_ = 0.5f; // 조준 상태에서의 y축 반동량.
	public float recoilAmount_z_ = 0.5f; // 조준 상태에서의 z축 반동량.

	[HideInInspector]
	public float velocity_z_recoil, velocity_x_recoil, velocity_y_recoil;

	[Header("")]
	public float recoilOverTime_x = 0.5f; // 반동 후 무기가 원래의 x축으로 돌아오는 데 걸리는 시간
	public float recoilOverTime_y = 0.5f; // 반동 후 무기가 원래의 y축으로 돌아오는 데 걸리는 시간
	public float recoilOverTime_z = 0.5f; // 반동 후 무기가 원래의 z축으로 돌아오는 데 걸리는 시간

	[Header("Gun Precision")]
	public float gunPrecision_notAiming = 200.0f;      // 비조준 상태에서의 추가 반동량.
	public float gunPrecision_aiming = 100.0f;         // 조준 상태에서의 추가 반동량.
	public float cameraZoomRatio_notAiming = 60;       // 비조준 상태에서의 첫 번째 카메라의 FOV.
	public float cameraZoomRatio_aiming = 40;          // 조준 상태에서의 첫 번째 카메라의 FOV.
	public float secondCameraZoomRatio_notAiming = 60; // 비조준 상태에서의 두 번째 카메라의 FOV.
	public float secondCameraZoomRatio_aiming = 40;    // 조준 상태에서의 두 번째 카메라의 FOV.

	[HideInInspector]
	public float gunPrecision;

	[Header("Animation names")]
	public string reloadAnimationName = "Player_Reload";
	public string aimingAnimationName = "Player_AImpose";
	public string meeleAnimationName = "Character_Malee";

	// 해당 스크립트에 필요한 변수의 초기화.
	void Awake()
	{
		mls = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLookScript>();
		player = mls.transform;
		mainCamera = mls.myCamera;
		secondCamera = GameObject.FindGameObjectWithTag("SecondCamera").GetComponent<Camera>();
		cameraComponent = mainCamera.GetComponent<Camera>();
		pmS = player.GetComponent<PlayerMovementScript>();

		bulletSpawnPlace = GameObject.FindGameObjectWithTag("BulletSpawn");
		hitMarker = transform.Find("hitMarkerSound").GetComponent<AudioSource>();

		rotationLastY = mls.currentYRotation;
		rotationLastX = mls.currentCameraXRotation;

		pmS.maxSpeed = walkingSpeed;
	}

	// 메서드를 호출하는 Update 루프.
	void Update()
	{
		Animations();
		GiveCameraScriptMySensitvity();
		PositionGun();
		CrossHairExpansionWhenWalking();

		if (is_Shooting)
			Shooting();
	}

	// 무기 위치 계산.
	void FixedUpdate()
	{
		RotationGun();
		MeeleAnimationsStates();
		CameraLookOn_And_Out();
	}

	// 조준 / 비조준 구분.
	public bool is_LookOn = false;
	void CameraLookOn_And_Out()
    {
		if(is_LookOn)
        {
			gunPrecision = gunPrecision_aiming;
			recoilAmount_x = recoilAmount_x_;
			recoilAmount_y = recoilAmount_y_;
			recoilAmount_z = recoilAmount_z_;
			currentGunPosition = Vector3.SmoothDamp(currentGunPosition, aimPlacePosition, ref gunPosVelocity, gunAimTime);
			cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_aiming, ref cameraZoomVelocity, gunAimTime);
			secondCamera.fieldOfView = Mathf.SmoothDamp(secondCamera.fieldOfView, secondCameraZoomRatio_aiming, ref secondCameraZoomVelocity, gunAimTime);
		}
		else
        {
			gunPrecision = gunPrecision_notAiming;
			recoilAmount_x = recoilAmount_x_non;
			recoilAmount_y = recoilAmount_y_non;
			recoilAmount_z = recoilAmount_z_non;
			currentGunPosition = Vector3.SmoothDamp(currentGunPosition, restPlacePosition, ref gunPosVelocity, gunAimTime);
			cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_notAiming, ref cameraZoomVelocity, gunAimTime);
			secondCamera.fieldOfView = Mathf.SmoothDamp(secondCamera.fieldOfView, secondCameraZoomRatio_notAiming, ref secondCameraZoomVelocity, gunAimTime);
		}
	}
	public void LookOn_And_Out_Check()
	{
		is_LookOn = !is_LookOn;
	}

	// 카메라에 각 총에 대해 다른 감도 옵션을 제공.
	[Header("Sensitvity of the gun")]
	public float mouseSensitvity_notAiming = 0.4f;   // 비조준 상태에서의 감도.
	public float mouseSensitvity_aiming = 0.05f;     // 조준 중 걷기 상태에서의 감도.
	public float mouseSensitvity_running = 0.05f;    // 조준 중 대쉬 상태에서의 감도.
	void GiveCameraScriptMySensitvity()
	{
		mls.mouseSensitvity_notAiming = mouseSensitvity_notAiming;
		mls.mouseSensitvity_aiming = mouseSensitvity_aiming;
	}
	
	// 십자선의 위치 조절.
	void CrossHairExpansionWhenWalking()
	{
		if (player.GetComponent<Rigidbody>().velocity.magnitude > 1 && is_Shooting == false)
		{
			expandValues_crosshair += new Vector2(20, 40) * Time.deltaTime;
			// 걷기 상태.
			if (player.GetComponent<PlayerMovementScript>().maxSpeed < runningSpeed)
			{
				expandValues_crosshair = new Vector2(Mathf.Clamp(expandValues_crosshair.x, 0, 10), Mathf.Clamp(expandValues_crosshair.y, 0, 20));
				fadeout_value = Mathf.Lerp(fadeout_value, 1, Time.deltaTime * 2);
			}
			// 대쉬 상태.
			else
			{
				fadeout_value = Mathf.Lerp(fadeout_value, 0, Time.deltaTime * 10);
				expandValues_crosshair = new Vector2(Mathf.Clamp(expandValues_crosshair.x, 0, 20), Mathf.Clamp(expandValues_crosshair.y, 0, 40));
			}
		}
		// 사격 상태.
		else
		{
			expandValues_crosshair = Vector2.Lerp(expandValues_crosshair, Vector2.zero, Time.deltaTime * 5);
			expandValues_crosshair = new Vector2(Mathf.Clamp(expandValues_crosshair.x, 0, 10), Mathf.Clamp(expandValues_crosshair.y, 0, 20));
			fadeout_value = Mathf.Lerp(fadeout_value, 1, Time.deltaTime * 2);
		}
	}

	// MeeleAttack 애니메이션을 트리거.
	[HideInInspector]
	public bool meeleAttack;
	public bool aiming;
	void MeeleAnimationsStates()
	{
		if (handsAnimator)
		{
			meeleAttack = handsAnimator.GetCurrentAnimatorStateInfo(0).IsName(meeleAnimationName);
			aiming = handsAnimator.GetCurrentAnimatorStateInfo(0).IsName(aimingAnimationName);
		}
	}

	// 플레이어 위치와 회전에 따라 무기 위치를 계산.
	private Vector3 velV;
	[HideInInspector]
	public Transform mainCamera;
	private Camera secondCamera;
	void PositionGun()
	{
		transform.position = Vector3.SmoothDamp(transform.position,
			mainCamera.transform.position -
			(mainCamera.transform.right * (currentGunPosition.x + currentRecoilXPos)) +
			(mainCamera.transform.up * (currentGunPosition.y + currentRecoilYPos)) +
			(mainCamera.transform.forward * (currentGunPosition.z + currentRecoilZPos)), ref velV, 0);

		pmS.cameraPosition = new Vector3(currentRecoilXPos, currentRecoilYPos, 0);

		currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref velocity_z_recoil, recoilOverTime_z);
		currentRecoilXPos = Mathf.SmoothDamp(currentRecoilXPos, 0, ref velocity_x_recoil, recoilOverTime_x);
		currentRecoilYPos = Mathf.SmoothDamp(currentRecoilYPos, 0, ref velocity_y_recoil, recoilOverTime_y);
	}

	// 마우스 모양 회전에 따라 무기를 회전.
	[Header("Rotation")]
	public float rotationLagTime = 0f;                  // 무기 교체에 걸리는 시간.
	public Vector2 forwardRotationAmount = Vector2.one; // 정회전 배율의 값.
	private Vector2 velocityGunRotate;
	private float gunWeightX, gunWeightY;
	private float rotationLastY;
	private float rotationDeltaY;
	private float angularVelocityY;
	private float rotationLastX;
	private float rotationDeltaX;
	private float angularVelocityX;
	void RotationGun()
	{
		rotationDeltaY = mls.currentYRotation - rotationLastY;
		rotationDeltaX = mls.currentCameraXRotation - rotationLastX;

		rotationLastY = mls.currentYRotation;
		rotationLastX = mls.currentCameraXRotation;

		angularVelocityY = Mathf.Lerp(angularVelocityY, rotationDeltaY, Time.deltaTime * 5);
		angularVelocityX = Mathf.Lerp(angularVelocityX, rotationDeltaX, Time.deltaTime * 5);

		gunWeightX = Mathf.SmoothDamp(gunWeightX, mls.currentCameraXRotation, ref velocityGunRotate.x, rotationLagTime);
		gunWeightY = Mathf.SmoothDamp(gunWeightY, mls.currentYRotation, ref velocityGunRotate.y, rotationLagTime);

		transform.rotation = Quaternion.Euler(gunWeightX + (angularVelocityX * forwardRotationAmount.x), gunWeightY + (angularVelocityY * forwardRotationAmount.y), 0);
	}

	// ShootMethod에서 호출되며 발사 시 반동 양이 증가.
	private float currentRecoilZPos;
	private float currentRecoilXPos;
	private float currentRecoilYPos;
	public void RecoilMath()
	{
		currentRecoilZPos -= recoilAmount_z;
		currentRecoilXPos -= (Random.value - 0.5f) * recoilAmount_x;
		currentRecoilYPos -= (Random.value - 0.5f) * recoilAmount_y;
		mls.wantedCameraXRotation -= Mathf.Abs(currentRecoilYPos * gunPrecision);
		mls.wantedYRotation -= (currentRecoilXPos * gunPrecision);

		expandValues_crosshair += new Vector2(6, 12);
	}

	// 총의 자동 사격 여부를 확인하고 그에 따라 ShootMethod를 실행.
	[Header("Shooting setup - MUSTDO")]
	public GameObject bullet;         // 무기에서 발사되는 탄환.
	public float roundsPerSecond;     // 자동 발사일 경우 초당 발사 수.
	private bool is_Shooting = false; // 사격 여부.
	private float waitTillNextFire;
	[HideInInspector]
	public GameObject bulletSpawnPlace;
	public void Shooting()
	{
		is_Shooting = true;
		ShootMethod();
		waitTillNextFire -= roundsPerSecond * Time.deltaTime;
	}

	// Shooting에서 호출되어 총알과 총구 섬광을 생성하고 반동을 요청.
	public GameObject[] muzzelFlash; // 총구 플래시 배열.
	public GameObject muzzelSpawn;   // 총의 섬광이 나올 위치.
	private GameObject holdFlash;
	private void ShootMethod()
	{
		if (waitTillNextFire <= 0 && !reloading)
		{
			if (bulletsInTheGun > 0)
			{
				int randomNumberForMuzzelFlash = Random.Range(0, 5);
				Instantiate(bullet, bulletSpawnPlace.transform.position, bulletSpawnPlace.transform.rotation);

				holdFlash = Instantiate(muzzelFlash[randomNumberForMuzzelFlash], muzzelSpawn.transform.position, muzzelSpawn.transform.rotation * Quaternion.Euler(0, 0, 90)) as GameObject;
				holdFlash.transform.parent = muzzelSpawn.transform;

				shoot_sound_source.Play();

				RecoilMath();

				waitTillNextFire = 1;
				bulletsInTheGun -= 1;
			}
			else
				StartCoroutine("Reload_Animation");
		}
	}

	// 사격 중지.
	public void ShootingEnd()
    {
		is_Shooting = false;
    }

	// 공격 명중 효과음 실행.
	public AudioSource shoot_sound_source, reloadSound_source; //사운드 및 재장전용 오디오.
	public static AudioSource hitMarker;                       // 공격 성공 효과음.
	public static void HitMarkerSound()
	{
		hitMarker.Play();
	}

	// 무기 리로드.
	[Header("reload time after anima")]
	public float reloadChangeBulletsTime; // 재장전 후 경과되는 시간.
	IEnumerator Reload_Animation()
	{
		if (bulletsIHave > 0 && bulletsInTheGun < amountOfBulletsPerLoad && !reloading)
		{
			if (reloadSound_source.isPlaying == false && reloadSound_source != null)
				reloadSound_source.Play();

			handsAnimator.SetBool("reloading", true);
			yield return new WaitForSeconds(0.5f);

			handsAnimator.SetBool("reloading", false);
			yield return new WaitForSeconds(reloadChangeBulletsTime - 0.5f);

			if (meeleAttack == false && pmS.maxSpeed != runningSpeed)
			{
				player.GetComponent<PlayerMovementScript>()._freakingZombiesSound.Play();

				if (bulletsIHave - amountOfBulletsPerLoad >= 0)
				{
					bulletsIHave -= amountOfBulletsPerLoad - bulletsInTheGun;
					bulletsInTheGun = amountOfBulletsPerLoad;
				}
				else if (bulletsIHave - amountOfBulletsPerLoad < 0)
				{
					float valueForBoth = amountOfBulletsPerLoad - bulletsInTheGun;

					if (bulletsIHave - valueForBoth < 0)
					{
						bulletsInTheGun += bulletsIHave;
						bulletsIHave = 0;
					}
					else
					{
						bulletsIHave -= valueForBoth;
						bulletsInTheGun += valueForBoth;
					}
				}
			}
			else
				reloadSound_source.Stop();
		}
	}

	// 총알 수를 hud UI 게임 오브젝트로 설정하고 드로잉.
	public TextMesh HUD_bullets; // 화면에 총알 수를 표시.
	void OnGUI()
	{
		if (!HUD_bullets)
		{
			try
			{
				HUD_bullets = GameObject.Find("HUD_bullets").GetComponent<TextMesh>();
			}
			catch (System.Exception ex)
			{
				print("Couldnt find the HUD_Bullets ->" + ex.StackTrace.ToString());
			}
		}
		if (mls && HUD_bullets)
			HUD_bullets.text = bulletsIHave.ToString() + " - " + bulletsInTheGun.ToString();

		DrawCrosshair();
	}

	[Header("Crosshair properties")]
	public Texture horizontal_crosshair, vertical_crosshair;
	public Vector2 top_pos_crosshair, bottom_pos_crosshair, left_pos_crosshair, right_pos_crosshair;
	public Vector2 size_crosshair_vertical = new Vector2(1, 1), size_crosshair_horizontal = new Vector2(1, 1);

	// 십자선 드로잉.
	[HideInInspector]
	public Vector2 expandValues_crosshair;
	private float fadeout_value = 1;
	void DrawCrosshair()
	{
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeout_value);

		GUI.DrawTexture(new Rect(vec2(left_pos_crosshair).x + position_x(-expandValues_crosshair.x) + Screen.width / 2, Screen.height / 2 + vec2(left_pos_crosshair).y, vec2(size_crosshair_horizontal).x, vec2(size_crosshair_horizontal).y), vertical_crosshair);//left
		GUI.DrawTexture(new Rect(vec2(right_pos_crosshair).x + position_x(expandValues_crosshair.x) + Screen.width / 2, Screen.height / 2 + vec2(right_pos_crosshair).y, vec2(size_crosshair_horizontal).x, vec2(size_crosshair_horizontal).y), vertical_crosshair);//right

		GUI.DrawTexture(new Rect(vec2(top_pos_crosshair).x + Screen.width / 2, Screen.height / 2 + vec2(top_pos_crosshair).y + position_y(-expandValues_crosshair.y), vec2(size_crosshair_vertical).x, vec2(size_crosshair_vertical).y), horizontal_crosshair);//top
		GUI.DrawTexture(new Rect(vec2(bottom_pos_crosshair).x + Screen.width / 2, Screen.height / 2 + vec2(bottom_pos_crosshair).y + position_y(expandValues_crosshair.y), vec2(size_crosshair_vertical).x, vec2(size_crosshair_vertical).y), horizontal_crosshair);//bottom
	}

	// GUI 이미지의 크기 및 위치 반환
	private float position_x(float var)
	{
		return Screen.width * var / 100;
	}
	private float position_y(float var)
	{
		return Screen.height * var / 100;
	}
	private Vector2 vec2(Vector2 _vec2)
	{
		return new Vector2(Screen.width * _vec2.x / 100, Screen.height * _vec2.y / 100);
	}

	// 현재 애니메이션이 실행 중인지 확인하고 R을 누를 때 재장전 애니메이션을 설정.
	public Animator handsAnimator;
	void Animations()
	{
		if (handsAnimator)
		{
			reloading = handsAnimator.GetCurrentAnimatorStateInfo(0).IsName(reloadAnimationName);

			handsAnimator.SetFloat("walkSpeed", pmS.currentSpeed);
			handsAnimator.SetBool("aiming", Input.GetButton("Fire2"));
			handsAnimator.SetInteger("maxSpeed", pmS.maxSpeed);
			if (Input.GetKeyDown(KeyCode.R) && pmS.maxSpeed < 5 && !reloading && !meeleAttack)
				StartCoroutine("Reload_Animation");
		}
	}

}
