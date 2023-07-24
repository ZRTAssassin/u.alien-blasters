using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Animator Sprite List")]
public class AnimatorSpriteList : ScriptableObject
{
    public List<Sprite> _sprites = new();
}
