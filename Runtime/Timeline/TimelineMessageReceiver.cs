using UnityEngine;
using UnityEngine.Playables;

namespace Extra.Postage.Timeline
{
    public class TimelineMessageReceiver : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is not MessageMarker PostageMarker) return;
            PostageMarker.Send();
        }
    }
}
