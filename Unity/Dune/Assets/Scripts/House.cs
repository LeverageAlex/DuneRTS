using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : ScriptableObject
{
    private List<Character> Characters = new List<Character> ();
    private readonly string color;
    private readonly string name;


    public House(string name, string color)
    {
        this.name = name;
        this.color = color;
    }

    public string Color
    {
        get { return color; }
    }

    public string Name
    {
        get { return name; }
    }

}
