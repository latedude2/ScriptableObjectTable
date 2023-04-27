using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Reflection;
using System;
namespace EnlitGames.ScriptableObjectTable
{
    public class ScriptableObjectPreview : EditorWindow
    {
        static ScriptableObject selectedScriptableObject;
        
        [MenuItem("Enlit Games/Scriptable Object Table")]
        public static void ShowExample()
        {
            var wnd = GetWindow<ScriptableObjectPreview>();
            wnd.titleContent = new GUIContent("Scriptable Object Table");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object.
            VisualElement root = rootVisualElement;

            // Import UXML.
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.enlit-games.scriptable-object-table/Editor/ScriptableObjectPreview.uxml");
            VisualElement ScrollViewExample = visualTree.Instantiate();
            root.Add(ScrollViewExample);
            

            ObjectField ScriptableObjectSelection = root.Query<ObjectField>("ScriptableObjectSelection");
            

            ScriptableObjectSelection.RegisterValueChangedCallback((evt) => { PopulateTable(evt); });

            ScriptableObjectSelection.value = selectedScriptableObject;
        }

        void PopulateTable(ChangeEvent<UnityEngine.Object> evt)
        {
            VisualElement root = rootVisualElement;
            VisualElement scrollview = root.Query<ScrollView>("scroll-view-wrap-example");
            scrollview.Clear();
            if (evt.newValue != null)
            {
                ScriptableObject scriptableObject = (ScriptableObject)evt.newValue;
                selectedScriptableObject = scriptableObject;
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

        

        void ShowHeader(ScriptableObjectData scriptableObjectData, VisualElement scrollview, float pathColumnWidth, List<float> columnWidths)
        {
            VisualElement headerRow = new VisualElement();
            headerRow.style.flexDirection = FlexDirection.Row;
            scrollview.Add(headerRow);
            Label pathHeader = new Label("File Path");
            pathHeader.style.width = pathColumnWidth;
            headerRow.Add(pathHeader);
            for(int i = 0; i < scriptableObjectData.fields.Count; i++)
            {
                
                Label fieldHeader = new Label(scriptableObjectData.fields[i].Name);
                fieldHeader.style.width = columnWidths[i];

                if(i % 2 == 0)
                {
                    fieldHeader.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
                }
                headerRow.Add(fieldHeader);
            }
        }

        void ShowScriptableObjectInstance(ScriptableObjectData scriptableObjectData, VisualElement scrollview, float columnWidth, List<float> columnWidths)
        {
            VisualElement scriptableObjectInstanceRow = new VisualElement();
            scriptableObjectInstanceRow.style.flexDirection = FlexDirection.Row;
            scrollview.Add(scriptableObjectInstanceRow);
            Label pathLabel = new Label(scriptableObjectData.path);
            pathLabel.style.width = columnWidth;
            scriptableObjectInstanceRow.Add(pathLabel);
            for(int i = 0; i < scriptableObjectData.fields.Count; i++)
            {
                
                VisualElement element = MakeVisualElementForValue(scriptableObjectData.fields[i].GetValue(scriptableObjectData.scriptableObjectInstance));
                
                SerializedObject so = new SerializedObject(scriptableObjectData.scriptableObjectInstance);

                if(element is IBindable)
                {
                    SerializedProperty property = so.FindProperty(scriptableObjectData.fields[i].Name);
                    ((IBindable)element).BindProperty(property);
                }
                

                element.style.width = columnWidths[i];

                //set background of every second column to grey
                if(i % 2 == 0)
                {
                    element.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
                }
                scriptableObjectInstanceRow.Add(element);
            }
        }

        VisualElement MakeVisualElementForValue(object value)
        {
            if(value == null)
            {
                return new Label("null");
            }
            VisualElement visualElement = new Label(value.ToString());
            if(value.GetType() == typeof(UnityEngine.Color))
            {
                visualElement = new ColorField();
                ((ColorField)visualElement).SetValueWithoutNotify((Color)value);
            }
            if(value.GetType() == typeof(UnityEngine.Color32))
            {
                visualElement = new ColorField();
                ((ColorField)visualElement).SetValueWithoutNotify((Color32)value);
            }
            if(value.GetType() == typeof(UnityEngine.Vector2))
            {
                visualElement = new Vector2Field();
                ((Vector2Field)visualElement).SetValueWithoutNotify((Vector2)value);
            }
            if(value.GetType() == typeof(UnityEngine.Vector3))
            {
                visualElement = new Vector3Field();
                ((Vector3Field)visualElement).SetValueWithoutNotify((Vector3)value);
            }
            if(value.GetType() == typeof(UnityEngine.Vector4))
            {
                visualElement = new Vector4Field();
                ((Vector4Field)visualElement).SetValueWithoutNotify((Vector4)value);
            }
            if(value.GetType() == typeof(UnityEngine.Rect))
            {
                visualElement = new RectField();
                ((RectField)visualElement).SetValueWithoutNotify((Rect)value);
            }
            if(value.GetType() == typeof(UnityEngine.Bounds))
            {
                visualElement = new BoundsField();
                ((BoundsField)visualElement).SetValueWithoutNotify((Bounds)value);
            }
            if(value.GetType() == typeof(UnityEngine.Object))
            {
                visualElement = new ObjectField();
                ((ObjectField)visualElement).SetValueWithoutNotify((UnityEngine.Object)value);
            }
            if(value.GetType() == typeof(UnityEngine.GameObject))
            {
                visualElement = new ObjectField();
                ((ObjectField)visualElement).SetValueWithoutNotify((UnityEngine.GameObject)value);
            }
            if(value.GetType() == typeof(UnityEngine.Component))
            {
                visualElement = new ObjectField();
                ((ObjectField)visualElement).SetValueWithoutNotify((UnityEngine.Component)value);
            }
            if(value.GetType() == typeof(UnityEngine.Transform))
            {
                visualElement = new ObjectField();
                ((ObjectField)visualElement).SetValueWithoutNotify((UnityEngine.Transform)value);
            }
            if(value.GetType() == typeof(UnityEngine.AnimationCurve))
            {
                visualElement = new CurveField();
                ((CurveField)visualElement).SetValueWithoutNotify((AnimationCurve)value);
            }
            if(value.GetType() == typeof(UnityEngine.Gradient))
            {
                visualElement = new GradientField();
                ((GradientField)visualElement).SetValueWithoutNotify((Gradient)value);
            }
            if(value.GetType() == typeof(UnityEngine.LayerMask))
            {
                visualElement = new LayerMaskField();
                ((LayerMaskField)visualElement).SetValueWithoutNotify((LayerMask)value);
            }
            if(value.GetType() == typeof(UnityEngine.RectInt))
            {
                visualElement = new RectIntField();
                ((RectIntField)visualElement).SetValueWithoutNotify((RectInt)value);
            }
            if(value.GetType() == typeof(UnityEngine.BoundsInt))
            {
                visualElement = new BoundsIntField();
                ((BoundsIntField)visualElement).SetValueWithoutNotify((BoundsInt)value);
            }
            if(value.GetType() == typeof(Enum))
            {
                visualElement = new EnumField();
                ((EnumField)visualElement).SetValueWithoutNotify((Enum)value);
            }
            if(value.GetType() == typeof(bool))
            {
                visualElement = new Toggle();
                ((Toggle)visualElement).SetValueWithoutNotify((bool)value);
            }
            if(value.GetType() == typeof(int))
            {
                visualElement = new IntegerField();
                ((IntegerField)visualElement).SetValueWithoutNotify((int)value);
            }
            if(value.GetType() == typeof(float))
            {
                visualElement = new FloatField();
                ((FloatField)visualElement).SetValueWithoutNotify((float)value);
            }
            if(value.GetType() == typeof(double))
            {
                visualElement = new DoubleField();
                ((DoubleField)visualElement).SetValueWithoutNotify((double)value);
            }
            if(value.GetType() == typeof(string) || value.GetType() == typeof(String) || value.GetType() == typeof(char))
            {
                visualElement = new TextField();
                ((TextField)visualElement).SetValueWithoutNotify((string)value);
            }


            
            
            
            return visualElement;
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
}