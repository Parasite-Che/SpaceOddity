using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName ="Coin",menuName ="ScriptableObject/Coin")]
public class CoinScriptableObject : ScriptableObject
{
    public int reward;
    public Sprite sprite;
}
