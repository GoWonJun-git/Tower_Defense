// 적 유닛의 이동 경로에 관한 Script.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPoints : MonoBehaviour
{
    public static Transform[] points; // 적 유닛의 이동 Point 배열.

    // 적 유닛의 이동 Point 지정.
    void Awake()
    {
        // 이동 Point의 갯수로 배열 초기화.
        points = new Transform[transform.childCount];

        // 각 Point의 위치를 배열에 저장.
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

}
