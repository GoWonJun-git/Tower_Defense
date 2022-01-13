// FPS 모드에서 탄환에 관한 Script.
using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	RaycastHit hit;
	public float maxDistance = 1000000; // 탄환의 최대 이동거리.
	public GameObject decalHitWall;     // 탄환이 벽에 충돌 시 발생할 효과.
	public GameObject bloodEffect;      // 탄환이 적과 충동 시 발생할 효과.
	public float floatInfrontOfWall;    // 렌더 문제 해결을 위해 사용되는 변수.
	public LayerMask ignoreLayer;

	/* 탄환이 해당 태그를 검색하는 레이캐스트를 생성.
	 * 레이캐스트는 해당 태그의 효과를 생성. */
	void Update()
	{
		if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
		{
			if (decalHitWall)
			{
				if (hit.transform.tag == "Map")
				{
					// 명중 효과.
					Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
					Destroy(gameObject);
				}
				if (hit.transform.tag == "Enemy")
				{
					// 출혈 효과.
					Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));

					Enemy_FPS e = hit.transform.GetComponent<Enemy_FPS>();
					// 처치 판정의 적이 존재할 경우.
					if (e != null)
					{
						// 적 유닛에 피해를 줌.
						e.TakeDamage();
					}

					Destroy(gameObject);
				}
			}
			Destroy(gameObject);
		}
		Destroy(gameObject, 0.1f);
	}

}