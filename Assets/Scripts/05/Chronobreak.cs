using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chronobreak : MonoBehaviour
{
    public float speed = 5f;

    private Queue<Vector3> moveQueue;
    private Stack<Vector3> moveHistory;
    private GameObject gameObj;
    private bool isMoving = false;
    private bool isRecording = false;
    private bool isPlaying = false;
    private Vector3 targetPos;

    public Text queueCount;
    public Text stackCount;

    void Start()
    {
        moveQueue = new Queue<Vector3>();
        moveHistory = new Stack<Vector3>();
        gameObj = this.gameObject;
        gameObj.GetComponent<Renderer>().material.color = Color.gray;
        targetPos = transform.position;
        queueCount.text = "";
        stackCount.text = "";
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // ³ìÈ­
        if (!isMoving)
        {
            if (x != 0 || y != 0)
            {
                Vector3 move = new Vector3(x, y, 0).normalized * speed * Time.deltaTime;
                targetPos += move;
                moveQueue.Enqueue(targetPos);

                if (Input.GetKey(KeyCode.R))
                {
                    moveHistory.Push(targetPos);
                    stackCount.text = $"StackCount : {moveHistory.Count}";
                }       
                queueCount.text = $"QueueCount : {moveQueue.Count}";               
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isMoving && moveQueue.Count > 0)
                {
                    isMoving = true;
                }                 
            }
        }
        else
        {
            // ³ìÈ­ ÇÃ·¹ÀÌ
            if (moveQueue.Count > 0)
            {
                transform.position = moveQueue.Dequeue();
            }    
            else
                isMoving = false;
        }
    }
}
