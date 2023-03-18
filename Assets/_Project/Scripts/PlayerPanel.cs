using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Player;
using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    Player _player;

    public void Bind(Player player)
    {
        _player = player;
    }

    void Update()
    {
        _text.SetText(_player.Coins.ToString());
    }
}
