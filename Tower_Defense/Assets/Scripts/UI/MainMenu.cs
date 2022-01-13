// Main Manu Sence 관련 Script.
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;    // Load 할 Scene 이름.
    public SceneFader sceneFader; // Scene Load 효과 파일.

    // Play 버튼 클릭 시 Scene 이동.
    public void Play()
    {
        // Scene 이동.
        FindObjectOfType<SceneFader>().FadeTo(levelToLoad);
    }

    // Quit 버튼 클릭 시 게임 종료.
    public void Quit()
    {
        // 게임 종료.
        Application.Quit();
    }

}
