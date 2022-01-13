// Skill 사용 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject Explosion; // 화염 공격 스킬.
    public GameObject Freezing;  // 냉기 공격 스킬.

    // Explosion 스킬 사용.
    public void SelectExplosion()
    {
        // 스킬 객체 생성 3초 후 제거.
        GameObject explosion = Instantiate(Explosion);
        Destroy(explosion, 3);

        // 구 형태의 범위를 제공하고 해당 범위 내의 모든 객체 정보 저장.
        Collider[] colliders = Physics.OverlapSphere(Explosion.transform.position, 5000);

        // 저장된 객체 수만큼 반복.
        foreach (Collider collider in colliders)
        {
            // 태그가 Enemy인 경우.
            if (collider.tag == "Enemy")
            {
                // 데미지 판정.
                Enemy e = collider.GetComponent<Enemy>();
                e.TakeDamage(1000);
            }
        }

        // 스킬 사용 비용만큼 재화를 감소.
        PlayerStats.Money -= 30;
    }

    // Freezing 스킬 사용.
    public void SelectFreezing()
    {
        // 스킬 객체 생성 3초 후 제거.
        GameObject freezing = Instantiate(Freezing);
        Destroy(freezing, 3);

        // 구 형태의 범위를 제공하고 해당 범위 내의 모든 객체 정보 저장.
        Collider[] colliders = Physics.OverlapSphere(Freezing.transform.position, 5000);

        // 저장된 객체 수만큼 반복.
        foreach (Collider collider in colliders)
        {
            // 태그가 Enemy인 경우.
            if (collider.tag == "Enemy")
            {
                // Slow 판정.
                Enemy e = collider.GetComponent<Enemy>();
                e.Slow(0.6f);
            }
        }

        // 스킬 사용 비용만큼 재화를 감소.
        PlayerStats.Money -= 50;
    }

}
