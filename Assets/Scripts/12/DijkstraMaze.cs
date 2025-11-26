using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DijkstraMaze : MonoBehaviour
{
    public int mapWidth = 21;
    public int mapHeight = 21;

    [Range(0, 100)] public int wallPercentage = 40;

    public GameObject wallPrefab;
    public GameObject pathPrefab;
    public GameObject solutionPrefab;
    public GameObject forestPrefab;
    public GameObject mudPrefab;

    private int[,] map;
    private List<Vector2Int> solutionPath;
    private Transform mapContainer;

    private Vector2Int startPos;
    private Vector2Int goalPos;
    private readonly Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

    public Text showText;
    public Button showPath;
    public Button autoPath;

    public MazePlayer player;

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
            solutionPath = Dijkstra(map, startPos, goalPos);

            if (solutionPath != null && solutionPath.Count > 0) break;
            if (attempts > 1000) break;
        }

        showText.text = $"솔루션 발견까지 생성 시도: {attempts}회";
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
                    if (Random.Range(0, 100) < wallPercentage)
                    {
                        map[x, y] = 0; // 벽
                    }
                    else
                    {
                        int terrainRoll = Random.Range(0, 100);

                        if (terrainRoll < 50) map[x, y] = 1;
                        else if (terrainRoll < 70) map[x, y] = 3;
                        else map[x, y] = 5;
                    }
                }
            }
        }
    }

    List<Vector2Int> Dijkstra(int[,] map, Vector2Int start, Vector2Int goal)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);

        int[,] dist = new int[w, h];                            // 지금까지 온 최소비용
        bool[,] visited = new bool[w, h];                       // 확정 여부
        Vector2Int?[,] parent = new Vector2Int?[w, h];          // 경로 복원용

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                dist[x, y] = int.MaxValue;
            }
        }

        dist[start.x, start.y] = 0;

        // 우선순위 큐 생성
        SimplePriorityQueue<Vector2Int> pq = new SimplePriorityQueue<Vector2Int>();
        pq.Enqueue(start, 0);

        while (pq.Count > 0)
        {
            Vector2Int cur = pq.Dequeue();

            if (visited[cur.x, cur.y]) continue;
            visited[cur.x, cur.y] = true;

            if (cur == goal) return ReconstructPath(parent, start, goal);

            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (!InBounds(map, nx, ny)) continue;
                if (map[nx, ny] == 0) continue;             // 벽

                int moveCost = TileCost(map[nx, ny]);       // cur -> (nx, ny) 비용
                if (moveCost == int.MaxValue) continue;

                int newDist = dist[cur.x, cur.y] + moveCost;

                // 더 싼 길 발견
                if (newDist < dist[nx, ny])
                {
                    dist[nx, ny] = newDist;
                    parent[nx, ny] = cur;
                    pq.Enqueue(new Vector2Int(nx, ny), newDist);
                }
            }
        }

        return null;
    }

    int TileCost(int tile)
    {
        if (tile == 0) return int.MaxValue;

        return tile;
    }

    bool InBounds(int[,] map, int x, int y)
    {
        return x >= 0 && y >= 0 &&
               x < map.GetLength(0) &&
               y < map.GetLength(1);
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
        if (mapContainer != null)
        {
            Destroy(mapContainer.gameObject);
        }
        mapContainer = new GameObject("MazeContainer").transform;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 pos = new Vector3(x, 0, y);
                int tileType = map[x, y];

                if (tileType == 1)
                    Instantiate(pathPrefab, pos, Quaternion.identity, mapContainer);
                else if (tileType == 3)
                    Instantiate(forestPrefab, pos, Quaternion.identity, mapContainer);
                else if (tileType == 5)
                    Instantiate(mudPrefab, pos, Quaternion.identity, mapContainer);
                else
                    Instantiate(pathPrefab, pos, Quaternion.identity, mapContainer);

                if (tileType == 0)
                {
                    Instantiate(wallPrefab, pos + Vector3.up * 0.5f, Quaternion.identity, mapContainer);
                }
            }
        }

        if (player != null)
        {
            Vector3 startWorldPos = new Vector3(startPos.x, 1.5f, startPos.y);
            player.transform.position = startWorldPos;
            player.StopMove();
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
