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
            scriptableObjectData.path = AssetDatabase.GUIDToAssetPath(scriptableObjectPath);
            scriptableObjectData.scriptableObjectInstance = (ScriptableObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(scriptableObjectPath), ScriptableObjectType);
            var fields = ScriptableObjectType.GetFields();
            scriptableObjectData.fields.AddRange(fields);
            scriptableObjectDataList.Add(scriptableObjectData);
        }

        //show scriptable object data in editor
        //scrollable horizontally
        EditorGUILayout.BeginVertical();
        ShowHeader(scriptableObjectDataList[0]);
        foreach (var scriptableObjectData in scriptableObjectDataList)
        {
            ShowScriptableObjectInstance(scriptableObjectData);
        }
        EditorGUILayout.EndVertical();

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

    void ShowHeader(ScriptableObjectData scriptableObjectData)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("ScriptableObject path", GUILayout.Width(300));
        EditorGUILayout.EndVertical();
        foreach (var field in scriptableObjectData.fields)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(field.Name, GUILayout.Width(100));
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    void ShowScriptableObjectInstance(ScriptableObjectData scriptableObjectData)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(scriptableObjectData.path, GUILayout.Width(300));
        EditorGUILayout.EndVertical();
        foreach (var field in scriptableObjectData.fields)
        {
            EditorGUILayout.BeginVertical();
            var value = field.GetValue(scriptableObjectData.scriptableObjectInstance);
            EditorGUILayout.LabelField(value.ToString(), GUILayout.Width(100));
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
}

class ScriptableObjectData
{
    public string name;
    public string path;
    public string type;
    public ScriptableObject scriptableObjectInstance;
    public List<FieldInfo> fields = new List<FieldInfo>();
}
