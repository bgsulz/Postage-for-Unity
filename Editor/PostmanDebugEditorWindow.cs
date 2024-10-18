using UnityEditor;
using UnityEngine;

namespace Extra.Postman.Editor
{
    public class PostmanDebugEditorWindow : EditorWindow
    {
        private static GUIStyle _box;
        private static GUIStyle Box => _box ??= new(EditorStyles.helpBox)
        {
            margin = new(2, 2, 2, 2),
            padding = new(4, 4, 4, 4)
        };

        private static GUIStyle _link;
        private static GUIStyle Link => _link ??= new(EditorStyles.linkLabel)
        {
            margin = new(0, 0, 0, 0),
            padding = new(0, 0, 0, 0)
        };

        private Vector2 _scrollPosition;

        [MenuItem("Window/Postman Debug")]
        public static void ShowWindow()
        {
            GetWindow<PostmanDebugEditorWindow>("Postman Debug");
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Clear", GUILayout.Width(60))) Reports.ClearReports();
            }

            using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {
                _scrollPosition = scroll.scrollPosition;
                foreach (var report in Reports.StoredReports)
                    DrawReport(report);
            }

            if (Application.isPlaying)
                Repaint();
        }

        private void DrawReport(Report report)
        {
            bool didClickButton;

            using (new EditorGUILayout.VerticalScope(style: Box))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField($"Address: {report.Address.Key}", GUILayout.MinWidth(60), GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField($"Parcel: {report.Parcel}", GUILayout.MinWidth(60), GUILayout.ExpandWidth(true));
                }
                didClickButton = GUILayout.Button(report.CallerInfo.ToString(), Link);
            }

            if (didClickButton)
                OpenCallerAsset(report.CallerInfo.TruncatedPath, report.CallerInfo);
        }

        private void OpenCallerAsset(string truncatedPath, CallerInfo callerInfo)
        {
            var id = AssetDatabase.LoadAssetAtPath<TextAsset>(truncatedPath)?.GetInstanceID() ?? null;

            if (id == null)
            {
                Debug.LogWarning("The caller asset could not be found.");
                return;
            }

            else AssetDatabase.OpenAsset(id!.Value, callerInfo.SourceLineNumber);
        }
    }
}

