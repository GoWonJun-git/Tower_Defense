// FPS 모드에서 효과에 관한 Script
using System.Collections;
using UnityEngine;

public class DestroyAfterTimeParticle : MonoBehaviour
{
	public float timeToDestroy = 0.8f; // 효과 발생 후 제거까지의 시간.

	// 효과 발생 후 일정시간 이후 제거.
	void Start()
	{
		Destroy(gameObject, timeToDestroy);
	}

}
