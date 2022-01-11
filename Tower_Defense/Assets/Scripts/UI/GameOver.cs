// Game Over 담당 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public SceneFader sceneFader; // Scene Load 효과 파일.
    public string menuSceneName;  // Menu 버튼 클릭 시 불러올 Scene 이름.

    // Retry Button 클릭 시 게임을 재시작.
    public void Retry()
    {
        // 게임을 재시작.
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    // Menu Button 클릭 시 Menu로 이동.
    public void Menu()
    {
        // Menu로 이동.
        sceneFader.FadeTo(menuSceneName);
    }

}
