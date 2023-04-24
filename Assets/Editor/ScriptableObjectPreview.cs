using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor;


public class ScriptableObjectPreview : EditorWindow
{
    private static ScriptableObjectPreview window;
    private static ScriptableObject scriptableObject;

    private static void Init()
    {
        window = (ScriptableObjectPreview)EditorWindow.GetWindow(typeof(ScriptableObjectPreview));
        window.Show();
    }

    [MenuItem("EnlitGames/ScriptableObject Preview")]
    public static void ShowWindow(MenuCommand command)
    {
        Init();        
    }

    private void OnGUI()
    {
        //dropdown for selecting scriptable object
        scriptableObject = (ScriptableObject)EditorGUILayout.ObjectField(scriptableObject, typeof(ScriptableObject), false);
        
        //Show nothing if scriptable object is not selected
        if (scriptableObject == null)
        {
            return;
        }
        
        //Get selected scriptable object type
        Type ScriptableObjectType = ByName(scriptableObject.GetType().Name);
        
        //Get all data of scriptable objects of selected type
        List<ScriptableObjectData> scriptableObjectDataList = new List<ScriptableObjectData>();
        var scriptableObjectPaths = AssetDatabase.FindAssets("t:" + scriptableObject.GetType().Name);
        foreach (var scriptableObjectPath in scriptableObjectPaths)
        {
            ScriptableObjectData scriptableObjectData = new ScriptableObjectData();
            //Get scriptable object
            scriptableObjectData.name = AssetDatabase.GUIDToAssetPath(scriptableObjectPath);
            scriptableObjectData.type = ScriptableObjectType.ToString();
            scriptableObjectData.scriptableObjectInstance = (ScriptableObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(scriptableObjectPath), ScriptableObjectType);
            var fields = ScriptableObjectType.GetFields();
            scriptableObjectData.fields.AddRange(fields);
            scriptableObjectDataList.Add(scriptableObjectData);
        }

        //show scriptable object data in editor
        foreach (var scriptableObjectData in scriptableObjectDataList)
        {
            ShowScriptableObjectInstance(scriptableObjectData);
        }

    }
    public static Type ByName(string name)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var tt = assembly.GetType(name);
            if (tt != null)
            {
                return tt;
            }
        }

        return null;
    }

    void ShowScriptableObjectInstance(ScriptableObjectData scriptableObjectData)
    {
        //show scriptable object data
        EditorGUILayout.LabelField(scriptableObjectData.name);

        foreach (var field in scriptableObjectData.fields)
        {
            var value = field.GetValue(scriptableObjectData.scriptableObjectInstance);
            Debug.Log(value.ToString());
            EditorGUILayout.LabelField(value.ToString());
        }
    }
}

class ScriptableObjectData
{
    public string name;
    public string type;
    public ScriptableObject scriptableObjectInstance;
    public List<FieldInfo> fields = new List<FieldInfo>();
}
