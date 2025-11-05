using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class HardAI : MonoBehaviour
{
    public Button startButton;
    public int cost = 20;
    public int handSize = 4;

    public List<SequenceUnit> cardDefinitions;

    private struct Card
    {
        public string name;
        public int cost;
        public int damage;
        public CardType type;

        public Card(string name, int cost, int damage, CardType type)
        {
            this.name = name;
            this.cost = cost;
            this.damage = damage;
            this.type = type;
        }
    }

    private List<Card> _masterDeckList = new List<Card>();
    private List<Card> _drawPile = new List<Card>();
    private List<Card> _hand = new List<Card>();

    private Coroutine _runningRoutine;

    private int _maxDamage;
    private List<Card> _bestSequence;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        InitializeDeck();
    }

    private void InitializeDeck()
    {
        _masterDeckList.Clear();

        foreach (SequenceUnit unit in cardDefinitions)
        {
            _masterDeckList.Add(new Card(unit.name, unit.sequenceCost, unit.damage, unit.cardType));
        }

        Debug.Log($"[AI] 덱 초기화 완료. 총 {_masterDeckList.Count}장의 카드가 있습니다.");
        ReshuffleDeck();
    }

    private void ReshuffleDeck()
    {
        _drawPile.Clear();
        _drawPile.AddRange(_masterDeckList);

        int n = _drawPile.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card temp = _drawPile[k];
            _drawPile[k] = _drawPile[n];
            _drawPile[n] = temp;
        }
        Debug.Log($"[AI] 덱을 다시 섞었습니다. (남은 카드: {_drawPile.Count}장)");
    }

    private void DrawHand()
    {
        _hand.Clear();
        System.Text.StringBuilder handLog = new System.Text.StringBuilder();
        handLog.Append("[AI] 이번 턴 손패: ");

        while (_hand.Count < handSize)
        {
            if (_drawPile.Count == 0) ReshuffleDeck();
            if (_drawPile.Count == 0) break;

            int lastIndex = _drawPile.Count - 1;
            Card drawnCard = _drawPile[lastIndex];
            _hand.Add(drawnCard);
            _drawPile.RemoveAt(lastIndex);
            handLog.Append($"{drawnCard.name}, ");
        }
        Debug.Log(handLog.ToString().TrimEnd(',', ' '));
    }

    public void OnStartButtonClicked()
    {
        if (_runningRoutine != null)
        {
            Debug.Log("[AI] 이미 탐색이 실행중입니다.");
            return;
        }
        DrawHand();
        _runningRoutine = StartCoroutine(FindMaxDamage());
    }

    private IEnumerator FindMaxDamage()
    {
        Debug.Log($"[AI] 최적 순서 탐색 시작... (대상 카드: {_hand.Count}장)");
        Stopwatch sw = new Stopwatch();
        sw.Start();

        _maxDamage = 0;
        _bestSequence = new List<Card>();

        bool[] usedHandIndices = new bool[_hand.Count];
        List<Card> currentSequence = new List<Card>();

        RecursiveSearch(currentSequence, usedHandIndices);

        sw.Stop();
        yield return null;

        Debug.Log($"[AI] 최적 순서 탐색 완료. 소요시간: {sw.Elapsed.TotalMilliseconds:F3} ms");

        if (_maxDamage > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Card card in _bestSequence)
            {
                sb.Append($"{card.name} -> ");
            }

            Debug.Log($"[AI] 최대 데미지: {_maxDamage} (코스트: {CalculateSequenceResult(_bestSequence).totalCost} / {cost})");
            Debug.Log($"[AI] 사용 순서: [{sb.ToString().TrimEnd(' ', '-', '>')}]");
        }
        else
        {
            Debug.Log("[AI] 손패에 사용할 수 있는 유효한 조합이 없습니다.");
        }

        _runningRoutine = null;
    }

    private void RecursiveSearch(List<Card> currentSequence, bool[] usedHandIndices)
    {
        (int currentDamage, int currentCost) = CalculateSequenceResult(currentSequence);

        if (currentCost <= cost)
        {
            if (currentDamage > _maxDamage)
            {
                _maxDamage = currentDamage;
                _bestSequence = new List<Card>(currentSequence);
            }
        }
        else return;

        for (int i = 0; i < _hand.Count; i++)
        {
            if (!usedHandIndices[i])
            {
                usedHandIndices[i] = true;
                currentSequence.Add(_hand[i]);

                RecursiveSearch(currentSequence, usedHandIndices);

                currentSequence.RemoveAt(currentSequence.Count - 1);
                usedHandIndices[i] = false;
            }
        }
    }

    private (int totalDamage, int totalCost) CalculateSequenceResult(List<Card> sequence)
    {
        if (sequence == null || sequence.Count == 0) return (0, 0);

        int totalDamage = 0;
        int totalCost = 0;
        int powerUpStacks = 0; 
        int lastAttackBaseDamage = 0;

        foreach (Card card in sequence)
        {
            totalCost += card.cost;

            switch (card.type)
            {
                case CardType.Attack:
                    float damageMod = 1.0f + (powerUpStacks * 0.5f);
                    int calculatedDamage = (int)(card.damage * damageMod);

                    totalDamage += calculatedDamage;

                    powerUpStacks = 0;

                    lastAttackBaseDamage = card.damage;
                    break;

                case CardType.PowerUp:
                    powerUpStacks++;
                    break;

                case CardType.Duplicate:
                    if (lastAttackBaseDamage > 0)
                    {
                        float duplicateMod = 1.0f + (powerUpStacks * 0.5f);
                        int duplicatedDamage = (int)(lastAttackBaseDamage * duplicateMod);

                        totalDamage += duplicatedDamage;

                        powerUpStacks = 0;
                    }
                    break;
            }
        }
        return (totalDamage, totalCost);
    }
}