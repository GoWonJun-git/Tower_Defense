// 돈 표시 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText; // 플레이어의 현재 머니.

    // 플레이어의 현재 머니를 규격에 맞춰 변경.
    void Update()
    {
        moneyText.text = PlayerStats.Money.ToString();
    }

}
