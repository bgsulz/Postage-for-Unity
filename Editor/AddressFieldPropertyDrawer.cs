using System;
using UnityEditor;
using UnityEngine;

namespace Extra.Postman.Editor
{
    [CustomPropertyDrawer(typeof(AddressField))]
    public class AddressFieldPropertyDrawer : PropertyDrawer
    {
        private readonly GUIContent[] _dropdownOptions = new GUIContent[] { new("String"), new("ScriptableObject") };
        private GUIStyle _popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _popupStyle ??= new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
            {
                imagePosition = ImagePosition.ImageOnly
            };

            var indexProp = property.FindPropertyRelative("selectedIndex");
            var asStringProp = property.FindPropertyRelative("asString");
            var asSOProp = property.FindPropertyRelative("asSO");

            var buttonRect = position;
            buttonRect.width = EditorGUIUtility.singleLineHeight;

            var fieldRect = position;
            fieldRect.width = position.width - buttonRect.width;
            buttonRect.x = fieldRect.xMax;

            var oldIndex = indexProp.intValue;
            indexProp.intValue = EditorGUI.Popup(buttonRect, indexProp.intValue, _dropdownOptions, _popupStyle);
            if (indexProp.intValue != oldIndex)
            {
                if (indexProp.intValue == 0 && string.IsNullOrWhiteSpace(asStringProp.stringValue))
                {
                    if (asSOProp.objectReferenceValue)
                    {
                        asStringProp.stringValue = (asSOProp.objectReferenceValue as AddressSO).Key;
                    }
                }
            }

            switch (indexProp.intValue)
            {
                case 0:
                    EditorGUI.PropertyField(fieldRect, asStringProp, label);
                    break;
                case 1:
                    if (asSOProp.objectReferenceValue != null)
                    {
                        fieldRect.width -= EditorGUIUtility.singleLineHeight;

                        var addRect = buttonRect;
                        addRect.x = fieldRect.xMax;

                        if (GUI.Button(addRect, EditorGUIUtility.IconContent("CreateAddNew"), EditorStyles.iconButton))
                            CreateAndSaveAddressSO();
                    }
                    EditorGUI.PropertyField(fieldRect, asSOProp, label);
                    break;
                default: throw new IndexOutOfRangeException();
            }

            if (property.serializedObject.ApplyModifiedProperties())
                property.serializedObject.Update();
        }

        private void CreateAndSaveAddressSO()
        {
            var path = EditorUtility.SaveFilePanelInProject($"New Address SO...", "Address", "asset", "");

            if (string.IsNullOrWhiteSpace(path)) return;

            var instance = ScriptableObject.CreateInstance<AddressSO>();
            AssetDatabase.CreateAsset(instance, path);
            Selection.activeObject = instance;
        }
    }
}