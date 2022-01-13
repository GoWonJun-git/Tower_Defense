/* Clear Round 표시 Script.
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundsServived : MonoBehaviour
{
    public Text roundsText; // Game Over Round 표시 창.

    // AnimateText 함수를 호출하여 Text 변경 Animation 수행.
    void OnEnable()
    {
        // AnimateText 함수를 호출하여 Text 변경 Animation 수행.
        StartCoroutine(AnimateText());
    }

    //Text를 Clear Round만큼 상승.
    IEnumerator AnimateText()
    {
        // Text의 시작을 0으로 설정.
        roundsText.text = "0";
        int round = 0;

        // Clear Round만큼 반복.
        while (round < PlayerStats.Round)
        {
            // Text를 Clear Round만큼 상승.
            round++;
            roundsText.text = round.ToString();

            // 0.5초 대기 후 다음 과정 진행.
            yield return new WaitForSeconds(.05f);
        }
    }

}
*/