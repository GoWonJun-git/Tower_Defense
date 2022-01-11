// Scene 이동 Script.
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image img;            // Scene 변경 시의 Image.
    public AnimationCurve curve; // 이미지 변화 곡선.

    // 시작 시 FadeIn 함수 호출.
    void Start()
    {
        // FadeIn 함수 호출.
        StartCoroutine(FadeIn());
    }

    // Scene 활성화.
    IEnumerator FadeIn()
    {
        float t = 1f; // 프레임 변경에 걸릴 시간.

        // 프레임 변경 시간만큼 작동.
        while (t > 0f)
        {
            // 1초마다 1씩 감소.
            t -= Time.deltaTime;

            // 변수 t값을 Curve에 맞춰 변경.
            float a = curve.Evaluate(t);

            // Image의 농도를 변경.
            img.color = new Color(0f, 0f, 0f, a);

            // 다음 프레임으로 이동.
            yield return 0;
        }
    }

    // Scene 변경.
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    // Scene 변경 후 처리.
    IEnumerator FadeOut(string scene)
    {
        float t = 0f; // 프레임 변경에 걸릴 시간.

        // 프레임 변경 시간만큼 작동.
        while (t < 1f)
        {
            // 1초마다 1씩 증가.
            t += Time.deltaTime;

            // 변수 t값을 Curve에 맞춰 변경.
            float a = curve.Evaluate(t);

            // Image의 농도를 변경.
            img.color = new Color(0f, 0f, 0f, a);

            // 다음 프레임으로 이동.
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }

}
