using UnityEngine;
using System.Collections.Generic;
using System.Reflection;



public class ScriptableObjectData
{
    public string name;
    public string path;
    public string type;
    public ScriptableObject scriptableObjectInstance;
    public List<FieldInfo> fields = new List<FieldInfo>();
}