// 돈, 목숨, 라운드 정보 관리 Script.
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;


public class PlayerStats : MonoBehaviour
{
    public static int Money;       // 플레이어의 보유 머니.
    public int startMoney = 500;   // 게임 시작 머니.

    public static int Live;        // 플레이어의 보유 라이프.
    public int startLive = 20;     // 게임 시작 라이프.

    public static int Round;       // 플레이어의 현재 라운드 수.
    public static int clearRounds; // 플레이어가 클리어 한 라운드 수.

    // 플레이어의 머니, 체력, 현재 및 클리어 라운드 수 초기화.
    void Start()
    {
        Money = startMoney;
        Live = startLive;
        Round = 0;
        clearRounds = 0;
    }

}
