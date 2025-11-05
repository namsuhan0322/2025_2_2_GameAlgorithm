using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class EasyAI : MonoBehaviour
{
    public Button startButton;
    public int cost = 15;

    public List<SequenceUnit> cardDefinitions;

    private List<Card> _hand = new List<Card>();
    private Coroutine _runningRoutine;

    private struct Card
    {
        public string name;
        public int cost;
        public int damage;

        public Card(string name, int cost, int damage)
        {
            this.name = name;
            this.cost = cost;
            this.damage = damage;
        }
    }

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        InitializeHand();
    }

    private void InitializeHand()
    {
        _hand.Clear();

        foreach (SequenceUnit unit in cardDefinitions)
        {
            _hand.Add(new Card(unit.name, unit.sequenceCost, unit.damage));
        }

        Debug.Log($"[AI] 손패 초기화 완료. 총 {_hand.Count}장의 카드를 들고 있습니다.");
    }

    public void OnStartButtonClicked()
    {
        if (_runningRoutine != null)
        {
            Debug.Log("[AI] 이미 탐색이 실행중입니다.");
            return;
        }

        _runningRoutine = StartCoroutine(FindMaxDamage());
    }

    private IEnumerator FindMaxDamage()
    {
        Debug.Log("[AI] 최적 조합 탐색 시작...");
        Stopwatch sw = new Stopwatch();
        sw.Start();

        int cardCount = _hand.Count;
        long combinations = 1L << cardCount;

        int maxDamage = 0;
        int bestCost = 0;
        List<Card> bestCombination = new List<Card>();
        long tryCount = 0;

        for (long i = 0; i < combinations; i++)
        {
            int currentDamage = 0;
            int currentCost = 0;
            List<Card> currentCombination = new List<Card>();
            tryCount++;

            for (int j = 0; j < cardCount; j++)
            {
                if ((i & (1L << j)) != 0)
                {
                    // j번째 카드를 조합에 포함
                    Card card = _hand[j];
                    currentCost += card.cost;
                    currentDamage += card.damage;
                    currentCombination.Add(card);
                }
            }

            if (currentCost <= cost)
            {
                if (currentDamage > maxDamage)
                {
                    // 새로운 최적의 조합 발견
                    maxDamage = currentDamage;
                    bestCost = currentCost;
                    bestCombination = currentCombination;
                }
            }

            if (tryCount % 100 == 0) yield return null;
        }

        sw.Stop();

        Debug.Log($"[AI] 최적 조합 탐색 완료. 소요시간: {sw.Elapsed.TotalMilliseconds:F3} ms");
        Debug.Log($"[AI] 총 {tryCount}개의 조합 확인");

        if (maxDamage > 0)
        {
            foreach (Card card in bestCombination)
            {
                Debug.Log($"쓴 카드 : {card.name} : ({card.damage}dmg, {card.cost}cost)");
            }

            Debug.Log($"[AI] 최대 데미지: {maxDamage} (코스트: {bestCost} / {cost})");
        }
        else
        {
            Debug.Log("[AI] 유효한 조합을 찾지 못했습니다 (사용할 수 있는 카드가 없음).");
        }

        _runningRoutine = null;
    }
}