using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace EnlitGames.ScriptableObjectTable
{
    public class ScriptableObjectData
    {
        public string name;
        public string path;
        public string type;
        public ScriptableObject scriptableObjectInstance;
        public List<FieldInfo> fields = new List<FieldInfo>();
    }
}