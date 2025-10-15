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
        showText.text = "<b><color=green>--- ���� ����! �����̽��ٸ� �������� ---</color></b>";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (unitQueue.Count > 0)
            {
                // ť���� ���� �� ������ �����ɴϴ�.
                Unit currentUnit = unitQueue.Dequeue();

                showText.text = $"{turnCount}�� / {currentUnit.Name}�� ���Դϴ�.";
                Debug.Log($"{turnCount}�� / {currentUnit.Name}�� ���Դϴ�.");

                // ���� ���� ���� �� ī���͸� 1 ������ŵ�ϴ�.
                turnCount++;

                // ���� �� �ð��� ����ϰ� �ٽ� ť�� �ִ� ������ �״�� �����մϴ�.
                float c = cooldown / currentUnit.Speed;
                currentUnit.NextTurnTime += c;
                unitQueue.Enqueue(currentUnit, currentUnit.NextTurnTime);
            }
        }
    }

    private void Initialize()
    {
        var p1 = new Unit("����", 5);
        var p2 = new Unit("������", 7);
        var p3 = new Unit("�ü�", 10);
        var p4 = new Unit("����", 12);

        Unit[] allUnit = { p1, p2, p3, p4 };

        foreach (var unit in allUnit)
        {
            unit.NextTurnTime = cooldown / unit.Speed;
            unitQueue.Enqueue(unit, unit.NextTurnTime);
        }
    }
}
