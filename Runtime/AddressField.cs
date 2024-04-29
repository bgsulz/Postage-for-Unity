using System;
using UnityEngine;

namespace Extra.Postman
{
    [Serializable]
    public class AddressField
    {
        [SerializeField] private int selectedIndex;
        [SerializeField] private string asString;
        [SerializeField] private AddressSO asSO;

        public static implicit operator Address(AddressField field)
        {
            return field.selectedIndex switch
            {
                0 => Address.Get(field.asString),
                1 => Address.Get(field.asSO.Key),
                _ => throw new IndexOutOfRangeException()
            };
        }
    }
}