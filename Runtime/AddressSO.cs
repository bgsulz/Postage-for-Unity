using System;
using UnityEngine;

namespace Extra.Postman
{
    [CreateAssetMenu(menuName = "Postman/Address")]
    public class AddressSO : ScriptableObject
    {
        [SerializeField] private string key = Guid.NewGuid().ToString();
        public string Key => key;

        public static implicit operator Address(AddressSO so) => Address.Get(so.Key);

        public void RefreshKey() => Key = Guid.NewGuid().ToString();

        public void Send<T>(T p) => Message.Send<T>(this, p);

        // For UnityEvents:
        public void Send() => Message.Send(this);
        public void Send(int p) => Message.Send(this, p);
        public void Send(float p) => Message.Send(this, p);
        public void Send(string p) => Message.Send(this, p);
        public void Send(bool p) => Message.Send(this, p);
        public void Send(UnityEngine.Object p) => Message.Send(this, p);
    }
}