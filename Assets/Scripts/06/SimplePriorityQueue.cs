using System;
using System.Collections.Generic;

public class SimplePriorityQueue<T>
{
    private List<(T item, float priority)> heap = new List<(T, float)>();

    public int Count => heap.Count;

    public void Enqueue(T item, float priority)
    {
        // 새 항목을 heap의 마지막에 추가한다.
        heap.Add((item, priority));
        // 부모 노드와 비교하며 우선순위에 따라 위로 올린다. (마지막 인덱스 값의 부모를 찾고 그 부모랑 비교해서 작으면 자리 스왑)
        HeapifyUp(heap.Count - 1);
    }

    // 제일 작은 인덱스값이 최상위 인덱스가 된다.
    public T Dequeue()
    {
        // heap 비어 있으면 예외를 던진다.
        if (heap.Count == 0)
            throw new InvalidOperationException("Queue is empty");

        // 최우선순위 항목을 가져온다.
        T rootItem = heap[0].item;
        // 뒤에서 첫번째 요소를 루트로 이동
        heap[0] = heap[^1];                 
        // (heap개수 - 1) 인덱스를 지운다.
        heap.RemoveAt(heap.Count - 1);
        // 최상위 인덱스를 다운 시킨다. (예 부모 40 이고 자식이 15,30 이면 그 중 더 작은자식15를 올리고 40을 그 자리로 내린다.)
        HeapifyDown(0);
        // 우선순위가 가장 높은 항목을 큐에서 제거하고 반환
        return rootItem;
    }

    private void HeapifyUp(int i)
    {
        // 인덱스가 제일 위로 갈때까지
        while (i > 0)
        {
            // 부모 인덱스
            int parent = (i - 1) / 2;
            // 부모보다 크거나 같으면 멈춤
            if (heap[i].priority >= heap[parent].priority)
                break;
            
            // 부모보다 작으면 스왑한다.
            (heap[i], heap[parent]) = (heap[parent], heap[i]);
            // 현재 인덱스를 부모 인덱스로 갱신
            i = parent;
        }
    }

    private void HeapifyDown(int i)
    {
        // 마지막 인덱스
        int last = heap.Count - 1;
        while (true)
        {
            int left = 2 * i + 1;               // 왼쪽 자식 
            int right = 2 * i + 2;              // 오른쪽 자식 
            int smallest = i;

            // 왼쪽 자식이 존재하고, 그 우선순위가 현재 노드보다 높으면 smallest를 왼쪽 자식으로 설정
            if (left <= last && heap[left].priority < heap[smallest].priority)
                smallest = left;
            // 오른쪽 자식이 존재하고, 현재 smallest보다 우선순위가 더 높으면 smallest 갱신
            if (right <= last && heap[right].priority < heap[smallest].priority)   
                smallest = right;

            // smallest 와 인덱스가 같으면 멈춤
            if (smallest == i) break;

            // 현재 노드와 더 작은 자식 노드를 스왑한다.
            (heap[i], heap[smallest]) = (heap[smallest], heap[i]);
            // 현재 인덱스를 부모 인덱스로 갱신
            i = smallest;
        }
    }
}
