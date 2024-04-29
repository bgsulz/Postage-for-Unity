using System;
using UnityEngine;

namespace Extra.Postman
{
    public abstract class Recipient
    {
        public abstract void Invoke<R>(R parcel);
    }

    public readonly struct Empty
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => Instance = new();

        public static Empty Instance = new();
    }

    public class RecipientEmpty : Recipient
    {
        public Action Action { get; }
        public RecipientEmpty(Action action) => Action = action;

        public override void Invoke<R>(R parcel)
        {
            if (parcel is Empty)
                Action.Invoke();
        }
    }

    public class Recipient<T> : Recipient
    {
        public Action<T> Action { get; }
        public Recipient(Action<T> action) => Action = action;

        public override void Invoke<R>(R parcel)
        {
            if (parcel is T parcelAsT)
                Action.Invoke(parcelAsT);
        }
    }
}