// 몹 설명창 담당 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Description_FPS : MonoBehaviour
{
    [Header("Button And Menu")]
    public GameObject Description_Button; // 몬스터 설명 버튼.
    public GameObject Description_Menu;   // 몬스터 설명 창.

    [Header("Mob")]
    public Text Name;
    public Text Description;
    public Text Health;
    public Text Speed;
    private float Timer = 0f; // 버튼 활성화 시간.

    void Update()
    {
        // 적 유닛 설명 버튼 활성화 여부 관리.
        if (Timer > 0f)
            Timer -= Time.deltaTime;
        else
            Description_Button.SetActive(false);
    }

    // 몹 설명 버튼 활성화 및 설명창 정보 갱신.
    public void Description_Button_Toggle(string name)
    {
        // Mob Description Button 활성화.
        Description_Button.SetActive(true);

        // 비활성화 타이머 세팅.
        Timer = 5f;

        // 몬스터 정보 객체 생성.
        Enemy_FPS tmp = Resources.Load<GameObject>("Monster/" + name).GetComponent<Enemy_FPS>();

        // Menu 정보 갱신.
        Name.text = "이름 : " + tmp.name;
        Description.text = tmp.description.Replace("\\n", "\n");
        Health.text = "체력 : " + tmp.starthealth;
        Speed.text = "속도 : " + tmp.startSpeed;
    }

    // 몹 설명창 활성화 여부 변경.
    public void Mob_Description_Menu_Toggle()
    {
        // 설명창 활성화 여부 전환.
        Description_Menu.SetActive(!Description_Menu.activeSelf);

        // 설명 창 활성화에 따른 플레이 화면 정지 여부 관리.
        if (Description_Menu.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

}
