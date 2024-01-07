using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace H2V.ExtensionsCore.Editor.Helpers
{
    public static class ObjectPrivateHandler
    {
        /// <summary>
        /// Use this to set private fields and properties.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="isField"></param>
        public static void SetPrivateProperty(this Object obj, string propertyName,
            object value, bool isField = false)
        {
            var serializedObject = new SerializedObject(obj);
            var correctPropertyName = isField ? $"<{propertyName}>k__BackingField" : propertyName;
            var property = serializedObject.FindProperty(correctPropertyName);
            property.boxedValue = value;
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        public static void SetPrivateArrayProperty(this Object obj, string propertyName,
            object[] values, bool isField = false)
        {
            var serializedObject = new SerializedObject(obj);
            var correctPropertyName = isField ? $"<{propertyName}>k__BackingField" : propertyName;
            var property = serializedObject.FindProperty(correctPropertyName);

            for (int i = 0; i < values.Length; i++)
            {
                property.InsertArrayElementAtIndex(0);
                property.GetArrayElementAtIndex(i).boxedValue = values[i];
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        /// <summary>
        /// Use this to set private method of an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        public static void CallPrivateMethod(this Object obj, string methodName,
            params object[] parameters)
        {
            var method = obj.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(obj, parameters);
        }
    }
}