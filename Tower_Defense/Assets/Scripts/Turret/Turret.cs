// 타워 정보 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;  // 타워가 공격할 적.
    private Enemy targetEnemy; // 적의 Enemy Conponent;
    private string enemyTag;   // 공격할 유닛의 태그.

    [Header("Turret")]
    public float range;            // 타워의 사정 거리.
    public float fireRate;         // 타워의 공격 속도.
    public string description;     // 타워의 간단한 설명.
    public float turnSpeed;        // 타워 공격부위의 회전 속도.
    public Transform partToRotate; // 타워에서 적 유닛 발견 시 회전할 부분.
    private float fireCountDown;   // 공격 후 다음 공격까지의 시간.

    [Header("Attack")]
    public GameObject bulletPrefab; // 공격 등장할 탄환.
    public Transform firePoint;     // 공격 탄환이 등장할 위치.
    public GameObject attactEffect; // 공격 효과.
    public AudioSource shootSound;  // 공격 효과음

    [Header("Missile")]
    public static AudioSource MissileSound; // 미사일 명중 효과음.

    [Header("Laser")]
    public bool useLaser;             // Laser 발사 여부.
    public LineRenderer lineRenderer; // 발사될 Laser.
    public int damageOverTime;        // 초당 레이저 공격력.
    public float slowPct;             // Laser 명중 시 감소될 이동속도.
    public GameObject impactEffect;   // Laser 명중 시의 효과.
    //public Light impactLight;         // Laser 명중 시 발생할 조명.
    public GameObject laserSound;     // Laser 명중 시 효과음.

    // 시작 후 0.5초마다 타겟을 지정 및 변경.
    // 미사일 명중 효과음 초기화.
    void Start()
    {
        enemyTag = "Enemy";
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        MissileSound = Resources.Load<AudioSource>("Tower/Missile/MissileSound");
    }

    // 적 유닛과 타워의 거리를 계산 후 공격 목표 설정.
    void UpdateTarget ()
    {
        // Enemy 태그를 가진 유닛 배열을 저장.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity; // 가장 가까운 적 유닛과의 거리.
        GameObject nearestEnemy = null;          // 가장 가까운 적 유닛.

        // 적 유닛의 수 만큼 반복.
        foreach (GameObject enemy in enemies)
        {
            // 적 유닛과 타워와의 거리를 계산.
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // 타워와 가장 가까운 거리의 적 유닛의 정보를 변경. 
            if (shortestDistance > distanceToEnemy)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // 타워의 거리 내에 적 유닛이 존재할 경우.
        if (nearestEnemy != null && shortestDistance <= range)
        {
            // target을 가장 가까운 유닛으로 설정.
            target = nearestEnemy.transform;
            // target의 Enemy Conponent 저장.
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        // 타워의 거리 내에 적 유닛이 존재하지 않을 경우.
        else
        {
            target = null;
        }
    }

    // 타워 방향 설정 및 공격.
    void Update()
    {
        // target이 없을 경우.
        if (target == null)
        {
            // 공격 수단이 Laser인 경우.
            if (useLaser)
            {
                // LineRenderer가 활성화 된 경우.
                if (lineRenderer.enabled)
                {
                    // LineRenderer 비활성화.
                    lineRenderer.enabled = false;
                    //impactLight.enabled = false;
                    impactEffect.SetActive(false);

                    // Laser 명중 효과음 중지.
                    laserSound.SetActive(false);

                }
            }
            // 이후의 Update 함수를 진행하지 않음.
            return;
        }

        // 타워를 회전.
        LockOnTarget();

        // 공격 수단이 Laser인 경우.
        if (useLaser)
        {
            // target을 공격.
            Laser();

            // 레이저 공격 이펙트 활성화.
            impactEffect.SetActive(true);
        }
        // 공격 수단이 Bullet이나 Missile인 경우.
        else
        {
            // 남은 공격 대기 시간이 0일 경우.
            if (fireCountDown <= 0)
            {
                // target을 공격.
                Shoot();
                // 공격 후 공격 대기 시간을 공격 속도에 따라 변경.
                fireCountDown = 1f / fireRate;
            }
            // 1초마다 공격 시간을 감소.
            fireCountDown -= Time.deltaTime;
        }
      
    }

    // 타워의 상단 부위 회전.
    void LockOnTarget()
    {
        // 타워가 target을 보도록 방향을 설정.
        Vector3 dir = target.position - transform.position;

        // 타워의 PartToRatate 개체를 회전.
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 lotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, lotation.y, 0f);
    }

    // 타워의 공격 판정.
    void Shoot()
    {
        // 화면에 탄환 생성, 스크립트 내에 탄환 객체 생성.
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 생성된 탄환을 기반으로 Bullet 객체 생성. 
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        // bullet 변수에 값이 있을 시에 적을 추적.
        if (bullet != null)
            bullet.Seek(target);

        // 화면에 Attack Effect 생성.
        GameObject attack_Effect = Instantiate(attactEffect, firePoint.position, firePoint.rotation);
        Destroy(attack_Effect, 1);

        // 공격 효과음 발생.
        shootSound.Play();
    }

    public static void Missile()
    {
        // 폭발 효과음 발생.
        Destroy(Instantiate(MissileSound), 1);
    }

    // Laser를 사용하여 공격.
    void Laser()
    {
        // Laser를 통한 공격 판정.
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

        // Laser 명중 시 이동속도 저하.
        targetEnemy.Slow(slowPct);

        // LineRenderer가 비활성화 된 경우.
        if (!lineRenderer.enabled)
        {
            // LineRenderer 활성화.
            lineRenderer.enabled = true;
            //impactLight.enabled = true;
        }

        // Laser의 LineRenderer에서 설정한 0번 색을 타워, 1번 색을 target으로 설정.
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        // Laser 명중 시 효과의 방향을 지정.
        Vector3 dir = firePoint.position - target.position;

        // 효과의 위치를 target 위치로 지정.
        impactEffect.transform.position = target.transform.position + dir.normalized;

        // 효과의 방향을 회전.
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);

        // Laser 명중 효과음 발생.
        laserSound.SetActive(true);
    }

    // 타워 공격 범위 시각화.
    void OnDrawGizmosSelected()
    {
        // 타워의 공격 범위를 시작적으로 표현.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
