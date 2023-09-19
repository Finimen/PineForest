using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DebugTools
{
    public class BaseCommand : MonoBehaviour
    {
        public static Dictionary<string, BaseCommand> DebugCommands = new Dictionary<string, BaseCommand>();

        private string _id;
        private string _description;
        private string _format;

        public BaseCommand(string id, string description, string format)
        {
            _id = id;
            _description = description;
            _format = format;

            string mainKeyword = format.Split(' ')[0];

            DebugCommands[mainKeyword] = this;
        }

        public string Id => _id;
        public string Description => _description;
        public string Format => _format;
    }

    public class DebugCommand : BaseCommand
    {
        private Action _action;

        public DebugCommand(string id, string description, string format, Action action)
            : base(id, description, format)
        {
            _action = action;
        }

        public void Invoke()
        {
            _action.Invoke();
        }
    }

    public class DebugCommand<T> : BaseCommand
    {
        private Action<T> _action;

        public DebugCommand(string id, string description, string format, Action<T> action)
            : base(id, description, format)
        {
            _action = action;
        }

        public void Invoke(T value)
        {
            _action.Invoke(value);
        }
    }

    public class DebugCommand<T1, T2> : BaseCommand
    {
        private Action<T1, T2> _action;

        public DebugCommand(string id, string description, string format, Action<T1, T2> action)
            : base(id, description, format)
        {
            _action = action;
        }

        public void Invoke(T1 v1, T2 v2)
        {
            _action.Invoke(v1, v2);
        }
    }
}