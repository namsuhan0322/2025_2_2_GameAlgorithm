using System;

[Serializable]
public class EnhanceStone
{
    public string name;
    public int cost;
    public int xp;

    public float Efficiency => (float)xp / cost;

    public EnhanceStone(string name, int cost, int xp)
    {
        this.name = name;      
        this.cost = cost;
        this.xp = xp;
    }
}
