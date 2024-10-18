using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Extra.Postman.Timeline
{
    [TrackColor(0.2f, 0.8f, 0.8f)]
    [TrackClipType(typeof(MessageMarker))]
    [TrackBindingType(typeof(TimelineMessageReceiver))]
    public class MessageTrack : MarkerTrack
    {
    }

    public class MessageMarker : Marker, INotification, INotificationOptionProvider
    {
        public enum ParcelType
        {
            String,
            Int,
            Float,
            Object,
            Vector2,
            Vector2Int,
            Vector3,
            Vector3Int,
            Vector4,
            Color
        }

        [SerializeField] private bool retroactive;
        [SerializeField] private AddressField address;
        [SerializeField] private ParcelType type;

        // Surely C# will someday support discriminated unions.
        [SerializeField] private string asString;
        [SerializeField] private int asInt;
        [SerializeField] private float asFloat;
        [SerializeField] private Object asObject;
        [SerializeField] private Vector2 asVector2;
        [SerializeField] private Vector2Int asVector2Int;
        [SerializeField] private Vector3 asVector3;
        [SerializeField] private Vector3Int asVector3Int;
        [SerializeField] private Vector4 asVector4;
        [SerializeField] private Color asColor;

        public AddressField Address => address;
        public ParcelType Type => type;

        public NotificationFlags flags => retroactive ? NotificationFlags.Retroactive : 0;
        public PropertyName id { get; }

        public void Send()
        {
            switch (type)
            {
                case ParcelType.String:
                    Message.Send(address, asString); break;
                case ParcelType.Int:
                    Message.Send(address, asInt); break;
                case ParcelType.Float:
                    Message.Send(address, asFloat); break;
                case ParcelType.Object:
                    Message.Send(address, asObject); break;
                case ParcelType.Vector2:
                    Message.Send(address, asVector2); break;
                case ParcelType.Vector2Int:
                    Message.Send(address, asVector2Int); break;
                case ParcelType.Vector3:
                    Message.Send(address, asVector3); break;
                case ParcelType.Vector3Int:
                    Message.Send(address, asVector3Int); break;
                case ParcelType.Vector4:
                    Message.Send(address, asVector4); break;
                case ParcelType.Color:
                    Message.Send(address, asColor); break;
            }
        }
    }
}
