namespace JengaDemo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Utilities : MonoBehaviour
    {
        public static string WrapJson(string jsonToBe)
        {
            return "{\"Items\":" + jsonToBe + "}";
        }

        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
