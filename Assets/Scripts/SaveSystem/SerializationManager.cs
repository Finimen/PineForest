using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.SaveSystem
{
    internal class SerializationManager
    {
        public static bool Save(string saveName, string folder, object saveData)
        {
            BinaryFormatter formatter = GetBinaryFormatter();

            UnityEngine.Debug.Log(Application.persistentDataPath + folder);

            if(!Directory.Exists(Application.persistentDataPath + folder))
            {
                Directory.CreateDirectory(Application.persistentDataPath + folder);
            }

            string path = Application.persistentDataPath + folder + saveName + ".save";

            FileStream fileStream = File.Create(path);

            formatter.Serialize(fileStream, saveData);

            fileStream.Close();

            return true;
        }

        public static object Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            BinaryFormatter formatter = GetBinaryFormatter();

            FileStream fileStream = File.Open(path, FileMode.Open);

            try
            {
                object save = formatter.Deserialize(fileStream);
                fileStream.Close();
                return save;
            }
            catch
            {
                UnityEngine.Debug.LogErrorFormat("Failed to load file at {0}", path);
                fileStream.Close();
                throw new FileNotFoundException();
            }
        }

        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            SurrogateSelector surrogateSelector = new SurrogateSelector();

            Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
            QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();
            ColorSerializationSurrogate colorSurrogate = new ColorSerializationSurrogate();
            TransformSerializationSurrogate transformSurrogate = new TransformSerializationSurrogate();
            RectTransformSerializationSurrogate rectTransformSurrogate = new RectTransformSerializationSurrogate();

            surrogateSelector.AddSurrogate(typeof (Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
            surrogateSelector.AddSurrogate(typeof (Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);
            surrogateSelector.AddSurrogate(typeof (Color), new StreamingContext(StreamingContextStates.All), colorSurrogate);
            surrogateSelector.AddSurrogate(typeof (RectTransform), new StreamingContext(StreamingContextStates.All), rectTransformSurrogate);
            //surrogateSelector.AddSurrogate(typeof (Transform), new StreamingContext(StreamingContextStates.All), transformSurrogate);

            formatter.SurrogateSelector = surrogateSelector;

            return formatter;
        }
    }
}