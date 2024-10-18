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
        [SerializeField] private bool retroactive;
        [SerializeField] private AddressField addressField;
        [SerializeField] private string parcel;

        public AddressField AddressField => addressField;
        public string Parcel => parcel;

        public NotificationFlags flags => retroactive ? NotificationFlags.Retroactive : 0;
        public PropertyName id { get; }
    }
}
