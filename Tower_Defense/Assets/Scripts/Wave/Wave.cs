// 적 유닛 정보 Script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string name; // 등장할 적 유닛.
    public int count;   // 적 유닛 숫자.
    public float rate;  // 적 재등장 속도.
}

