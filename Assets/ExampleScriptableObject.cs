using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Example Data",menuName = "Example Data",order = 0)]
public class ExampleScriptableObject : ScriptableObject
{        
    [SerializeField] public string cardName;

    [SerializeField] public int manaCost;

    [SerializeField] public Color cardColor;

    //different types to test functionality
    [SerializeField] public int intTest;
    [SerializeField] public float floatTest;
    [SerializeField] public string stringTest;
    [SerializeField] public bool boolTest;
    [SerializeField] public Vector2 vector2Test;
    [SerializeField] public Vector3 vector3Test;
    [SerializeField] public Vector4 vector4Test;
    [SerializeField] public Color colorTest;
    [SerializeField] public Color32 color32Test;
    [SerializeField] public Transform transformTest;
    [SerializeField] public GameObject gameObjectTest;
    [SerializeField] public ExampleScriptableObject scriptableObject;
    [SerializeField] public int[] intArray;
    [SerializeField] public ExampleScriptableObject[] scriptableObjectArray;
    [SerializeField] public Sprite exampleSprite;
    [SerializeField] public NestedClass nestedClassTest;
    
}

[Serializable]
public class NestedClass
{
    [SerializeField] int testNestedInt;
}
