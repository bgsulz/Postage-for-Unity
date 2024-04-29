using UnityEngine;
using UnityEditor;

namespace Extra.Postman.Editor
{
    [CustomEditor(typeof(AddressSO))]
    public class AddressSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("<Key>k__BackingField"));
                GUI.enabled = true;

                var button = GUILayout.Button(EditorGUIUtility.IconContent("Refresh"),
                                              EditorStyles.iconButton,
                                              GUILayout.Height(EditorGUIUtility.singleLineHeight));

                if (button)
                {
                    (target as AddressSO).RefreshKey();
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                }
            }
        }
    }
}