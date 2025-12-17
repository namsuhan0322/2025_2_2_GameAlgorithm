using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEnhanceSystem : MonoBehaviour
{
    private List<EnhanceStone> stones = new List<EnhanceStone>();

    [Header("EXP UI")]
    public Text expText;
    public Slider expSlider;

    [Header("Level UI")]
    public Text currentLevelText;
    public Text nextLevelText;

    [Header("버튼")]
    public Button enchant;
    public Button bruteForce;
    public Button greedyMinWaste;
    public Button greedyEfficiency;
    public Button greedyBigFirst;

    [Header("로그")]
    public Text log;

    private int currentExp = 0;
    private int weaponLevel = 1;

    public int TargetExp => 8 * weaponLevel * weaponLevel;

    void Start()
    {
        stones.Add(new EnhanceStone("강화석 소", 8, 3));
        stones.Add(new EnhanceStone("강화석 중", 12, 5));
        stones.Add(new EnhanceStone("강화석 대", 30, 12));
        stones.Add(new EnhanceStone("강화석 특대", 45, 20));

        enchant.onClick.AddListener(Enchant);
        bruteForce.onClick.AddListener(BruteForce);
        greedyMinWaste.onClick.AddListener(GreedyMinWaste);
        greedyEfficiency.onClick.AddListener(GreedyEfficiency);
        greedyBigFirst.onClick.AddListener(GreedyBigFirst);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (currentLevelText != null) currentLevelText.text = $"+{weaponLevel}";
        if (nextLevelText != null) nextLevelText.text = $"+{weaponLevel + 1}";

        if (expSlider != null)
        {
            expSlider.maxValue = TargetExp;
            expSlider.value = currentExp;
        }

        if (expText != null)
        {
            expText.text = $"필요 경험치 {currentExp} / {TargetExp}";
        }
    }

    public void Enchant()
    {
        if (currentExp >= TargetExp)
        {
            // 레벨업 처리
            weaponLevel++;
            currentExp = 0;

            log.text = $"<color=cyan>강화 성공! +{weaponLevel} 달성!</color>";
            UpdateUI();
        }
        else
        {
            log.text = $"<color=red>경험치가 부족합니다. ({currentExp}/{TargetExp})</color>";
        }
    }

    public void BruteForce()
    {
        StartCoroutine(BruteForceRoutine());
    }

    private IEnumerator BruteForceRoutine()
    {
        log.text = "최적 해(최소 비용) 계산 중...";
        yield return null;

        Stopwatch sw = new Stopwatch();
        sw.Start();

        int neededExp = TargetExp - currentExp;
        if (neededExp <= 0)
        {
            log.text = "이미 경험치가 가득 찼습니다.";
            yield break;
        }

        int maxSmall = (neededExp / stones[0].xp) + 1;
        int maxMedium = (neededExp / stones[1].xp) + 1;
        int maxLarge = (neededExp / stones[2].xp) + 1;
        int maxXLarge = (neededExp / stones[3].xp) + 1;

        int minCost = int.MaxValue;
        int[] bestCounts = new int[4];
        int finalExp = 0;

        for (int xl = 0; xl <= maxXLarge; xl++)
        {
            for (int l = 0; l <= maxLarge; l++)
            {
                for (int m = 0; m <= maxMedium; m++)
                {
                    for (int s = 0; s <= maxSmall; s++)
                    {
                        int tempExp = (s * stones[0].xp) + (m * stones[1].xp) + (l * stones[2].xp) + (xl * stones[3].xp);
                        int tempCost = (s * stones[0].cost) + (m * stones[1].cost) + (l * stones[2].cost) + (xl * stones[3].cost);

                        if (tempExp >= neededExp)
                        {
                            if (tempCost < minCost)
                            {
                                minCost = tempCost;
                                finalExp = tempExp;
                                bestCounts[0] = s;
                                bestCounts[1] = m;
                                bestCounts[2] = l;
                                bestCounts[3] = xl;
                            }
                            break;
                        }
                    }
                }
            }
            if (xl % 5 == 0) yield return null;
        }

        sw.Stop();

        PrintResult("Brute Force (최소 비용)", minCost, finalExp + currentExp, // 현재 경험치 포함
            new Dictionary<string, int>() {
                { stones[0].name, bestCounts[0] },
                { stones[1].name, bestCounts[1] },
                { stones[2].name, bestCounts[2] },
                { stones[3].name, bestCounts[3] }
            }, sw.Elapsed.TotalMilliseconds);
    }

    private void RunGreedy(string strategyName, System.Comparison<EnhanceStone> sorter)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        List<EnhanceStone> sortedStones = new List<EnhanceStone>(stones);
        sortedStones.Sort(sorter);

        int neededExp = TargetExp - currentExp;
        if (neededExp <= 0)
        {
            log.text = "이미 경험치가 가득 찼습니다.";
            return;
        }

        int totalCost = 0;
        int currentSimulatedExp = 0;

        Dictionary<string, int> purchaseCount = new Dictionary<string, int>();
        foreach (var stone in stones) purchaseCount.Add(stone.name, 0);

        foreach (var stone in sortedStones)
        {
            if (neededExp <= 0) break;

            int count = neededExp / stone.xp;

            if (count > 0)
            {
                purchaseCount[stone.name] += count;
                totalCost += count * stone.cost;
                int addedExp = count * stone.xp;
                currentSimulatedExp += addedExp;
                neededExp -= addedExp;
            }
        }

        if (neededExp > 0)
        {
            EnhanceStone smallStone = stones[0];
            int extraSmallCount = Mathf.CeilToInt((float)neededExp / smallStone.xp);

            purchaseCount[smallStone.name] += extraSmallCount;
            totalCost += extraSmallCount * smallStone.cost;
            currentSimulatedExp += extraSmallCount * smallStone.xp;
        }

        sw.Stop();
        PrintResult(strategyName, totalCost, currentSimulatedExp + currentExp, purchaseCount, sw.Elapsed.TotalMilliseconds);
    }

    private void PrintResult(string title, int cost, int finalExp, Dictionary<string, int> counts, double timeMs)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"<color=yellow>[{title}]</color>");

        foreach (var pair in counts)
        {
            if (pair.Value > 0)
                sb.AppendLine($"{pair.Key} x {pair.Value}");
        }
        sb.AppendLine($"총 가격 : {cost} gold");
        sb.AppendLine($"최종 경험치 : {finalExp} / {TargetExp} (초과: {finalExp - TargetExp})");
        sb.AppendLine($"연산 시간 : {timeMs:F4} ms");

        log.text = sb.ToString();
        currentExp = finalExp;

        UpdateUI();
    }

    public void GreedyMinWaste()
    {
        RunGreedy("Greedy (경험치 낭비 최소)", (a, b) => a.xp.CompareTo(b.xp));
    }

    public void GreedyEfficiency()
    {
        RunGreedy("Greedy (골드 효율)", (a, b) => b.Efficiency.CompareTo(a.Efficiency));
    }

    public void GreedyBigFirst()
    {
        RunGreedy("Greedy (큰 거 부터)", (a, b) => b.xp.CompareTo(a.xp));
    }
}