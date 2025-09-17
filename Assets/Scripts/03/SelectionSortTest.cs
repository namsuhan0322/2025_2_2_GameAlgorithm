using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSortTest : MonoBehaviour
{
    public void Show()
    {
        int[] data = GenerateRandomArray(100);
        StartSelectionSort(data);
        foreach (var item in data)
        {
            Debug.Log(item);
        }
    }

    private int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();
        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000);
        }

        return arr;
    }

    public void StartSelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIndex])
                    minIndex = j;
            }

            int temp = arr[minIndex];
            arr[minIndex] = arr[i];
            arr[i] = temp;
        }
    }
}
