using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Extra.Postman
{
    public static class Message
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _responses = new();
        }

        private static Dictionary<Address, List<Recipient>> _responses = new();

        #region Empty

        public static void Subscribe(Address address, Action action)
        {
            if (!_responses.ContainsKey(address))
                _responses.Add(address, new());

            _responses[address].Add(new RecipientEmpty(action));
        }

        public static void Unsubscribe(Address address, Action action)
        {
            if (!_responses.ContainsKey(address))
                return;

            var index = _responses[address].FindIndex(x => x is RecipientEmpty e && e.Action == action);
            if (index >= 0)
                _responses[address].RemoveAt(index);
        }

        public static void Send(Address address) => Send(address, Empty.Instance);

        #endregion

        #region Parameter

        public static void Subscribe<R>(Address address, Action<R> action)
        {
            if (!_responses.ContainsKey(address))
                _responses.Add(address, new());

            _responses[address].Add(new Recipient<R>(action));
        }

        public static void Unsubscribe<R>(Address address, Action<R> action)
        {
            if (!_responses.ContainsKey(address))
                return;

            var index = _responses[address].FindIndex(x => x is Recipient<R> r && r.Action == action);
            if (index >= 0)
                _responses[address].RemoveAt(index);
        }

        public static void Send<R>(
            Address address,
            R parcel
#if UNITY_EDITOR
            ,
            [CallerMemberName] string memberName = default,
            [CallerFilePath] string sourceFilePath = default,
            [CallerLineNumber] int sourceLineNumber = default
#endif
        )
        {
#if UNITY_EDITOR
            Reports.Report(memberName, sourceFilePath, sourceLineNumber, address, parcel);
#endif

            if (!_responses.TryGetValue(address, out var recipients))
                return;

            foreach (var item in recipients)
                item.Invoke(parcel);
        }

        #endregion
    }
}