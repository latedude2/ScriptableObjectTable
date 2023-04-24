using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Example Data",menuName = "Example Data",order = 0)]
public class ExampleScriptableObject : ScriptableObject
{        
    [SerializeField] public string cardName;

    [SerializeField] public int manaCost;
}
