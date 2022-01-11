// 체력 표시 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveUI : MonoBehaviour
{
    public Text liveText; // 플레이어의 현재 Live 표시 창.

    // 플레이어의 현재 Live 표시창을 규격에 맞춰 변경.
    void Update()
    {
        // 플레이어의 현재 Live 표시창을 규격에 맞춰 변경.
        liveText.text = PlayerStats.Live.ToString();
    }

}