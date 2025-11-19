using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayer : MonoBehaviour
{
    public float moveSpeed = 5f;

    private List<Vector2Int> path;
    private int targetIndex;
    private bool isMoving = false;
    private Vector3 targetPos;

    public void StartMove(List<Vector2Int> newPath)
    {
        if (newPath == null || newPath.Count == 0) return;

        path = newPath;
        targetIndex = 0;

        UpdateTargetPosition();
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving || path == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            transform.position = targetPos;
            targetIndex++;

            if (targetIndex >= path.Count)
            {
                isMoving = false;
                Debug.Log("Player: 목표 지점 도착 완료!");
            }
            else
            {
                UpdateTargetPosition();
            }
        }
    }

    void UpdateTargetPosition()
    {
        Vector2Int gridPos = path[targetIndex];

        targetPos = new Vector3(gridPos.x, transform.position.y, gridPos.y);

        Vector3 dir = targetPos - transform.position;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void StopMove()
    {
        isMoving = false;
    }
}