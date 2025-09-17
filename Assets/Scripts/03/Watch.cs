using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Watch : MonoBehaviour
{
    private Stopwatch sw;

    private SelectionSortTest _selectionSortTest;
    private BubbleSortTest _bubbleSortTest;
    private QuickSortTest _quickSortTest;

    public Text selectionSortText;
    public Text bubbleSortText;
    public Text quickSortText;

    private long _time;

    [SerializeField] private int[] _data1;
    [SerializeField] private int[] _data2;
    [SerializeField] private int[] _data3;

    void Start()
    {
        sw = new Stopwatch();

        _data1 = GenerateRandomArray(10000);
        _data2 = GenerateRandomArray(10000);
        _data3 = GenerateRandomArray(10000);

        _selectionSortTest = GetComponent<SelectionSortTest>();
        _bubbleSortTest = GetComponent<BubbleSortTest>();
        _quickSortTest = GetComponent<QuickSortTest>();
    }

    public void ShowSelectionSort()
    {
        SW();
        _selectionSortTest.StartSelectionSort(_data1);
        sw.Stop();
        _time = sw.ElapsedMilliseconds;

        selectionSortText.text = $"selectionTime : {_time} ms";
    }

    public void ShowBubbleSort()
    {
        SW();
        _bubbleSortTest.StartBubbleSort(_data2);
        sw.Stop();
        _time = sw.ElapsedMilliseconds;

        bubbleSortText.text = $"bubbleTime : {_time} ms";
    }

    public void ShowQuickSort()
    {
        SW();
        _quickSortTest.StartQuickSort(_data3, 0, _data3.Length - 1);
        sw.Stop();
        _time = sw.ElapsedMilliseconds;

        quickSortText.text = $"quickTime : {_time} ms";
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

    private void SW()
    {
        sw.Reset();
        sw.Start();
    } 
}
