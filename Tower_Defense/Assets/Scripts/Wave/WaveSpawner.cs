// Wave에 따른 적 유닛 생성 관련 Script.
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;            // 적 유닛 Prefeb.
    public float timeBetweenWaves;  // Wave 시작 후 다음 Wave까지의 시간.
    public GameManager gameManager; // GameManager 파일 객체.
    public Description description; // Description 파일 객체.

    public GameObject startPoind; // 적 유닛 생성 위치.
    private Start_End start;      // 적 유닛 생성 이펙트.

    public static int EnemiesAlive; // 맵에 남은 Enemy 수.
    public static int waveIndex;    // Wave Index.
    private float countDown ;       // 다음 Wave까지의 남은 시간.

    // 해당 Script의 사용 객체 초기화.
    void Start()
    {
        // 맵에 남은 적 유닛 수 초기화.
        EnemiesAlive = 0;

        // Wave 횟수 초기화.
        waveIndex = 0;

        // 최초 Wave까지의 타이머 세팅.
        countDown = 3f;

        // Wave Clear 시 다음 Wave까지의 타이머 세팅.
        timeBetweenWaves = 2;

        // Enemy 생성 위치 초기화.
        start = startPoind.GetComponent<Start_End>();
    }

    // Wave 관리.
    void Update()
    {
        // 맵에 남은 Enemy가 있을 경우 함수 중지.
        if (EnemiesAlive > 0)
            return;

        // countDown이 0이 될 경우.
        if (countDown <= 0)
        {
            // 다음 Wave 수행.
            StartCoroutine(SpawnWave());

            // countDown을 2초로 변경.
            countDown = timeBetweenWaves;

            // 함수 중지.
            return;
        }

        // 1초마다 countDown을 1씩 감소.
        countDown -= Time.deltaTime;

        // 소수점 제외.
        countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);

        // 해당 Level의 모든 Wave 클리어 시.
        if (waveIndex == waves.Length)
        {
            // Game Clear 함수 호출.
            gameManager.WinLevel();

            // 해당 게임 중지.
            enabled = false;
        }
    }

    // Round 관리.
    IEnumerator SpawnWave()
    {
        // 현재 표시될 Round 상승.
        PlayerStats.Round++;

        // Game Over 시 표시될 Round 상승.
        PlayerStats.clearRounds++;

        // Wave 난이도에 맞춰 적 유닛 생성.
        Wave wave = waves[waveIndex];

        // 맵에 남은 Enemy 수 설정.
        EnemiesAlive = wave.count;

        // waveIndex의 수 만큼 적 유닛을 생성.
        for (int i = 0; i < wave.count; i++)
        {
            // 적 유닛 생성.
            SpawnMonster(wave.name);

            // 적 유닛 생성 후 일정 시간 대기 후 다음 유닛 생성.
            yield return new WaitForSeconds(1f / wave.rate);
        }

        // waveIndex 상승.
        waveIndex++;
    }

    // Enemy 유닛을 생성.
    void SpawnMonster(string name)
    {
        // 적 유닛 객체 생성.
        GameObject tmp = Resources.Load<GameObject>("Monster/" + name);
        
        // Enemy Script가 존재하지 않는 경우.
        if (tmp.GetComponent<Enemy>() == null)
            AddScript(tmp, name);
        // Enemy Script가 존재하는 않는 경우.
        else
            Instantiate(tmp, startPoind.transform.position, startPoind.transform.rotation);

        // 적 생성 이펙트 활성화.
        start.StartEffect();

        // Enemy 설명 버튼 활성화.
        description.Mob_Description_Button_Toggle(name);
    }

    // 적 유닛에 Enemy Script 추가.
    void AddScript(GameObject _tmp, string _name)
    {
        // Enemy Script 객체 생성.
        Enemy obj = _tmp.AddComponent<Enemy>();

        // CSV 파일 로드.
        TextAsset sourceFile = Resources.Load<TextAsset>("Data/Monster_Data");
        StringReader sr = new StringReader(sourceFile.text);

        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
                continue;
            
            string[] data = line.Split(',');

            // 객체 이름에 맞춰 Enemy 정보 갱신.
            if (_name == data[0])
            {
                obj.startSpeed = float.Parse(data[1]);
                obj.starthealth = float.Parse(data[2]);
                obj.gold = int.Parse(data[3]);
                obj.isDead = bool.Parse(data[4].ToLower());
                obj.description = data[5];
                obj.ani = obj.GetComponent<Animator>();
            }
        }

        // 적 유닛 생성.
        Instantiate(obj, startPoind.transform.position, startPoind.transform.rotation);
    }

}
