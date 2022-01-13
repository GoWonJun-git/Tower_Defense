// 라운드 표시 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
    public Text roundText; // 플레이어의 현재 Round 표시 창.

    // 플레이어의 현재 Round 표시창을 규격에 맞춰 변경.
    void Update()
    {
        // 플레이어의 현재 Round 표시창을 규격에 맞춰 변경.
        roundText.text = PlayerStats.Round.ToString();
    }

}
