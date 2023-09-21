using System.Runtime.Serialization;
using UnityEngine;

namespace Assets.Scripts.SaveSystem
{
    internal class TransformSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Transform transform = (Transform)obj;

            info.AddValue(nameof(transform.position), transform.position);
            info.AddValue(nameof(transform.rotation), transform.rotation);
            info.AddValue(nameof(transform.localScale), transform.localScale);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector surrogateSelector)
        {
            Transform transform = (Transform)obj;

            transform.position =  (Vector3)info.GetValue(nameof(transform.position), typeof(Vector3));
            transform.rotation = (Quaternion)info.GetValue(nameof(transform.rotation), typeof(Quaternion));
            transform.localScale =  (Vector3)info.GetValue(nameof(transform.localScale), typeof(Vector3));

            obj = transform;

            return obj;
        }
    }
}