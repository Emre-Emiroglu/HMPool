#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CodeCatGames.HMPool.Runtime
{
    /// <summary>
    /// Custom property drawer for the PoolDatum structure in the Unity Editor.
    /// This drawer customizes how the fields of PoolDatum are displayed in the Inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(PoolDatum))]
    public sealed class PoolDatumDrawer : PropertyDrawer
    {
        #region Fields
        private string[] _typeNames;
        private string[] _displayNames;
        private int _selectedIndex;
        #endregion

        #region Core
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty classTypeNameProp = property.FindPropertyRelative("classTypeName");

            if (_typeNames == null)
                LoadTypes();

            _selectedIndex = Mathf.Max(0, Array.IndexOf(_typeNames!, classTypeNameProp.stringValue));

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            float y = position.y;

            SerializedProperty isMonoProp = property.FindPropertyRelative("isMono");
            
            float isMonoHeight = EditorGUI.GetPropertyHeight(isMonoProp);
            
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, isMonoHeight), isMonoProp);
            
            y += isMonoHeight + 2;
            
            bool isMono = isMonoProp.boolValue;
            
            if (isMono)
            {
                SerializedProperty monoPrefabProp = property.FindPropertyRelative("monoPrefab");
                
                float monoPrefabHeight = EditorGUI.GetPropertyHeight(monoPrefabProp);
                
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, monoPrefabHeight), monoPrefabProp);
                
                y += monoPrefabHeight + 2;
            }
            
            if (_typeNames.Length > 0)
            {
                int newIndex =
                    EditorGUI.Popup(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight),
                        "ClassType", _selectedIndex, _displayNames);
    
                if (EditorGUI.EndChangeCheck())
                {
                    _selectedIndex = newIndex;
                        
                    classTypeNameProp.stringValue = _typeNames[_selectedIndex];
                }
                
                y += EditorGUIUtility.singleLineHeight + 2;
            }

            string[] fieldNames = { "initialSize", "defaultCapacity", "maximumSize" };
            
            foreach (string name in fieldNames)
            {
                SerializedProperty prop = property.FindPropertyRelative(name);
                
                float propHeight = EditorGUI.GetPropertyHeight(prop);
                
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, propHeight), prop);
                
                y += propHeight + 2;
            }

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0;

            SerializedProperty isMonoProp = property.FindPropertyRelative("isMono");
            
            height += EditorGUI.GetPropertyHeight(isMonoProp) + 2;

            if (isMonoProp.boolValue)
            {
                SerializedProperty monoPrefabProp = property.FindPropertyRelative("monoPrefab");
                
                height += EditorGUI.GetPropertyHeight(monoPrefabProp) + 2;
            }
            else
                height += EditorGUIUtility.singleLineHeight + 2;


            string[] fieldNames = { "initialSize", "defaultCapacity", "maximumSize" };
            height += fieldNames.Select(property.FindPropertyRelative)
                .Select(prop => EditorGUI.GetPropertyHeight(prop) + 8).Sum();

            return height;
        }
        #endregion

        #region Executes
        private void LoadTypes()
        {
            List<Type> poolableTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t =>
                t.IsClass && !t.IsAbstract && (typeof(IPoolable).IsAssignableFrom(t) ||
                                               (t.BaseType != null &&
                                                typeof(PurePoolable).IsAssignableFrom(t.BaseType)) ||
                                               (t.BaseType != null &&
                                                typeof(MonoPoolable).IsAssignableFrom(t.BaseType)))).ToList();

            _typeNames = poolableTypes.Select(t => t.AssemblyQualifiedName).ToArray();
            _displayNames = poolableTypes.Select(t => t.Name).ToArray();
        }
        #endregion
    }
}
#endif
