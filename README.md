# Postman

| [License](License.md) |
| - |

___

**Hassle-free event system for Unity projects.**
Shuttle around arbitrary data; use the Inspector as much or as little as you want.

---

## Quick start guide
Just call `Message.Send` and pass in an "address" and optionally an object.

```c#
using Extra.Postman;
using UnityEngine;

public class Sender : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Message.Send("key down", KeyCode.Space);
    }
}
```

Then receive it elsewhere by subscribing a method to the same "address."

```c#
using Extra.Postman;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    private void OnEnable()
    {
        // The type parameter filters received messages.
        // Only messages with data of type KeyCode will invoke PrintKeyPressed.
        Message.Subscribe<KeyCode>("key down", PrintKeyPressed);
    }
    
    private void OnDisable()
    {
        Message.Unsubscribe<KeyCode>("key down", PrintKeyPressed);
    }
    
    private void PrintKeyPressed(KeyCode code)
    {
        Debug.Log($"{code} was pressed!");
    }
}
```

---

## Convenience features

**Postman** is super compact, but there are several convenience features for cleaner and/or more Inspector-forward usage.

```cs
using Extra.Postman;
using UnityEngine;

// MessageBehaviour takes care of unsubscribing for you.
// In the Subscribe method, simply call subscribe on the MessageBus parameter 
// instead of Message.Subscribe().
public class FancyReceiver : MessageBehaviour
{
    // You can use a string or an AddressSO ScriptableObject as an address.
    // AddressField lets you toggle between the two in the Inspector.
    // Just pass it in like a normal address.
    [SerializeField] private AddressField address;
    
    protected override void Subscribe(MessageBus bus)
    {		
        // Subscribe with the type parameter "object"
        // to receive all messages with data of any type.
        bus.Subscribe<object>(address, PrintKeyPressed);
    }
    
    private void PrintKeyPressed(object data)
    {
        if (data is KeyCode code)
            Debug.Log($"{code} was pressed!");
        else
            Debug.Log($"Received data: {data}");
    }
}
```

---

## How do I add this to Unity?
It's easy!

#### If you have Git...
1. Open the Unity Editor. On the top ribbon, click Window > Package Manager.
2. Click the + button in the upper left corner, and click "Add package from git url..."
3. Enter "https://github.com/bgsulz/Postman-for-Unity.git"
4. Enjoy!

#### If you don't have Git (or want to modify the code)...
1. Click the big green "Code" button and click Download ZIP.
2. Extract the contents of the .zip into your Unity project's Assets folder.
3. Enjoy!
