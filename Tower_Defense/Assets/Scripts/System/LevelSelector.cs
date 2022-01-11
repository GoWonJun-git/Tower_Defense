// Level 선택 Script.
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;      // Scene Load 효과 파일.
    public Button[] levelButtons; // Button 모음.

    // 클리어 한 Level 수준 호출.
    void Start()
    {
        // 클리어 한 Level 수준으로 변수 초기화.
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        // levelButtons Size만큼 반복.
        for (int i = 0; i < levelButtons.Length; i++)
        {
            // 클리어 Level을 넘어가는 경우.
            if (i + 1 > levelReached)
            {
                // 상호작용 기능 비활성화.
                levelButtons[i].interactable = false;
            }
        }
    }

    // Level Button 클릭 시 해당 Level로 이동. 
    public void Select(string levelName)
    {
        // 해당 Level로 이동. 
        fader.FadeTo(levelName);
    }

}
