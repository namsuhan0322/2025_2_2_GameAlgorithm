using System;
using System.Collections.Generic;

public class SimplePriorityQueue<T>
{
    private List<(T item, float priority)> heap = new List<(T, float)>();

    public int Count => heap.Count;

    public void Enqueue(T item, float priority)
    {
        // �� �׸��� heap�� �������� �߰��Ѵ�.
        heap.Add((item, priority));
        // �θ� ���� ���ϸ� �켱������ ���� ���� �ø���. (������ �ε��� ���� �θ� ã�� �� �θ�� ���ؼ� ������ �ڸ� ����)
        HeapifyUp(heap.Count - 1);
    }

    // ���� ���� �ε������� �ֻ��� �ε����� �ȴ�.
    public T Dequeue()
    {
        // heap ��� ������ ���ܸ� ������.
        if (heap.Count == 0)
            throw new InvalidOperationException("Queue is empty");

        // �ֿ켱���� �׸��� �����´�.
        T rootItem = heap[0].item;
        // �ڿ��� ù��° ��Ҹ� ��Ʈ�� �̵�
        heap[0] = heap[^1];                 
        // (heap���� - 1) �ε����� �����.
        heap.RemoveAt(heap.Count - 1);
        // �ֻ��� �ε����� �ٿ� ��Ų��. (�� �θ� 40 �̰� �ڽ��� 15,30 �̸� �� �� �� �����ڽ�15�� �ø��� 40�� �� �ڸ��� ������.)
        HeapifyDown(0);
        // �켱������ ���� ���� �׸��� ť���� �����ϰ� ��ȯ
        return rootItem;
    }

    private void HeapifyUp(int i)
    {
        // �ε����� ���� ���� ��������
        while (i > 0)
        {
            // �θ� �ε���
            int parent = (i - 1) / 2;
            // �θ𺸴� ũ�ų� ������ ����
            if (heap[i].priority >= heap[parent].priority)
                break;
            
            // �θ𺸴� ������ �����Ѵ�.
            (heap[i], heap[parent]) = (heap[parent], heap[i]);
            // ���� �ε����� �θ� �ε����� ����
            i = parent;
        }
    }

    private void HeapifyDown(int i)
    {
        // ������ �ε���
        int last = heap.Count - 1;
        while (true)
        {
            int left = 2 * i + 1;               // ���� �ڽ� 
            int right = 2 * i + 2;              // ������ �ڽ� 
            int smallest = i;

            // ���� �ڽ��� �����ϰ�, �� �켱������ ���� ��庸�� ������ smallest�� ���� �ڽ����� ����
            if (left <= last && heap[left].priority < heap[smallest].priority)
                smallest = left;
            // ������ �ڽ��� �����ϰ�, ���� smallest���� �켱������ �� ������ smallest ����
            if (right <= last && heap[right].priority < heap[smallest].priority)   
                smallest = right;

            // smallest �� �ε����� ������ ����
            if (smallest == i) break;

            // ���� ���� �� ���� �ڽ� ��带 �����Ѵ�.
            (heap[i], heap[smallest]) = (heap[smallest], heap[i]);
            // ���� �ε����� �θ� �ε����� ����
            i = smallest;
        }
    }
}
