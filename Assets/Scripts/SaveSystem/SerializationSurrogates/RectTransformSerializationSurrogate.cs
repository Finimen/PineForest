using System.Runtime.Serialization;
using UnityEngine;

namespace Assets.Scripts.SaveSystem
{
    internal class RectTransformSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            RectTransform rectTransform = (RectTransform)obj;

            info.AddValue("rect" + "position", rectTransform.position);
            info.AddValue("rect" + "rotation", rectTransform.rotation);
            info.AddValue("rect" + "scale", rectTransform.localScale);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector surrogateSelector)
        {
            RectTransform rectTransform = (RectTransform)obj;

            rectTransform.position = (Vector3)info.GetValue("rect" + "position", typeof(Vector3));
            rectTransform.rotation = (Quaternion)info.GetValue("rect" + "rotation", typeof(Quaternion));
            rectTransform.localScale = (Vector3)info.GetValue("rect" + "scale", typeof(Vector3));

            obj = rectTransform;

            return obj;
        }
    }
}