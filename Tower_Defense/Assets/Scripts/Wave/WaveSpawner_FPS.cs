// FPS ��忡�� Wave�� ���� �� ���� ���� ���� Script.
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner_FPS : MonoBehaviour
{
    public Wave[] waves;                // �� ���� Prefeb.
    public float timeBetweenWaves;      // Wave ���� �� ���� Wave������ �ð�.
    public GameManager gameManager;     // GameManager ���� ��ü.
    public Description_FPS description; // Description ���� ��ü.

    private Transform startSpanPoint; // �� ���� �ʱ� ���� ��ġ.
    public Transform spanPoint;       // �� ������ ���� ��ġ.

    public static int EnemiesAlive; // �ʿ� ���� Enemy ��.
    public static int waveIndex;    // Wave Index.
    private float countDown;        // ���� Wave������ ���� �ð�.

    // �ش� Script�� ���� �ʱ�ȭ.
    void Start()
    {
        // �ʿ� ���� �� ���� �� �ʱ�ȭ.
        EnemiesAlive = 0;

        // Wave Ƚ�� �ʱ�ȭ.
        waveIndex = 0;

        // ���� Wave������ Ÿ�̸� ����.
        countDown = 3f;

        // Wave Clear �� ���� Wave������ Ÿ�̸� ����.
        timeBetweenWaves = 2;

        // Enemy ���� ��ġ �ʱ�ȭ.
        startSpanPoint = spanPoint;
    }

    // Wave ����.
    void Update()
    {
        // �ʿ� ���� Enemy�� ���� ��� �Լ� ����.
        if (EnemiesAlive > 0)
            return;

        // countDown�� 0�� �� ���.
        if (countDown <= 0)
        {
            // ���� Wave ����.
            StartCoroutine(SpawnWave());

            // countDown�� 2�ʷ� ����.
            countDown = timeBetweenWaves;

            // �Լ� ����.
            return;
        }

        // 1�ʸ��� countDown�� 1�� ����.
        countDown -= Time.deltaTime;

        // �Ҽ��� ����.
        countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);

        // �ش� Level�� ��� Wave Ŭ���� ��.
        if (waveIndex == waves.Length)
        {
            // Game Clear �Լ� ȣ��.
            gameManager.WinLevel();

            // �ش� ���� ����.
            enabled = false;
        }
    }

    // Round ����.
    IEnumerator SpawnWave()
    {
        // ���� ǥ�õ� Round ���.
        PlayerStats.Round++;

        // Game Over �� ǥ�õ� Round ���.
        PlayerStats.clearRounds++;

        // Wave ���̵��� ���� �� ���� ����.
        Wave wave = waves[waveIndex];

        // �ʿ� ���� Enemy �� ����.
        EnemiesAlive = wave.count;

        // �� ���� ��ġ �ʱ�ȭ.
        spanPoint = startSpanPoint;

        // waveIndex�� �� ��ŭ �� ������ ����.
        for (int i = 0; i < wave.count; i++)
        {
            // �� ���� ����.
            SpawnMonster(wave.name);

            // �� ���� ���� �� ���� �ð� ��� �� ���� ���� ����.
            yield return new WaitForSeconds(1f / wave.rate);
        }

        // waveIndex ���.
        waveIndex++;
    }

    // Enemy ������ ����.
    void SpawnMonster(string name)
    {
        // �� ���� ��ü ����.
        GameObject tmp = Resources.Load<GameObject>("Monster/" + name);

        // Enemy Script�� �������� �ʴ� ���.
        if (tmp.GetComponent<Enemy_FPS>() == null)
            AddScript(tmp, name);
        // Enemy Script�� �����ϴ� �ʴ� ���.
        else
        {
            // �� ���� ���� ��ġ ���� �� ����.
            spanPoint.position = new Vector3(spanPoint.position.x + 5, spanPoint.position.y, spanPoint.position.z);
            Instantiate(tmp, spanPoint.position, spanPoint.rotation);
        }

        // Enemy ���� ��ư Ȱ��ȭ.
        description.Description_Button_Toggle(name);
    }

    // �� ���ֿ� Enemy Script �߰�.
    void AddScript(GameObject _tmp, string _name)
    {
        // Enemy Script ��ü ����.
        Enemy_FPS obj = _tmp.AddComponent<Enemy_FPS>();

        // CSV ���� �ε�.
        TextAsset sourceFile = Resources.Load<TextAsset>("Data/Monster_Data");
        StringReader sr = new StringReader(sourceFile.text);

        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
                continue;

            string[] data = line.Split(',');

            // ��ü �̸��� ���� Enemy ���� ����.
            if (_name == data[0])
            {
                obj.startSpeed = float.Parse(data[1]);
                obj.starthealth = float.Parse(data[2]);
                obj.isDead = bool.Parse(data[4].ToLower());
                obj.description = data[5];
                obj.ani = obj.GetComponent<Animator>();
            }
        }

        // �� ���� ���� ��ġ ���� �� ����.
        spanPoint.position = new Vector3(spanPoint.position.x + 5, spanPoint.position.y, spanPoint.position.z);
        Instantiate(obj, spanPoint.position, spanPoint.rotation);
    }

}
