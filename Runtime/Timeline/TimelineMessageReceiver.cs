using UnityEngine;
using UnityEngine.Playables;

namespace Extra.Postman.Timeline
{
    public class TimelineMessageReceiver : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is not MessageMarker postmanMarker) return;
            Message.Send(postmanMarker.AddressField, postmanMarker.Parcel);
        }
    }
}
