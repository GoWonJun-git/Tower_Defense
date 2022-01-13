// �� ���� �� ������ ���� ����Ʈ ��� Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_End : MonoBehaviour
{
    float timer;              // ����Ʈ Ȱ�� �ð�.
    public GameObject effect; // Ȱ��ȭ�� ����Ʈ.

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            effect.SetActive(false);
    }

    // ����Ʈ ����.
    public void StartEffect()
    {
        effect.SetActive(true);
        timer = 1f;
    }

    // ����Ʈ �Ҹ�.
    public void EndEffect()
    {
        effect.SetActive(true);
        timer = 1f;
    }

}
