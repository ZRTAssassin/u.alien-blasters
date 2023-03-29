using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int Coins;
    public int Health = 6;
}

[Serializable]
public class GameData
{
    string _gameName;
    public string GameName => _gameName;
    public GameData(string gameName)
    {
        _gameName = gameName;
    }
    public List<PlayerData> PlayerDatas = new List<PlayerData>();
}