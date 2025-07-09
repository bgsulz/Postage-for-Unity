using UnityEngine;
using UnityEditor;

namespace Extra.Postage.Editor
{
    [CustomEditor(typeof(AddressSO))]
    public class AddressSOEditor : UnityEditor.Editor
    {
        private SerializedProperty _keyProperty;

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); 
            _keyProperty = serializedObject.FindProperty("key");

            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(_keyProperty);
                GUI.enabled = true;

                var button = GUILayout.Button(EditorGUIUtility.IconContent("Refresh"),
                                              EditorStyles.iconButton,
                                              GUILayout.Height(EditorGUIUtility.singleLineHeight));

                if (button)
                {
                    Undo.RecordObject(target, "Refresh Address Key");  
                    RefreshKeyViaSerializedProperty();  
                }
            }

            serializedObject.ApplyModifiedProperties(); 
        }

        private void RefreshKeyViaSerializedProperty() 
            => _keyProperty.stringValue = System.Guid.NewGuid().ToString();
    }
}