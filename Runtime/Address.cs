using UnityEngine;
using System.Collections.Generic;

namespace Extra.Postage
{
    public class Address
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _knownAddresses = new();
        }

        private static Dictionary<string, Address> _knownAddresses = new();

        public string Key { get; }
        public Address(string key) => Key = key;

        public static Address Get(string key)
        {
            if (_knownAddresses.TryGetValue(key, out var address)) return address;
            var newAddress = new Address(key);
            _knownAddresses.Add(key, newAddress);
            return newAddress;
        }

        public static implicit operator string(Address address) => address.Key;
        public static implicit operator Address(string key) => Get(key);
        public static bool operator ==(Address a, Address b) => a.Equals(b);
        public static bool operator !=(Address a, Address b) => !(a == b);

        public override bool Equals(object obj) => obj is Address address && address.Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
    }
}