// 타워의 발사체 담당 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target; // 탄환이 공격할 대상.
    private Vector3 dir;      // 탄환의 이동 방향.
    private float dis;        // 탄환과 target과의 거리.

    public int damage;  // 탄환의 공격력.
    public float speed; // 탄환의 이동 속도.
    public GameObject impactEffect; // 탄환 적중 시 발생할 시각적 효과.

    [Header("Missile")]
    public float explosionRadius; // 미사일 폭발 범위.

    // 탄환 생성 후 2초가 지나면 탄환 제거.
    public void Start()
    {
        Destroy(gameObject, 2);
    }

    // 탄환 방향 설정.
    public void Seek(Transform _target)
    {
        // 탄환이 추적할 적을 결정.
        target = _target;

        // 탄환이 발사될 방향 설정.
        dir = target.position - transform.position;
    }

    // 탄환 이동.
    void Update()
    {
        // 지정된 target이 없을 시 탄환 제거 후 Update 종료.
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        // 탄환이 이동할 거리 설정.
        float distanceThisFrame = speed * Time.deltaTime;

        // 탄환을 target 방향으로 이동.
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

        // 탄환과 target과의 거리 계산.
        dis = Vector3.Distance(gameObject.transform.position, target.position);

        // 거리가 5이하일 경우 명중 판정.
        if (dis < 5)
            Hit_Target();
    }

    // 탄환 명중 판정.
    public void Hit_Target() 
    {
        // 시각적 효과 발생.
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);

        // 폭발 범위가 있을 경우 폭발 함수 호출.
        if (explosionRadius > 0f)
            explode();
        // 폭발 범위가 없을 경우 데미지 판정.
        else
            Damage(target, damage);

        // 탄환 제거.
        Destroy(gameObject);
    }

    // 미사일 공격으로 인한 폭발 판정.
    public void explode()
    {
        // 구 형태의 범위를 제공하고 해당 범위 내의 모든 객체 정보 저장.
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);

        // 저장된 객체 수만큼 반복.
        foreach (Collider collider in colliders)
        {
            // 태그가 Enemy인 경우.
            if (collider.tag == "Enemy")
                // 데미지 판정.
                Damage(collider.transform, damage);
        }

        // 미사일 명중 효과음 발생.
        Turret.Missile();
    }

    // 적 유닛에게 데미지 판정.
    public void Damage (Transform enemy, int _datage)
    {
        // 적 유닛의 Enemy Script를 객체화.
        Enemy e = enemy.GetComponent<Enemy>();

        // 처치 판정의 적이 존재할 경우.
        if (e != null)
            // 적 유닛에 피해를 줌.
            e.TakeDamage(_datage);
    }

    // 폭발 범위 색상 설정 및 시각화.
    void OnDrawGizmosSelected()
    {
        // 폭발 범위 색상 설정.
        Gizmos.color = Color.red;

        // 폭발 범위 시각화.
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
