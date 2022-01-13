// Game Over 및 Clear 결정 Script.
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOvers;    // Game Over 판단.
    public GameObject gameOverUI;      // Game Over 시 등장할 UI.
    public GameObject completeLevelUI; // Game Clear 시 등장할 UI.

    // Game Start 시 GameOver 판단 변수 초기화.
    void Start()
    {
        GameIsOvers = false;
    }

    // Game Over 상황을 판단.
    void Update()
    {
        // Game Over 상황인 경우.
        if (GameIsOvers)
            return;

        // Live가 0이 된 경우.
        if (PlayerStats.Live == 0)
        {
            EndGame();
        }

        // 바로 중지 키 입력 시.
        if (Input.GetKeyDown("e"))
        {
            EndGame();
        }
    }

    // Game Over 판정. 
    void EndGame()
    {
        // Game Over 판단 변수를 변경.
        GameIsOvers = true;

        // Game Over UI 활성화.
        gameOverUI.SetActive(true);
    }

    // Game Clear 판정.
    public void WinLevel()
    {
        // Game Over 판단 변수를 변경.
        GameIsOvers = true;

        // Game Clear UI 활성화.
        completeLevelUI.SetActive(true);
    }

}
