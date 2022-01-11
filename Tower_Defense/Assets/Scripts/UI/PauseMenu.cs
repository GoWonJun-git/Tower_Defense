// 일시정지 UI 관련 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;         // Pause Menu.
    public SceneFader sceneFader; // Scene Load 효과 파일.
    public string menuSceneName;  // Menu 버튼 클릭 시 불러올 Scene 이름.

    // Pause Menu 활성 여부 반전. 
    public void Toggle()
    {
        // Pause Menu 활성 여부 반전.
        ui.SetActive(!ui.activeSelf);

        // Pause Menu가 활성화 될 시 게임 시간 정지.
        if (ui.activeSelf)
            Time.timeScale = 0f;
        // Pause Menu가 비활성화 될 시 게임 시간 정지 해제.
        else
            Time.timeScale = 1f;
    }

    // 게임 재시작.
    public void Retry()
    {
        // Pause Menu 비활성화.
        Toggle();

        // 게임을 재시작.
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    // Menu로 이동.
    public void Menu()
    {
        // Pause Menu 비활성화. 
        Toggle();

        // Menu로 이동.
        sceneFader.FadeTo(menuSceneName);
    }

}
