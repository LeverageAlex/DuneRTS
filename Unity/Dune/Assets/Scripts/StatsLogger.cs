using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Stores events of game for statistical purposes on end of game
*/
public class StatsLogger
{
    private static List<Statistic> stats = new List<Statistic>();
    private static StatsLogger instance;

    public static StatsLogger getInstance()
    {
        if(instance == null)
        {
            instance = new StatsLogger();
        }
        return instance;
    }

    public void addStatistic(string player, int roundNbr, string actionText)
    {
        stats.Add(new Statistic(player, roundNbr, actionText));
    }


    public class Statistic
    {
        public string player;
        public int roundNbr;
        public string actionText;

        public Statistic(string _player, int _roundNbr, string _actionText)
        {
            this.player = _player;
            this.roundNbr = _roundNbr;
            this.actionText = _actionText;
        }
    }
}
