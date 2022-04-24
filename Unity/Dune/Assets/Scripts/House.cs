using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : ScriptableObject
{
    private List<Character> Characters = new List<Character> ();
    private readonly Color color;
    private readonly string houseName;
    private int atomicsLeft;
    


    public House(string name, Color color)
    {
        this.houseName = name;
        this.color = color;
        
    }

    public Color Color
    {
        get { return color; }
    }

    public string Name
    {
        get { return name; }
    }

    public int AtomicsLeft
    {
        get { return atomicsLeft; }
    }

    

}
