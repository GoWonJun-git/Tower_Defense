// 적 유닛 담당 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed;   // 유닛의 초기 이동 속도.
    public float starthealth;  // 유닛의 초기 체력.
    public int gold;           // 유닛의 드랍 Money.
    public string description; // 유닛의 간단한 설명.
    public Animator ani;       // 유닛의 Animation 변수.
    public bool isDead;        // 유닛의 사망 여부.

    private Transform targer;   // 유닛이 이동할 위치.
    private float speed;        // 유닛의 현재 이동 속도.
    private float health;       // 유닛의 현재 체력.
    private int wavePoinsIndex; // targer의 이동 위치 순서.
    private Start_End endPoint; // 유닛의 목표에 도착시 활성화 될 이펙트.

    // 해당 Script의 사용 객체 초기화.
    void Start()
    {
        // 유닛이 이동할 위치 지정.
        targer = WayPoints.points[0];

        // 유닛의 이동 위치 순서 지정.
        wavePoinsIndex = 0;

        // 유닛의 이동 속도 지정.
        speed = startSpeed;

        // 유닛의 체력 지정.
        health = starthealth;

        // 유닛의 목표에 도착시 활성화 될 이펙트 객체 지정.
        endPoint = GameObject.Find("END").GetComponent<Start_End>();
    }

    // 유닛 이동.
    void Update()
    {
        // 유닛 사망 시 행동 중지.
        if (isDead)
            return;

        // 유닛의 이동할 위치로의 방향 설정.
        Vector3 dir = targer.position - transform.position;

        // 유닛 이동.
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        transform.LookAt(targer);

        // 유닛이 targer 위치에 도달한 경우 다음 target으로 이동.
        if (Vector3.Distance(transform.position, targer.position) < 1f)
            GetNextWayPoints();

        /* 유닛의 이동 속도를 초기 속도로 변경.
         * Laser 공격 이후 이동 속도 원상복구 용도.
         * Freezing 스킬이 사용되지 않을 경우에만 작동. */
        if (GameObject.Find("Freezing(Clone)") == null)
            speed = startSpeed;
    }

    // 유닛이 이동할 target을 설정.
    void GetNextWayPoints()
    {
        // 유닛이 EndPoint에 도달한 경우 유닛 제거.
        if (wavePoinsIndex == WayPoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        // 다음 target 위치를 다음 Point로 설정.
        wavePoinsIndex++;
        targer = WayPoints.points[wavePoinsIndex];
    }

    // 유닛이 EndPoint에 도달한 경우.
    void EndPath()
    {
        // End Point 도착 이펙트 활성화.
        endPoint.EndEffect();

        // 유닛 제거.
        Destroy(gameObject);

        // 라이프 감소.
        PlayerStats.Live--;

        // 맵에 남은 유닛 수 감소.
        WaveSpawner.EnemiesAlive--;
    }

    // 유닛 이동 속도 감소.
    public void Slow(float pct)
    {
        // Enemy Speed 감소.
        speed = startSpeed * (1f - pct);
    }

    // 유닛의 피해 판정.
    public void TakeDamage(float amount)
    {
        // 유닛의 체력에서 피해량을 감소.
        health -= amount;

        // 유닛의 체력이 0보다 낮고, 사망 판정이 아닌 경우.
        if (health <= 0 && !isDead)
            // 사망 판정.
            Die();
    } 

    // 유닛의 사망 판정.
    void Die()
    {
        // 유닛 사망 여부 변경.
        isDead = true;
        gameObject.tag = "Untagged";

        // 유닛 사망 Effect 발생.
        ani.SetBool("Die", true);

        // 유닛의 사망 금액을 보유 Money에 추가.
        PlayerStats.Money += gold;

        // 맵에 남은 유닛 수 감소.
        WaveSpawner.EnemiesAlive--;

        // 유닛 제거.
        Destroy(gameObject, 1);
    }

}
