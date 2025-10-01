using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueSample : MonoBehaviour
{
    void Start()
    {
        Queue<string> queue = new Queue<string>();

        queue.Enqueue("ù ��°");
        queue.Enqueue("�� ��°");
        queue.Enqueue("�� ��°");

        Debug.Log("======= Queue 1 =======");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("=======================");

        // Peek : �� �� ������ Ȯ��
        Debug.Log($"Peek : {queue.Peek()}"); // "ù ��°"

        // Dequeue : �� �� ������ ������
        Debug.Log($"Dequeue : {queue.Dequeue()}");  // "ù ��°"
        Debug.Log($"Dequeue : {queue.Dequeue()}");  // "�� ��°"

        Debug.Log($"���� ������ �� : {queue.Count}"); // 1

        Debug.Log("======= Queue 2 ======");
        foreach (string item in queue)
            Debug.Log(item);
        Debug.Log("======================");
    }
}
