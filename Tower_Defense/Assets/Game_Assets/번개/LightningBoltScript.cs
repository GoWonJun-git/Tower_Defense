using UnityEngine;
using System.Collections.Generic;

namespace DigitalRuby.LightningBolt
{
    // 번개의 애니메이션 유형.
    public enum LightningBoltAnimationMode
    {
        None, Random, Loop, PingPong
    }

    [RequireComponent(typeof(LineRenderer))]
    public class LightningBoltScript : MonoBehaviour
    {
        // 번개가 방출되는 게임 개체.
        public GameObject StartObject;

        // 번개가 방출되는 위치.
        public Vector3 StartPosition;

        // 번개가 끝나는 게임 개체.
        public GameObject EndObject;

        // 번개가 끝나는 위치.
        public Vector3 EndPosition;

        // 숫자가 높을수록 더 많은 선분을 생성.
        [Range(0, 8)]
        public int Generations = 8;
        
        // 번개 지속시간.
        [Range(0.01f, 0.1f)]
        public float Duration = 0.05f;
        private float timer;

        // 번개의 생성폭.
        [Range(0.0f, 0.1f)]
        public float ChaosFactor = 0.05f;

        // 텍스처의 행열 수.
        [Range(1, 64)] public int Rows = 8;
        [Range(1, 64)] public int Columns = 2;

        // 번개의 애니메이션 모드
        public LightningBoltAnimationMode AnimationMode = LightningBoltAnimationMode.PingPong;

        [HideInInspector]
        [System.NonSerialized]
        public System.Random RandomGenerator = new System.Random();

        private LineRenderer lineRenderer;
        private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();
        private int startIndex;
        private Vector2 size;
        private Vector2[] offsets;
        private int animationOffsetIndex;
        private int animationPingPongDirection = 1;
        private bool orthographic;

        // 수직 벡터 가져오기.
        private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
        {
            if (directionNormalized == Vector3.zero)
                side = Vector3.right;
            else
            {
                // 외적을 사용하여 directionNormalized 주위의 수직 벡터를 검색.
                float x = directionNormalized.x;
                float y = directionNormalized.y;
                float z = directionNormalized.z;
                float px, py, pz;
                float ax = Mathf.Abs(x), ay = Mathf.Abs(y), az = Mathf.Abs(z);
                if (ax >= ay && ay >= az)
                {
                    // x는 최대값이므로 (1, 1)에서 임의로 (py, pz)를 선택.
                    py = 1.0f;
                    pz = 1.0f;
                    px = -(y * py + z * pz) / x;
                }
                else if (ay >= az)
                {
                    // y는 최대값이므로 (1, 1)에서 임의로 (px, pz)를 선택.
                    px = 1.0f;
                    pz = 1.0f;
                    py = -(x * px + z * pz) / y;
                }
                else
                {
                    // z는 최대값이므로 (1, 1)에서 임의로 (px, pz)를 선택.
                    px = 1.0f;
                    py = 1.0f;
                    pz = -(x * px + y * py) / z;
                }
                side = new Vector3(px, py, pz).normalized;
            }
        }

