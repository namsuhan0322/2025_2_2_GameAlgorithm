using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrunManager : MonoBehaviour
{
    private SimplePriorityQueue<Unit> unitQueue = new SimplePriorityQueue<Unit>();

    public float cooldown = 100f;
    private int turnCount = 1;

    public Text showText;

    void Start()
    {
        Initialize();
        showText.text = "<b><color=green>--- 전투 시작! 스페이스바를 누르세요 ---</color></b>";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (unitQueue.Count > 0)
            {
                // 큐에서 다음 턴 유닛을 가져옵니다.
                Unit currentUnit = unitQueue.Dequeue();

                showText.text = $"{turnCount}턴 / {currentUnit.Name}의 턴입니다.";
                Debug.Log($"{turnCount}턴 / {currentUnit.Name}의 턴입니다.");

                // 다음 턴을 위해 턴 카운터를 1 증가시킵니다.
                turnCount++;

                // 다음 턴 시간을 계산하고 다시 큐에 넣는 로직은 그대로 유지합니다.
                float c = cooldown / currentUnit.Speed;
                currentUnit.NextTurnTime += c;
                unitQueue.Enqueue(currentUnit, currentUnit.NextTurnTime);
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
