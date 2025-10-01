using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour
{
    public float speed = 5f;
    public float chronoBreakSpeed = 5f;

    private Queue<(Vector3, bool)> _moveQueue = new Queue<(Vector3, bool)>();
    private Stack<Vector3> _moveHistory = new Stack<Vector3>();

    private Vector3 _targetPos;
    private bool _isRecording = true;
    private bool _isStack = false;

    void Start()
    {
        _targetPos = transform.position;
        gameObject.GetComponent<Renderer>().material.color = Color.gray;
        _isRecording = true;
        _moveQueue.Clear();
        _moveHistory.Clear();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (_isRecording)
        {                    
            if (x != 0 || y != 0 || Input.GetKeyDown(KeyCode.R))
            {
                Vector3 move = new Vector3(x, 0, y).normalized * speed * Time.deltaTime;
                _targetPos += move;
                _moveQueue.Enqueue((_targetPos, false));
            }

            if (Input.GetKeyDown(KeyCode.R))
                RecordingStack();
            else
                _moveHistory.Push(_targetPos);

            if (Input.GetKey(KeyCode.Space)) _isRecording = false;

            return;
        }

        if (_moveQueue.Count > 0)
        {
            Vector3 target;
            _isStack = false;
            
            (target, _isStack) = _moveQueue.Dequeue();
            transform.position = target;

            if (_isStack)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else
        {
            _isRecording = true;
            _targetPos = transform.position; 
            _moveHistory.Clear();

            gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    private void RecordingStack()
    {
        if (_moveHistory.Count > 0)
        {
            int historyCount = _moveHistory.Count;
            for (int i = 0; i < historyCount; i++)
            {
                if (_moveHistory.Count == 0) break;

                Vector3 target = _moveHistory.Pop();

                if (i % chronoBreakSpeed == 0 || _moveHistory.Count == 0)
                {
                    _moveQueue.Enqueue((target, true));
                    _targetPos = target;
                }
            }
        }
    }
}