using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSample : MonoBehaviour
{
    void Start()
    {
        Stack<int> stack = new Stack<int>();

        stack.Push(10);
        stack.Push(20);
        stack.Push(30);

        Debug.Log("======= Stack 1 =======");
        foreach(int num in stack)
            Debug.Log(num);
        Debug.Log("=======================");

        // Peek : �� �� ������ Ȯ��
        Debug.Log($"Peek : {stack.Peek()}"); // 30 (���� ����)

        // Pop : �� �� ������ ������
        Debug.Log($"Pop : {stack.Pop()}");  // 30 ������ ����
        Debug.Log($"Pop : {stack.Pop()}");  // 20 ������ ����

        Debug.Log($"���� ������ �� : {stack.Count}"); // 1

        Debug.Log("======= Stack 2 ======");
        foreach(int num in stack)
            Debug.Log(num);
        Debug.Log("======================");
    }
}
