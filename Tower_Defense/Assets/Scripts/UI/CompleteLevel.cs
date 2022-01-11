// Level Clear 담당 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteLevel : MonoBehaviour
{
    public SceneFader sceneFader; // Scene Load 효과 파일.
    public string menuSceneName;  // Menu 버튼 클릭 시 불러올 Scene 이름.
    public string nextLevel;      // Game Clear 시 다음 난이도의 Scene 이름.
    public int levelToUnlock;     // Game Clear 시 클릭 가능한 난이도.

    // Next Level 클릭 판정.
    public void Continue()
    {
        // Clear 난이도 상승.
        if (PlayerPrefs.GetInt("levelReached") < levelToUnlock)
            PlayerPrefs.SetInt("levelReached", levelToUnlock);

        // 다음 난이도로 이동.
        sceneFader.FadeTo(nextLevel);
    }

    // Menu Button 클릭 판정.
    public void Menu()
    {
        // Clear 난이도 상승.
        if (PlayerPrefs.GetInt("levelReached") < levelToUnlock)
            PlayerPrefs.SetInt("levelReached", levelToUnlock);

        // Menu로 이동.
        sceneFader.FadeTo(menuSceneName);
    }

}
