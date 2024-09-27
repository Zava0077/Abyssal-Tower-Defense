using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteLoad")]
public class SpriteLoad : ScriptableObject
{
    [SerializeField] public List<Sprite> sprites;
}
