using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EnlitGames.ScriptableObjectTable
{
    public static class ColumnWidthCalculator
    {
        public static float FindScriptableObjectPathColumnWidth(List<ScriptableObjectData> scriptableObjectDataList)
        {
            float columnWidth = ConvertToColumnWidth("File Path");
            foreach(var scriptableObjectData in scriptableObjectDataList)
            {
                float fieldValueWidth = ConvertToColumnWidth(scriptableObjectData.path);
                if (fieldValueWidth > columnWidth)
                {
                    columnWidth = fieldValueWidth;
                }
            }
            return columnWidth;
        }

        public static List<float> FindColumnWidths(List<ScriptableObjectData> scriptableObjectDataList)
        {
            List<float> columnWidths = new List<float>();
            for(int i = 0; i < scriptableObjectDataList[0].fields.Count; i++)
            {
                columnWidths.Add(ConvertToColumnWidth(scriptableObjectDataList[0].fields[i].Name));
            }
            foreach(var scriptableObjectData in scriptableObjectDataList)
            {
                for(int i = 0; i < scriptableObjectData.fields.Count; i++)
                {
                    float fieldValueWidth = 0;
                    if(scriptableObjectData.fields[i].GetValue(scriptableObjectData.scriptableObjectInstance) != null)
                        fieldValueWidth = ConvertToColumnWidth(scriptableObjectData.fields[i].GetValue(scriptableObjectData.scriptableObjectInstance).ToString());
                    //if type is color, skip
                    if (scriptableObjectData.fields[i].FieldType == typeof(Color) || scriptableObjectData.fields[i].FieldType == typeof(Color32))
                        continue;
                    //if type is GameObject or Transform, skip
                    if (scriptableObjectData.fields[i].FieldType == typeof(GameObject) || scriptableObjectData.fields[i].FieldType == typeof(Transform))
                        continue;

                    if (fieldValueWidth > columnWidths[i])
                    {
                        columnWidths[i] = fieldValueWidth;
                    }
                }
            }
            return columnWidths;
        }

        public static float ConvertToColumnWidth(string text)
        {
            return text.Length * 6f + 30;
        }
    }
}