using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{  
    public string Name { get; private set; }
    public int Speed { get; private set; }

    public float NextTurnTime { get; set; }

    public Unit(string name, int speed)
    {
        this.Name = name;
        this.Speed = speed;
        this.NextTurnTime = 0f;
    }
}
