using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Extra.Postman.Timeline.MessageMarker;

namespace Extra.Postman.Timeline.Editor
{
    [CustomEditor(typeof(MessageMarker))]
    public class MessageMarkerEditor : UnityEditor.Editor
    {
        private MessageMarker _target;
        private MessageMarker Target => _target ? _target : _target = target as MessageMarker;

        private string[] _excludedProperties = {
            "m_Script",
        };

        private ParcelType _prevValue;

        private SerializedProperty Prop(string name) => serializedObject.FindProperty(name);

        public override void OnInspectorGUI()
        {
            if (_excludedProperties.Length == 1)
            {
                var propNames = Enum.GetNames(typeof(ParcelType)).Select(x => $"as{x}");
                _excludedProperties = _excludedProperties.Concat(propNames).ToArray();
            }

            DrawPropertiesExcluding(serializedObject, _excludedProperties);

            if (_prevValue != Target.Type)
            {
                TryTransferValue(_prevValue, Target.Type);
                _prevValue = Target.Type;
            }

            var title = new GUIContent("Parcel");
            var propName = $"as{Target.Type}";
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propName), title);
        }

        private void TryTransferValue(ParcelType prev, ParcelType curr)
        {
            Debug.Log("Trying to transfer value from " + prev + " to " + curr);

            if (prev == ParcelType.Int && curr == ParcelType.Float)
            {
                Prop("asFloat").floatValue = Prop("asInt").intValue;
                return;
            }

            if (prev == ParcelType.Float && curr == ParcelType.Int)
            {
                Prop("asInt").intValue = (int)Prop("asFloat").floatValue;
                return;
            }

            if (prev == ParcelType.Vector2 && curr == ParcelType.Vector2Int)
            {
                var v2 = Prop("asVector2").vector2Value;
                Prop("asVector2Int").vector2IntValue = new((int)v2.x, (int)v2.y);
                return;
            }

            if (prev == ParcelType.Vector2Int && curr == ParcelType.Vector2)
            {
                Prop("asVector2").vector2Value = Prop("asVector2Int").vector2IntValue;
                return;
            }

            if (prev == ParcelType.Vector3 && curr == ParcelType.Vector3Int)
            {
                var v3 = Prop("asVector3").vector3Value;
                Prop("asVector3Int").vector3IntValue = new((int)v3.x, (int)v3.y, (int)v3.z);
                return;
            }

            if (prev == ParcelType.Vector3Int && curr == ParcelType.Vector3)
            {
                Prop("asVector3").vector3Value = Prop("asVector3Int").vector3IntValue;
                return;
            }
        }
    }
}
