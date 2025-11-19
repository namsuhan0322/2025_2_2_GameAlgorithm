using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDFS : MonoBehaviour
{
    public int mapWidth = 21;
    public int mapHeight = 21;
    [Range(0, 100)]
    public int wallPercentage = 40; 

    public GameObject wallPrefab;   
    public GameObject pathPrefab;  
    public GameObject solutionPrefab; 

    private int[,] map;                
    private bool[,] visited;           
    private List<Vector2Int> solutionPath;
    private Transform mapContainer;     

    private Vector2Int startPos;
    private Vector2Int goalPos;
    private readonly Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

    void Start()
    {
        if (mapWidth % 2 == 0) mapWidth++;
        if (mapHeight % 2 == 0) mapHeight++;

        GenerateAndCheckMaze();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateAndCheckMaze();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowSolution();
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

        Debug.Log($"솔루션 발견까지 생성 시도: {attempts}회");
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
                // 시작(1,1)과 끝(x-2, y-2)은 무조건 길
                else if ((x == startPos.x && y == startPos.y) || (x == goalPos.x && y == goalPos.y))
                {
                    map[x, y] = 0; // 0: 길
                }
                // 그 외 내부는 wallPercentage 확률로 벽 생성
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
    }

    void ShowSolution()
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
