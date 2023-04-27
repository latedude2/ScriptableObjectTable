using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Reflection;
using System;

public class ScriptableObjectPreview : EditorWindow
{
    [MenuItem("Enlit Games/Scriptable Object Table")]
    public static void ShowExample()
    {
        var wnd = GetWindow<ScriptableObjectPreview>();
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object.
        VisualElement root = rootVisualElement;

        // Import UXML.
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ScriptableObjectPreview.uxml");
        VisualElement ScrollViewExample = visualTree.Instantiate();
        root.Add(ScrollViewExample);

        // Find the scroll view by name.
        

        ObjectField ScriptableObjectSelection = root.Query<ObjectField>("ScriptableObjectSelection");

        ScriptableObjectSelection.RegisterValueChangedCallback((evt) => { PopulateTable(evt); });
            
    }

    void PopulateTable(ChangeEvent<UnityEngine.Object> evt)
    {
        VisualElement root = rootVisualElement;
        VisualElement scrollview = root.Query<ScrollView>("scroll-view-wrap-example");
        scrollview.Clear();
        if (evt.newValue != null)
        {
            ScriptableObject scriptableObject = (ScriptableObject)evt.newValue;
            ShowSelectedScriptableObject(scriptableObject, scrollview);
        }
    }

    void ShowSelectedScriptableObject(ScriptableObject scriptableObject, VisualElement scrollview)
    {
        List<ScriptableObjectData> scriptableObjectDataList = GetScriptableObjectDataList(scriptableObject);

        float pathColumnWidth = ColumnWidthCalculator.FindScriptableObjectPathColumnWidth(scriptableObjectDataList);
        List<float> columnWidths = ColumnWidthCalculator.FindColumnWidths(scriptableObjectDataList);
        

        ShowHeader(scriptableObjectDataList[0], scrollview, pathColumnWidth, columnWidths);
        for(int i = 0; i < scriptableObjectDataList.Count; i++)
        {
            ShowScriptableObjectInstance(scriptableObjectDataList[i], scrollview, pathColumnWidth, columnWidths);
        }
        
    }

    void ShowScriptableObjectInstance(ScriptableObjectData scriptableObjectData, VisualElement scrollview, float columnWidth, List<float> columnWidths)
    {
        VisualElement scriptableObjectInstance = new VisualElement();
        scriptableObjectInstance.style.flexDirection = FlexDirection.Row;
        scrollview.Add(scriptableObjectInstance);
        Label pathLabel = new Label(scriptableObjectData.path);
        pathLabel.style.width = columnWidth;
        scriptableObjectInstance.Add(pathLabel);
        for(int i = 0; i < scriptableObjectData.fields.Count; i++)
        {
            Label fieldLabel = new Label(scriptableObjectData.fields[i].GetValue(scriptableObjectData.scriptableObjectInstance).ToString());
            fieldLabel.style.width = columnWidths[i];
            scriptableObjectInstance.Add(fieldLabel);
        }
    }

    void ShowHeader(ScriptableObjectData scriptableObjectData, VisualElement scrollview, float pathColumnWidth, List<float> columnWidths)
    {
        VisualElement header = new VisualElement();
        header.style.flexDirection = FlexDirection.Row;
        scrollview.Add(header);
        Label pathHeader = new Label("File Path");
        pathHeader.style.width = pathColumnWidth;
        header.Add(pathHeader);
        for(int i = 0; i < scriptableObjectData.fields.Count; i++)
        {
            Label fieldHeader = new Label(scriptableObjectData.fields[i].Name);
            fieldHeader.style.width = columnWidths[i];
            header.Add(fieldHeader);
        }
    }



    public static Type GetTypeFromName(string name)
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

    List<ScriptableObjectData> GetScriptableObjectDataList(ScriptableObject scriptableObject)
    {
        Type ScriptableObjectType = GetTypeFromName(scriptableObject.GetType().Name);
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

        return scriptableObjectDataList;
    }

}