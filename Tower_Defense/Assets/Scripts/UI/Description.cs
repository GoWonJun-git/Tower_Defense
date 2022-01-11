// 설명창 담당 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    [Header("Button")]
    public GameObject mob_Description_Button;   // 몬스터 설명 버튼.
    public GameObject Tower_Description_Button; // 타워 설명 버튼.

    [Header("Menu")]
    public GameObject mob_Description_Menu;   // 몬스터 설명 창.
    public GameObject tower_Description_Menu; // 타워 설명 창.

    [Header("Mob")]
    public Text mob_Name;
    public Text mob_Description;
    public Text mob_Health;
    public Text mob_Speed;
    public Text mob_Gold;
    private float mob_Timer = 0f; // 버튼 활성화 시간.

    [Header("Tower")]
    public GameObject tower_UI_Check; // 버튼 활성화 여부.
    public Text tower_Name;
    public Text tower_Description;
    public Text tower_Rate;
    public Text tower_Range;
    public Text tower_Damage;
    public Image tower_Image;

    void Update()
    {
        // 적 유닛 설명 버튼 활성화 여부 관리.
        if (mob_Timer > 0f)
            mob_Timer -= Time.deltaTime;
        else
            mob_Description_Button.SetActive(false);

        // Node UI 활성화 여부와 타워 설명 버튼 활성화 여부 연동.
        Tower_Description_Button.SetActive(tower_UI_Check.activeSelf);
    }

    // 몹 설명 버튼 활성화 및 설명창 정보 갱신.
    public void Mob_Description_Button_Toggle(string name)
    {
        // Mob Description Button 활성화.
        mob_Description_Button.SetActive(true);

        // 비활성화 타이머 세팅.
        mob_Timer = 3f;

        // 몬스터 정보 객체 생성.
        Enemy tmp = Resources.Load<GameObject>("Monster/" + name).GetComponent<Enemy>();

        // Menu 정보 갱신.
        mob_Name.text = "이름 : " + tmp.name;
        mob_Description.text = tmp.description.Replace("\\n", "\n");
        mob_Health.text = "체력 : " + tmp.starthealth;
        mob_Speed.text = "속도 : " + tmp.startSpeed;
        mob_Gold.text = "골드 : " + tmp.gold;
    }

    // 몹 설명창 활성화 여부 변경.
    public void Mob_Description_Menu_Toggle()
    {
        // 몹 설명창 활성화 여부 전환.
        mob_Description_Menu.SetActive(!mob_Description_Menu.activeSelf);

        // 설명 창 활성화에 따른 플레이 화면 정지 여부 관리.
        if (mob_Description_Menu.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    // 타워 설명창 정보 갱신.
    public void Tower_Description_Button_Toggle(GameObject tower)
    {
        // 타워 정보 객체 생성.
        Turret tmp_Turret = tower.GetComponent<Turret>();

        // Menu 정보 갱신.
        tower_Name.text = "이름 : " + tmp_Turret.name.Substring(0, tower.name.Length-7).Replace("_", " ");
        tower_Description.text = tmp_Turret.description.Replace("\\n", "\n");
        tower_Rate.text = "속도 : " + tmp_Turret.fireRate;
        tower_Range.text = "범위 : " + tmp_Turret.range;

        // 타워 종류에 맞춰 공격력 정보 갱신.
        if (tower.name.Substring(0, 5) != "Laser")
            tower_Damage.text = "공격력 : " + tmp_Turret.bulletPrefab.GetComponent<Bullet>().damage;
        else
            tower_Damage.text = "공격력 : " + tmp_Turret.damageOverTime;

        // Image 객체 생성 후 Image 변경.
        Image Button_Image = Tower_Description_Button.GetComponent<Image>();
        Button_Image.sprite = Resources.Load<Sprite>("Tower/UI/" + tmp_Turret.name.Split('_')[0]);
        tower_Image.sprite = Button_Image.sprite;
    }

    // 타워 설명창 활성화 여부 변경.
    public void Tower_Description_Menu_Toggle()
    {
        // 타워 설명창 활성화 여부 전환.
        tower_Description_Menu.SetActive(!tower_Description_Menu.activeSelf);

        // 설명 창 활성화에 따른 플레이 화면 정지 여부 관리.
        if (tower_Description_Menu.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

}
