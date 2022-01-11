// ����â ��� Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    [Header("Button")]
    public GameObject mob_Description_Button;   // ���� ���� ��ư.
    public GameObject Tower_Description_Button; // Ÿ�� ���� ��ư.

    [Header("Menu")]
    public GameObject mob_Description_Menu;   // ���� ���� â.
    public GameObject tower_Description_Menu; // Ÿ�� ���� â.

    [Header("Mob")]
    public Text mob_Name;
    public Text mob_Description;
    public Text mob_Health;
    public Text mob_Speed;
    public Text mob_Gold;
    private float mob_Timer = 0f; // ��ư Ȱ��ȭ �ð�.

    [Header("Tower")]
    public GameObject tower_UI_Check; // ��ư Ȱ��ȭ ����.
    public Text tower_Name;
    public Text tower_Description;
    public Text tower_Rate;
    public Text tower_Range;
    public Text tower_Damage;
    public Image tower_Image;

    void Update()
    {
        // �� ���� ���� ��ư Ȱ��ȭ ���� ����.
        if (mob_Timer > 0f)
            mob_Timer -= Time.deltaTime;
        else
            mob_Description_Button.SetActive(false);

        // Node UI Ȱ��ȭ ���ο� Ÿ�� ���� ��ư Ȱ��ȭ ���� ����.
        Tower_Description_Button.SetActive(tower_UI_Check.activeSelf);
    }

    // �� ���� ��ư Ȱ��ȭ �� ����â ���� ����.
    public void Mob_Description_Button_Toggle(string name)
    {
        // Mob Description Button Ȱ��ȭ.
        mob_Description_Button.SetActive(true);

        // ��Ȱ��ȭ Ÿ�̸� ����.
        mob_Timer = 3f;

        // ���� ���� ��ü ����.
        Enemy tmp = Resources.Load<GameObject>("Monster/" + name).GetComponent<Enemy>();

        // Menu ���� ����.
        mob_Name.text = "�̸� : " + tmp.name;
        mob_Description.text = tmp.description.Replace("\\n", "\n");
        mob_Health.text = "ü�� : " + tmp.starthealth;
        mob_Speed.text = "�ӵ� : " + tmp.startSpeed;
        mob_Gold.text = "��� : " + tmp.gold;
    }

    // �� ����â Ȱ��ȭ ���� ����.
    public void Mob_Description_Menu_Toggle()
    {
        // �� ����â Ȱ��ȭ ���� ��ȯ.
        mob_Description_Menu.SetActive(!mob_Description_Menu.activeSelf);

        // ���� â Ȱ��ȭ�� ���� �÷��� ȭ�� ���� ���� ����.
        if (mob_Description_Menu.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    // Ÿ�� ����â ���� ����.
    public void Tower_Description_Button_Toggle(GameObject tower)
    {
        // Ÿ�� ���� ��ü ����.
        Turret tmp_Turret = tower.GetComponent<Turret>();

        // Menu ���� ����.
        tower_Name.text = "�̸� : " + tmp_Turret.name.Substring(0, tower.name.Length-7).Replace("_", " ");
        tower_Description.text = tmp_Turret.description.Replace("\\n", "\n");
        tower_Rate.text = "�ӵ� : " + tmp_Turret.fireRate;
        tower_Range.text = "���� : " + tmp_Turret.range;

        // Ÿ�� ������ ���� ���ݷ� ���� ����.
        if (tower.name.Substring(0, 5) != "Laser")
            tower_Damage.text = "���ݷ� : " + tmp_Turret.bulletPrefab.GetComponent<Bullet>().damage;
        else
            tower_Damage.text = "���ݷ� : " + tmp_Turret.damageOverTime;

        // Image ��ü ���� �� Image ����.
        Image Button_Image = Tower_Description_Button.GetComponent<Image>();
        Button_Image.sprite = Resources.Load<Sprite>("Tower/UI/" + tmp_Turret.name.Split('_')[0]);
        tower_Image.sprite = Button_Image.sprite;
    }

    // Ÿ�� ����â Ȱ��ȭ ���� ����.
    public void Tower_Description_Menu_Toggle()
    {
        // Ÿ�� ����â Ȱ��ȭ ���� ��ȯ.
        tower_Description_Menu.SetActive(!tower_Description_Menu.activeSelf);

        // ���� â Ȱ��ȭ�� ���� �÷��� ȭ�� ���� ���� ����.
        if (tower_Description_Menu.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

}
