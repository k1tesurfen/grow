using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Competitor : IComparable<Competitor>
{
    public string name;
    public int score;

    //Competitors in the game always start with a score of 0.
    public Competitor(string name)
    {
        this.name = name;
        score = 0;
    }

    //this method is important for a collection to sort the competitors with the .Sort() method
    public int CompareTo(Competitor other)
    {
        if(other == null)
        {
            return 1;
        }

        return score - other.score;
    }
}
