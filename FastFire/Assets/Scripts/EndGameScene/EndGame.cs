using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame
{
    public static int NumKills { get; set; }
    public static string PlayerName { get; set; }
    public static string GameTime { get; set; }

    public void changeValue(int numKills, string playerName, string gameTime)
    {
        NumKills = numKills;
        PlayerName = playerName;
        GameTime = gameTime.Substring(0,8);
    }

    public int getKills()
    {
        return NumKills;
    }

    public string getNickName()
    {
        return PlayerName;
    }

    public string getTime()
    {
        return GameTime;
    }
}
