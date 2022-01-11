// FPS ��� �� ���� ��� Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FPS : MonoBehaviour
{
    public float startSpeed;   // ������ �ʱ� �̵� �ӵ�.
    public float starthealth;  // ������ �ʱ� ü��.
    private float health;      // ������ ���� ü��.
    public string description; // ������ ������ ����.
    public Animator ani;       // ������ Animation ����.
    public bool isDead;        // ������ ��� ����.

    private GameObject targer; // ������ �̵��� ��ġ.
    private float speed;       // ������ ���� �̵� �ӵ�.
    private float attackTimer; // �� ���� ���� ��Ÿ��.

    // �ش� Script�� ��� ��ü �ʱ�ȭ.
    void Awake()
    {
        // ���� ��ǥ ����.
        targer = GameObject.FindGameObjectWithTag("Player");

        // ������ �̵� �ӵ� ����.
        speed = startSpeed;

        // ������ ü�� ����.
        health = starthealth;

        // �� ���� ���� ��Ÿ�� ����.
        attackTimer = 0f;
    }

    // ���� �̵�.
    void Update()
    {
        // ���� ��� �� �ൿ ����.
        if (isDead)
            return;

        // ������ targer ��ġ�� ������ ��� ���� ��� ����.
        if (Vector3.Distance(transform.position, targer.transform.position) < 5f)
        {
            transform.LookAt(targer.transform.position);
            Attack();
        }
        // �̵�.
        else
        {
            // �̵� �ִϸ��̼����� ����.
            ani.SetBool("Attack", false);

            // ������ �̵��� ��ġ���� ���� ����.
            Vector3 dir = targer.transform.position - transform.position;
            dir.y = 0;

            // ���� �̵�.
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            transform.LookAt(targer.transform.position);
        }
    }

    // ������ ���� ����.
    public void Attack()
    {
        // ���� �ִϸ��̼����� ����.
        ani.SetBool("Attack", true);

        // ���� ��Ÿ�� ����.
        attackTimer -= Time.deltaTime;

        // ���� ����.
        if(attackTimer < 0)
        {
            attackTimer = 1f;
            PlayerStats.Live--;
        }
    }

    // ������ ���� ����.
    public void TakeDamage()
    {
        // ������ ü�¿��� ���ط��� ����.
        health -= 1;

        // ������ ü���� 0���� ����, ��� ������ �ƴ� ���.
        if (health <= 0 && !isDead)
            // ��� ����.
            Die();
    }

    // ������ ��� ����.
    void Die()
    {
        // ���� ��� ���� ����.
        isDead = true;

        // ���� �±� ����.
        gameObject.tag = "Untagged";

        // ���� ��� Effect �߻�.
        ani.SetBool("Die", true);

        // �ʿ� ���� ���� �� ����.
        WaveSpawner_FPS.EnemiesAlive--;

        // ���� ����.
        Destroy(gameObject, 1);
    }

}