        // 번개 화살 생성.
        private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount)
        {
            if (generation < 0 || generation > 8)
                return;
            else if (orthographic)
                start.z = end.z = Mathf.Min(start.z, end.z);

            segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));

            if (generation == 0)
                return;

            Vector3 randomVector;
            if (offsetAmount <= 0.0f)
                offsetAmount = (end - start).magnitude * ChaosFactor;

            while (generation-- > 0)
            {
                int previousStartIndex = startIndex;
                startIndex = segments.Count;

                for (int i = previousStartIndex; i < startIndex; i++)
                {
                    start = segments[i].Key;
                    end = segments[i].Value;

                    // determine a new direction for the split
                    Vector3 midPoint = (start + end) * 0.5f;

                    // adjust the mid point to be the new location
                    RandomVector(ref start, ref end, offsetAmount, out randomVector);
                    midPoint += randomVector;

                    // add two new segments
                    segments.Add(new KeyValuePair<Vector3, Vector3>(start, midPoint));
                    segments.Add(new KeyValuePair<Vector3, Vector3>(midPoint, end));
                }

                // halve the distance the lightning can deviate for each generation down
                offsetAmount *= 0.5f;
            }
        }

        public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
        {
            if (orthographic)
            {
                Vector3 directionNormalized = (end - start).normalized;
                Vector3 side = new Vector3(-directionNormalized.y, directionNormalized.x, directionNormalized.z);
                float distance = ((float)RandomGenerator.NextDouble() * offsetAmount * 2.0f) - offsetAmount;
                result = side * distance;
            }
            else
            {
                Vector3 directionNormalized = (end - start).normalized;
                Vector3 side;
                GetPerpendicularVector(ref directionNormalized, out side);

                // generate random distance
                float distance = (((float)RandomGenerator.NextDouble() + 0.1f) * offsetAmount);

                // get random rotation angle to rotate around the current direction
                float rotationAngle = ((float)RandomGenerator.NextDouble() * 360.0f);

                // rotate around the direction and then offset by the perpendicular vector
                result = Quaternion.AngleAxis(rotationAngle, directionNormalized) * side * distance;
            }
        }

        private void SelectOffsetFromAnimationMode()
        {
            int index;

            if (AnimationMode == LightningBoltAnimationMode.None)
            {
                lineRenderer.material.mainTextureOffset = offsets[0];
                return;
            }
            else if (AnimationMode == LightningBoltAnimationMode.PingPong)
            {
                index = animationOffsetIndex;
                animationOffsetIndex += animationPingPongDirection;
                if (animationOffsetIndex >= offsets.Length)
                {
                    animationOffsetIndex = offsets.Length - 2;
                    animationPingPongDirection = -1;
                }
                else if (animationOffsetIndex < 0)
                {
                    animationOffsetIndex = 1;
                    animationPingPongDirection = 1;
                }
            }
            else if (AnimationMode == LightningBoltAnimationMode.Loop)
            {
                index = animationOffsetIndex++;
                if (animationOffsetIndex >= offsets.Length)
                    animationOffsetIndex = 0;
            }
            else
                index = RandomGenerator.Next(0, offsets.Length);

            if (index >= 0 && index < offsets.Length)
                lineRenderer.material.mainTextureOffset = offsets[index];
            else
                lineRenderer.material.mainTextureOffset = offsets[0];
        }

        private void UpdateLineRenderer()
        {
            int segmentCount = (segments.Count - startIndex) + 1;
            lineRenderer.positionCount = segmentCount;

            if (segmentCount < 1)
            {
                return;
            }

            int index = 0;
            lineRenderer.SetPosition(index++, segments[startIndex].Key);

            for (int i = startIndex; i < segments.Count; i++)
            {
                lineRenderer.SetPosition(index++, segments[i].Value);
            }

            segments.Clear();

            SelectOffsetFromAnimationMode();
        }

        private void Start()
        {
            orthographic = (Camera.main != null && Camera.main.orthographic);
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
            UpdateFromMaterialChange();
        }

        private void Update()
        {
            orthographic = (Camera.main != null && Camera.main.orthographic);

            if (timer <= 0.0f)
                Trigger();
            timer -= Time.deltaTime;
        }

        // 번개를 발사.
        public void Trigger()
        {
            Vector3 start, end;
            timer = Duration + Mathf.Min(0.0f, timer);

            if (StartObject == null)
                start = StartPosition;
            else
                start = StartObject.transform.position + StartPosition;

            if (EndObject == null)
                end = EndPosition;
            else
                end = EndObject.transform.position + EndPosition;

            startIndex = 0;
            GenerateLightningBolt(start, end, Generations, Generations, 0.0f);
            UpdateLineRenderer();
        }

        // 라인 렌더러에서 재료를 변경하는 경우 이 메소드를 호출.
        public void UpdateFromMaterialChange()
        {
            size = new Vector2(1.0f / Columns, 1.0f / Rows);
            lineRenderer.material.mainTextureScale = size;
            offsets = new Vector2[Rows * Columns];

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    offsets[x + (y * Columns)] = new Vector2((float)x / Columns, (float)y / Rows);
                }
            }
        }

    }
}