using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteamProfileDisplay : MonoBehaviour
{
    [SerializeField] RawImage _profilePicture;
    [SerializeField] TextMeshProUGUI _profileName;
    [SerializeField] SteamId _steamIdId;

    public SteamId SteamId => _steamIdId;
    

    public void SetProfilePicture(Texture2D newImage)
    {
        _profilePicture.texture = newImage;
    }

    public void SetProfileName(string newName)
    {
        _profileName.text = newName;
    }

    public void SetSteamId(SteamId id)
    {
        _steamIdId = id;
    }
}
