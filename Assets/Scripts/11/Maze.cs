using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Maze : MonoBehaviour
{
    public int mapWidth = 21;
    public int mapHeight = 21;

    [Range(0, 100)] public int wallPercentage = 40;

    public GameObject wallPrefab;
    public GameObject pathPrefab;
    public GameObject solutionPrefab;
    public GameObject depthPrefab;

    private int[,] map;
    private bool[,] visited;
    private List<Vector2Int> solutionPath;
    private Transform mapContainer;

    private Vector2Int startPos;
    private Vector2Int goalPos;
    private Vector2Int?[,] parent;
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
        do
        {
            attempts++;
            GenerateMaze();
        } while (!IsMazeSolvable());

        solutionPath = FindPathBFS();

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
                // 가장 외곽은 무조건 벽
                if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
                {
                    map[x, y] = 1; // 1: 벽
                }
                else if ((x == startPos.x && y == startPos.y) || (x == goalPos.x && y == goalPos.y))
                {
                    map[x, y] = 0; // 0: 길
                }
                else
                {
                    map[x, y] = Random.Range(0, 100) < wallPercentage ? 1 : 0;
                }
            }
        }
    }

    bool IsMazeSolvable()
    {
        visited = new bool[mapWidth, mapHeight];
        solutionPath = new List<Vector2Int>();
        return SearchMaze(startPos.x, startPos.y);
    }

    bool SearchMaze(int x, int y)
    {
        if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight) return false;
        if (map[x, y] == 1 || visited[x, y]) return false;

        visited[x, y] = true;
        solutionPath.Add(new Vector2Int(x, y));

        if (x == goalPos.x && y == goalPos.y) return true;

        foreach (var d in dirs)
        {
            if (SearchMaze(x + d.x, y + d.y)) return true; // 목표를 찾으면 즉시 true 반환
        }

        solutionPath.RemoveAt(solutionPath.Count - 1);
        return false;
    }

    private List<Vector2Int> FindPathBFS()
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);
        visited = new bool[w, h];
        parent = new Vector2Int?[w, h];

        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(startPos);
        visited[startPos.x, startPos.y] = true;

        while (q.Count > 0)
        {
            Vector2Int cur = q.Dequeue();

            // 목표 도착
            if (cur == goalPos)
            {
                Debug.Log("BFS : Goal 도착!");
                return ReconstructPath();
            }

            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (!IsBounds(nx, ny)) continue;
                if (map[nx, ny] == 1) continue;
                if (visited[nx, ny]) continue;

                visited[nx, ny] = true;
                parent[nx, ny] = cur;
                q.Enqueue(new Vector2Int(nx, ny));
            }
        }

        Debug.Log("BFS: 경로 없음");
        return null;
    }

    bool IsBounds(int x, int y)
    {
        return x >= 0 & y >= 0 &&
               x < map.GetLength(0) &&
               y < map.GetLength(1);
    }

    List<Vector2Int> ReconstructPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? cur = goalPos;

        // goal -> start 방향으로 parent 따라가기
        while (cur.HasValue)
        {
            path.Add(cur.Value);
            cur = parent[cur.Value.x, cur.Value.y];
        }

        path.Reverse(); // start -> goal 순서로 반전
        Debug.Log($"경로 길이 : {path.Count}");
        foreach (var p in path)
        {
            Debug.Log(p);
        }
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
                Instantiate(pathPrefab, pos, Quaternion.identity, mapContainer);

                if (map[x, y] == 1)
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
