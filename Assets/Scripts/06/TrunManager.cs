using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunManager : MonoBehaviour
{
    private SimplePriorityQueue<Unit> unitQueue = new SimplePriorityQueue<Unit>();

    public float cooldown = 100f;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (unitQueue.Count > 0)
            {
                Unit currentUnit = unitQueue.Dequeue();


            }
        }
    }

    private void Initialize()
    {
        var p1 = new Unit("전사", 5);
        var p2 = new Unit("마법사", 7);
        var p3 = new Unit("궁수", 10);
        var p4 = new Unit("도적", 12);

        Unit[] allUnit = { p1, p2, p3, p4 };

        foreach (var unit in allUnit)
        {
            unit.NextTurnTime = cooldown / unit.Speed;
            unitQueue.Enqueue(unit, unit.NextTurnTime);
        }
    }
}
