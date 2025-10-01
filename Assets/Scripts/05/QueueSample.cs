using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueSample : MonoBehaviour
{
    void Start()
    {
        Queue<string> queue = new Queue<string>();

        queue.Enqueue("첫 번째");
        queue.Enqueue("두 번째");
        queue.Enqueue("세 번째");

        Debug.Log("======= Queue 1 =======");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("=======================");

        // Peek : 맨 위 데이터 확인
        Debug.Log($"Peek : {queue.Peek()}"); // "첫 번째"

        // Dequeue : 맨 앞 데이터 꺼내기
        Debug.Log($"Dequeue : {queue.Dequeue()}");  // "첫 번째"
        Debug.Log($"Dequeue : {queue.Dequeue()}");  // "두 번째"

        Debug.Log($"남은 데이터 수 : {queue.Count}"); // 1

        Debug.Log("======= Queue 2 ======");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("======================");
    }
}
