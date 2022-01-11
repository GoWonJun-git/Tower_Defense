// 적 생성 및 도착점 도달 이팩트 담당 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_End : MonoBehaviour
{
    float timer;              // 이펙트 활성 시간.
    public GameObject effect; // 활성화할 이펙트.

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            effect.SetActive(false);
    }

    // 이펙트 생성.
    public void StartEffect()
    {
        effect.SetActive(true);
        timer = 1f;
    }

    // 이펙트 소멸.
    public void EndEffect()
    {
        effect.SetActive(true);
        timer = 1f;
    }

}
