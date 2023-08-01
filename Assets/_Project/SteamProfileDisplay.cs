using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteamProfileDisplay : MonoBehaviour
{
    [SerializeField] RawImage _profilePicture;
    [SerializeField] TextMeshProUGUI _profileName;
    

    public void SetProfilePicture(Texture2D newImage)
    {
        _profilePicture.texture = newImage;
    }

    public void SetProfileName(string newName)
    {
        _profileName.text = newName;
    }
}
