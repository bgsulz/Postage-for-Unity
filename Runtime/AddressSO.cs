using System;
using UnityEngine;

namespace Extra.Postman
{
    [CreateAssetMenu(menuName = "Postman/Address")]
    public class AddressSO : ScriptableObject
    {
        [field: SerializeField] public string Key { get; private set; } = Guid.NewGuid().ToString();

        public static implicit operator Address(AddressSO so) => Address.Get(so.Key);

        public void RefreshKey() => Key = Guid.NewGuid().ToString();

        public void Send<T>() => Message.Send<T>(this);

        // For UnityEvents:
        public void Send() => Message.Send(this);
        public void Send(int p) => Message.Send(this, p);
        public void Send(float p) => Message.Send(this, p);
        public void Send(string p) => Message.Send(this, p);
        public void Send(bool p) => Message.Send(this, p);
        public void Send(Object p) => Message.Send(this, p);
    }
}