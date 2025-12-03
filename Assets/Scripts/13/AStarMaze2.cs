using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStarMaze2 : MonoBehaviour
{
    public int mapWidth = 21;
    public int mapHeight = 21;

    [Range(0, 100)] public int wallPercentage = 40;
    [Range(1, 10)] public int enemyCount = 3;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject pathPrefab;
    public GameObject solutionPrefab;
    public GameObject forestPrefab;
    public GameObject mudPrefab;
    public GameObject enemyPrefab;

    [Header("UI & Player")]
    public Text showText;
    public Button showPath;
    public Button autoPath;
    public MazePlayer player;

    // 내부 변수
    private int[,] map;
    private List<Vector2Int> solutionPath;
    private Transform mapContainer;

    private Vector2Int startPos;
    private Vector2Int goalPos;
    private List<Vector2Int> enemiesPos = new List<Vector2Int>();
    private readonly Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

    void Start()
    {
        if (mapWidth % 2 == 0) mapWidth++;
        if (mapHeight % 2 == 0) mapHeight++;

        GenerateAndCheckMaze();

        showPath.onClick.AddListener(ShowSolution);
        autoPath.onClick.AddListener(StartPlayerMovement);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateAndCheckMaze();
        }
    }

    void GenerateAndCheckMaze()
    {
        int attempts = 0;

        while (true)
        {
            attempts++;
            GenerateMaze();
            SpawnEnemies();

            if (IsPathExist(startPos, goalPos))
            {
                solutionPath = AStar(map, startPos, goalPos);
                break;
            }

            if (attempts > 1000) break;
        }

        showText.text = $"맵 생성 시도: {attempts}회";
        DrawMap();
    }

    void GenerateMaze()
    {
        map = new int[mapWidth, mapHeight];
        startPos = new Vector2Int(1, 1);
        goalPos = new Vector2Int(mapWidth - 2, mapHeight - 2);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // 가장자리 벽
                if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
                {
                    map[x, y] = 0; // 벽
                }
                else if ((x == startPos.x && y == startPos.y) || (x == goalPos.x && y == goalPos.y))
                {
                    map[x, y] = 1; // 땅
                }
                else
                {
                    // 랜덤 벽 생성
                    if (Random.Range(0, 100) < wallPercentage)
                    {
                        map[x, y] = 0; // 벽
                    }
                    else
                    {
                        int terrainRoll = Random.Range(0, 100);
                        if (terrainRoll < 50) map[x, y] = 1;      // 일반 땅
                        else if (terrainRoll < 70) map[x, y] = 3; // 숲
                        else map[x, y] = 5;                       // 진흙
                    }
                }
            }
        }
    }

    bool IsPathExist(Vector2Int start, Vector2Int goal)
    {
        bool[,] visited = new bool[mapWidth, mapHeight];
        Queue<Vector2Int> q = new Queue<Vector2Int>();

        q.Enqueue(start);
        visited[start.x, start.y] = true;

        while (q.Count > 0)
        {
            Vector2Int cur = q.Dequeue();
            if (cur == goal) return true;

            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (!InBounds(nx, ny)) continue;
                if (visited[nx, ny]) continue;
                if (map[nx, ny] == 0) continue; // 벽만 못 지나감

                visited[nx, ny] = true;
                q.Enqueue(new Vector2Int(nx, ny));
            }
        }
        return false;
    }

    List<Vector2Int> AStar(int[,] map, Vector2Int start, Vector2Int goal)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);

        int[,] gCost = new int[w, h];
        bool[,] visited = new bool[w, h];
        Vector2Int?[,] parent = new Vector2Int?[w, h];

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                gCost[x, y] = int.MaxValue;

        gCost[start.x, start.y] = 0;

        List<Vector2Int> open = new List<Vector2Int>();
        open.Add(start);

        while (open.Count > 0)
        {
            // F cost가 가장 낮은 노드 찾기
            int bestIndex = 0;
            int bestF = F(open[0], gCost, goal);

            for (int i = 1; i < open.Count; i++)
            {
                int f = F(open[i], gCost, goal);
                if (f < bestF)
                {
                    bestF = f;
                    bestIndex = i;
                }
            }

            Vector2Int cur = open[bestIndex];
            open.RemoveAt(bestIndex);

            if (visited[cur.x, cur.y]) continue;
            visited[cur.x, cur.y] = true;

            // 도착
            if (cur == goal) return ReconstructPath(parent, start, goal);

            // 이웃 탐색
            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (!InBounds(nx, ny)) continue;
                if (map[nx, ny] == 0) continue; // 벽
                if (visited[nx, ny]) continue;

                // 이동 비용 계산
                int moveCost = GetMovementCost(nx, ny);
                int newG = gCost[cur.x, cur.y] + moveCost;

                if (newG < gCost[nx, ny])
                {
                    gCost[nx, ny] = newG;
                    parent[nx, ny] = cur;

                    if (!open.Contains(new Vector2Int(nx, ny)))
                        open.Add(new Vector2Int(nx, ny));
                }
            }
        }

        return null;
    }

    int F(Vector2Int pos, int[,] gCost, Vector2Int goal)
    {
        return gCost[pos.x, pos.y] + H(pos, goal);
    }

    int H(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    int GetMovementCost(int x, int y)
    {
        int baseCost = 1;

        foreach (var d in dirs)
        {
            int nx = x + d.x;
            int ny = y + d.y;

            if (InBounds(nx, ny))
            {
                // 비용 2 추가
                if (map[nx, ny] == 0) return baseCost + 2; 
            }
        }

        foreach (var enemy in enemiesPos)
        {
            float distance = Vector2Int.Distance(new Vector2Int(x, y), enemy);

            if (distance < 3.0f)
            {
                int penalty = (int)((3.0f - distance) * 20);
                baseCost += penalty;
            }
        }

        return baseCost;
    }

    bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < mapWidth && y < mapHeight;
    }

    List<Vector2Int> ReconstructPath(Vector2Int?[,] parent, Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? cur = goal;

        while (cur.HasValue)
        {
            path.Add(cur.Value);
            if (cur.Value == start) break;
            cur = parent[cur.Value.x, cur.Value.y];
        }

        path.Reverse();
        return path;
    }

    void DrawMap()
    {
        if (mapContainer != null) Destroy(mapContainer.gameObject);
        mapContainer = new GameObject("MazeContainer").transform;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 pos = new Vector3(x, 0, y);
                int tileType = map[x, y];

                // 바닥 생성
                GameObject prefabToUse = pathPrefab;
                if (tileType == 3) prefabToUse = forestPrefab;
                else if (tileType == 5) prefabToUse = mudPrefab;

                Instantiate(prefabToUse, pos, Quaternion.identity, mapContainer);

                // 벽 생성
                if (tileType == 0)
                {
                    Instantiate(wallPrefab, pos + Vector3.up * 0.5f, Quaternion.identity, mapContainer);
                }
            }
        }

        EnemiesSpawnAndPositions();

        if (player != null)
        {
            Vector3 startWorldPos = new Vector3(startPos.x, 1.5f, startPos.y);
            player.transform.position = startWorldPos;
            player.StopMove();
        }
    }

    void SpawnEnemies()
    {
        enemiesPos.Clear();
        int count = 0;
        int safety = 0;

        while (count < enemyCount && safety < 1000)
        {
            safety++;
            int x = Random.Range(1, mapWidth - 1);
            int y = Random.Range(1, mapHeight - 1);

            // 벽, 숲, 진흙 이면 스킵
            if (map[x, y] == 0 || map[x, y] == 3 || map[x, y] == 5) continue;
            Vector2Int pos = new Vector2Int(x, y);
            // 시작점이나 도착점이면 스킵
            if (pos == startPos || pos == goalPos) continue;

            if (enemiesPos.Contains(pos)) continue;

            enemiesPos.Add(pos);
            count++;
        }
    }

    void EnemiesSpawnAndPositions()
    {
        if (enemyPrefab == null) return;

        foreach (var pos in enemiesPos)
        {
            Vector3 worldPos = new Vector3(pos.x, 1f, pos.y);
            Instantiate(enemyPrefab, worldPos, Quaternion.identity, mapContainer);
        }
    }

    public void StartPlayerMovement()
    {
        if (solutionPath != null && player != null)
        {
            player.StartMove(solutionPath);
        }
    }

    public void ShowSolution()
    {
        if (solutionPath == null || solutionPath.Count == 0) return;

        foreach (Vector2Int pos in solutionPath)
        {
            if (pos == startPos || pos == goalPos) continue;
            Vector3 visualPos = new Vector3(pos.x, 0.6f, pos.y);
            Instantiate(solutionPrefab, visualPos, Quaternion.identity, mapContainer);
        }
    }
}