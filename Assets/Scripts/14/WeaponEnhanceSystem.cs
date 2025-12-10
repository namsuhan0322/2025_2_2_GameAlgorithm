using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEnhanceSystem : MonoBehaviour
{
    private List<EnhanceStone> stones = new List<EnhanceStone>();

    [Header("EXP")]
    public Text expText;
    public Slider expSlider;

    [Header("버튼")]
    public Button enchant;
    public Button bruteForce;
    public Button greedyMinWaste;
    public Button greedyEfficiency;
    public Button greedyBigFirst;

    public Text log;

    private int currentExp;

    public int targetLevel;

    public int Targetexp => 8 * targetLevel * targetLevel;

    void Start()
    {
        stones.Add(new EnhanceStone("강화석 소", 3, 8));
        stones.Add(new EnhanceStone("강화석 중", 5, 12));
        stones.Add(new EnhanceStone("강화석 대", 12, 30));
        stones.Add(new EnhanceStone("강화석 특대", 20, 45));

        enchant.onClick.AddListener(Enchant);
        bruteForce.onClick.AddListener(BruteForce);
        greedyMinWaste.onClick.AddListener(GreedyMinWaste);
        greedyEfficiency.onClick.AddListener(GreedyEfficiency);
        greedyBigFirst.onClick.AddListener(GreedyBigFirst);

        if (expSlider != null) expSlider.value = 0f;
    }

    void Update()
    {
        expText.text = $"필요 경험치 {currentExp} / {Targetexp}";
    }

    public void Enchant()
    {

    }

    public void BruteForce()
    {

    }

    public void GreedyMinWaste()
    {
        
    }

    public void GreedyEfficiency()
    {
        
    }

    public void GreedyBigFirst()
    {
        
    }
}
