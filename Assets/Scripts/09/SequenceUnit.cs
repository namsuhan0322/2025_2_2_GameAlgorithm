public enum CardType
{
    Attack,    
    PowerUp,   
    Duplicate   
}

[System.Serializable]
public class SequenceUnit
{
    public string name;
    public CardType cardType; 
    public int damage;
    public int sequenceCost;

    public SequenceUnit(string name, CardType type, int damage, int sequenceCost)
    {
        this.name = name;
        this.cardType = type;
        this.damage = damage;
        this.sequenceCost = sequenceCost;
    }
}