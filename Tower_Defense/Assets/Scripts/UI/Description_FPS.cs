// �� ����â ��� Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Description_FPS : MonoBehaviour
{
    [Header("Button And Menu")]
    public GameObject Description_Button; // ���� ���� ��ư.
    public GameObject Description_Menu;   // ���� ���� â.

    [Header("Mob")]
    public Text Name;
    public Text Description;
    public Text Health;
    public Text Speed;
    private float Timer = 0f; // ��ư Ȱ��ȭ �ð�.

    void Update()
    {
        // �� ���� ���� ��ư Ȱ��ȭ ���� ����.
        if (Timer > 0f)
            Timer -= Time.deltaTime;
        else
            Description_Button.SetActive(false);
    }

    // �� ���� ��ư Ȱ��ȭ �� ����â ���� ����.
    public void Description_Button_Toggle(string name)
    {
        // Mob Description Button Ȱ��ȭ.
        Description_Button.SetActive(true);

        // ��Ȱ��ȭ Ÿ�̸� ����.
        Timer = 5f;

        // ���� ���� ��ü ����.
        Enemy_FPS tmp = Resources.Load<GameObject>("Monster/" + name).GetComponent<Enemy_FPS>();

        // Menu ���� ����.
        Name.text = "�̸� : " + tmp.name;
        Description.text = tmp.description.Replace("\\n", "\n");
        Health.text = "ü�� : " + tmp.starthealth;
        Speed.text = "�ӵ� : " + tmp.startSpeed;
    }

    // �� ����â Ȱ��ȭ ���� ����.
    public void Mob_Description_Menu_Toggle()
    {
        // ����â Ȱ��ȭ ���� ��ȯ.
        Description_Menu.SetActive(!Description_Menu.activeSelf);

        // ���� â Ȱ��ȭ�� ���� �÷��� ȭ�� ���� ���� ����.
        if (Description_Menu.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

}
