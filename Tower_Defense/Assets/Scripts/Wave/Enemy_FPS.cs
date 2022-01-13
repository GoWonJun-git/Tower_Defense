// FPS 모드 적 유닛 담당 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FPS : MonoBehaviour
{
    public float startSpeed;   // 유닛의 초기 이동 속도.
    public float starthealth;  // 유닛의 초기 체력.
    private float health;      // 유닛의 현재 체력.
    public string description; // 유닛의 간단한 설명.
    public Animator ani;       // 유닛의 Animation 변수.
    public bool isDead;        // 유닛의 사망 여부.

    private GameObject targer; // 유닛이 이동할 위치.
    private float speed;       // 유닛의 현재 이동 속도.
    private float attackTimer; // 적 유닛 공격 쿨타임.

    // 해당 Script의 사용 객체 초기화.
    void Awake()
    {
        // 적의 목표 설정.
        targer = GameObject.FindGameObjectWithTag("Player");

        // 유닛의 이동 속도 지정.
        speed = startSpeed;

        // 유닛의 체력 지정.
        health = starthealth;

        // 적 유닛 공격 쿨타임 지정.
        attackTimer = 0f;
    }

    // 유닛 이동.
    void Update()
    {
        // 유닛 사망 시 행동 중지.
        if (isDead)
            return;

        // 유닛이 targer 위치에 도달한 경우 공격 모션 실행.
        if (Vector3.Distance(transform.position, targer.transform.position) < 5f)
        {
            transform.LookAt(targer.transform.position);
            Attack();
        }
        // 이동.
        else
        {
            // 이동 애니메이션으로 변경.
            ani.SetBool("Attack", false);

            // 유닛의 이동할 위치로의 방향 설정.
            Vector3 dir = targer.transform.position - transform.position;
            dir.y = 0;

            // 유닛 이동.
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            transform.LookAt(targer.transform.position);
        }
    }

    // 유닛의 공격 판정.
    public void Attack()
    {
        // 공격 애니메이션으로 변경.
        ani.SetBool("Attack", true);

        // 공격 쿨타임 감소.
        attackTimer -= Time.deltaTime;

        // 공격 실행.
        if(attackTimer < 0)
        {
            attackTimer = 1f;
            PlayerStats.Live--;
        }
    }

    // 유닛의 피해 판정.
    public void TakeDamage()
    {
        // 유닛의 체력에서 피해량을 감소.
        health -= 1;

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

        // 유닛 태그 변경.
        gameObject.tag = "Untagged";

        // 유닛 사망 Effect 발생.
        ani.SetBool("Die", true);

        // 맵에 남은 유닛 수 감소.
        WaveSpawner_FPS.EnemiesAlive--;

        // 유닛 제거.
        Destroy(gameObject, 1);
    }

}