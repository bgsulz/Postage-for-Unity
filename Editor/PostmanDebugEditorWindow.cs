using UnityEditor;
using UnityEngine;

namespace Extra.Postman.Editor
{
    public class PostmanDebugEditorWindow : EditorWindow
    {
        const float MinTextWidth = 30;

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

        private static GUIStyle _labelRight;
        private static GUIStyle LabelRight => _labelRight ??= new(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleRight
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
                if (GUILayout.Button("Clear", style: EditorStyles.toolbarButton, GUILayout.Width(60))) Reports.ClearReports();
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
            var didClickButton = false;

            using (new EditorGUILayout.VerticalScope(style: Box))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField($"To: {report.Address.Key}", GUILayout.MinWidth(MinTextWidth), GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField($"[{report.ParcelTypeAsString}] {report.ParcelAsString}", style: LabelRight, GUILayout.MinWidth(MinTextWidth), GUILayout.ExpandWidth(true));
                }
                if (report.CallerInfo.SourceFilePath.EndsWith("MessageTrack.cs"))
                    EditorGUILayout.LabelField("Sent from Timeline");
                else
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

