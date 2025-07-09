## Send and receive messages
#### Zero baloney.

Call `Message.Send` with an address (a string) and optionally a parameter.

Addresses can also be `AddressSO` ScriptableObjects.

The cleanest way to do this is to inherit from MessageBehaviour, which automatically unsubscribes when the object is disabled.

```cs
public class SendAndReceive : MessageBehaviour
{
    [SerializeField] private AddressField address;

    protected override void Subscribe(MessageBus bus) => bus.Subscribe<string>(address, Debug.Log);

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        Message.Send(address, Time.time.ToString());
    }
}
```

If you don't have that option, simply call `Message.Subscribe` and `Message.Unsubscribe` instead.

```cs
public class SendAndReceive : MonoBehaviour
{
    [SerializeField] private AddressField address;

    private void OnEnable() => Message.Subscribe<string>(address, Debug.Log);
	private void OnDisable() => Message.Unsubscribe<string>(address, Debug.Log);

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        Message.Send(address, Time.time.ToString());
    }
}
```

## Debug incoming messages
#### No more "why is that happening?"

Open the Postage Debug window (`Window > Postage Debug`) to see a live-updating record of all incoming events.

## Send messages from the timeline
#### A simpler alternative to Signal Emitters.

Add a `MessageTrack` to a Unity Timeline, bind it to a `TimelineMessageReceiver`, and right click to add Message Markers. These will call `Message.Send` when played.