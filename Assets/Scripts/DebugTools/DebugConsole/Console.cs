using Assets.Scripts.FireSystems;
using Assets.Scripts.WeatherSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.DebugTools
{
    public class Console : MonoBehaviour
    {
        private GUIStyle _logStyle;

        private List<string> _commandsOutput = new List<string>();

        private DisplayType _displayType = DisplayType.None;

        private string _consoleInput;
        private bool _isShowed;

        private void Awake()
        {
            InitializeCommands();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _isShowed = !_isShowed;
            }
        }

        private void OnGUI()
        {
            if (!_isShowed)
            {
                return;
            }

            if(_logStyle == null)
            {
                _logStyle = new GUIStyle(GUI.skin.label);
                _logStyle.fontSize = 16;
            }

            DrawConsole(out var newInput, out var y);

            UpdateDisplayType(newInput, y);

            _consoleInput = newInput;

            Event currentEvent = Event.current;
            
            if (currentEvent.isKey)
            {
                switch (currentEvent.keyCode)
                {
                    case KeyCode.Tab:
                        _displayType = DisplayType.Autocomplete;
                        break;

                    case KeyCode.Escape:
                        _isShowed = false;
                        break;

                    case KeyCode.Return:
                        Return();
                        break;
                }
            }
        }

        private void InitializeCommands()
        {
            new DebugCommand("help", "Lists all available debug commands.", "help", () =>
            {
                _displayType = DisplayType.Help;
            });

            new DebugCommand("clean", "Cleans console output", "clean", () =>
            {
                _commandsOutput.Clear();
            });

            new DebugCommand("set_random_fire", "Set a random tree on fire.", "set_random_fire", () =>
            {
                var tree = FindObjectOfType<FireComponent>();
                tree.IsFire = true;

                _commandsOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] Random fire started on {tree.name}.");

                _displayType = DisplayType.Output;
            });

            new DebugCommand<int>("weather", "Sets weather to specific ID", "weather <id>", (x) =>
            {
                WeatherSystem.WeatherSystem weatherSystem = FindFirstObjectByType<WeatherSystem.WeatherSystem>();
                weatherSystem.SetWeather(x);

                _commandsOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] Weather was selected to {weatherSystem.Current.Name}.");

                _displayType = DisplayType.Output;
            });

            new DebugCommand<int>("set_time", "Sets time", "set_time <hour>", (time) =>
            {
                ChangerDayAndNight dayAndNight = FindObjectOfType<ChangerDayAndNight>();

                dayAndNight.SetTime(time);

                _commandsOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] Time set to {time} hour.");

                _displayType = DisplayType.Output;
            });

            new DebugCommand<int>("set_time_speed", "Sets time speed", "set_speed <seconds>", (x) =>
            {
                ChangerDayAndNight dayAndNight = FindObjectOfType<ChangerDayAndNight>();
               
                _commandsOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] Time speed was set from {dayAndNight.TimeSpeed} to {x}.");

                dayAndNight.SetTimeSpeed(x);

                _displayType = DisplayType.Output;
            });

            new DebugCommand<int>("get_all_resources", "Get receive all resources in the specified quantity in a random storages", "get_all_resources <count>", (x) =>
            {
                var resources = new Resources(x, x, x);
                var storages = World.Storages.FindAll(x => x.Resources.TotalCount() < x.MaxResources);

                foreach ( var storage in storages)
                {
                    var resourcesTransfer = resources.GetClampedResources(resources, storage.MaxResources - storage.Resources.TotalCount());

                    resources -= resourcesTransfer;
                    storage.TransferResources(resourcesTransfer);
                }

                _commandsOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] Resources increased " +
                    $"(added: {new Resources(x, x, x).TotalCount() - resources.TotalCount()}).");

                _displayType = DisplayType.Output;
            });
        }

        private void DrawConsole(out string input, out float y)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            y = 32;
            input = GUI.TextField(new Rect(0, 0, Screen.width, y), _consoleInput);

            GUI.Box(new Rect(0, y, Screen.width, Screen.height - y), "");
        }

        private void UpdateDisplayType(string newInput, float y)
        {
            switch (_displayType)
            {
                case DisplayType.Help:
                    ShowHelp(y);
                    break;
                case DisplayType.Autocomplete:
                    ShowAutocomplete(y, newInput);
                    break;
                case DisplayType.Output:
                    ShowOutput(y);
                    break;

                case DisplayType.None:

                    break;
            }
        }

        private void ShowHelp(float y)
        {
            foreach (var command in BaseCommand.DebugCommands.Values)
            {
                GUI.Label(
                    new Rect(2, y, Screen.width, 26),
                    $"{command.Format} - {command.Description}",
                    _logStyle
                );

                y += 22;
            }
        }

        private void ShowAutocomplete(float y, string newInput)
        {
            IEnumerable<string> autocompleteCommands =
                BaseCommand.DebugCommands.Keys.Where(k => k.StartsWith(newInput.ToLower()));

            foreach (string k in autocompleteCommands)
            {
                BaseCommand command = BaseCommand.DebugCommands[k];
                
                GUI.Label(
                    new Rect(2, y, Screen.width, 26),
                    $"{command.Format} - {command.Description}",
                    _logStyle
                );

                y += 22;
            }
        }

        private void ShowOutput(float y)
        {
            foreach (string line in _commandsOutput)
            {
                GUI.Label(new Rect(2, y, Screen.width, 26), line, _logStyle);
                y += 22;
            }
        }

        private void Return()
        {
            HandleConsoleInput();
            _consoleInput = "";
        }

        private void HandleConsoleInput()
        {
            //Короче проверяем ввод, параметры если есть передаем через пробел
            string[] inputParts = _consoleInput.Split(' ');
            string mainKeyword = inputParts[0];
            
            BaseCommand currentCommand;

            if (BaseCommand.DebugCommands.TryGetValue(mainKeyword.ToLower(), out currentCommand))
            {
                if (currentCommand is DebugCommand nonParameters)
                {
                    nonParameters.Invoke();
                }
                else
                {
                    if (inputParts.Length < 2)
                    {
                        Debug.LogError("Missing parameter!");
                        return;
                    }

                    //Проверяем какой тип аргумента у экшена
                    if (currentCommand is DebugCommand<string> stringCommand)
                    {
                        stringCommand.Invoke(inputParts[1]);
                    }
                    else if (currentCommand is DebugCommand<int> intCommand)
                    {
                        int i;

                        if (int.TryParse(inputParts[1], out i))
                        {
                            intCommand.Invoke(i);
                        }
                        else
                        {
                            Debug.LogError($"'{currentCommand.Id}' requires an int parameter!");
                            return;
                        }
                    }
                    else if (currentCommand is DebugCommand<float> floatCommand)
                    {
                        float f;

                        if (float.TryParse(inputParts[1], out f))
                        {
                            floatCommand.Invoke(f);
                        }
                        else
                        {
                            Debug.LogError($"'{currentCommand.Id}' requires a float parameter!");
                            return;
                        }
                    }
                    else if (currentCommand is DebugCommand<string, int> stringIntCommand)
                    {
                        int i;

                        if (int.TryParse(inputParts[2], out i))
                        {
                            stringIntCommand.Invoke(inputParts[1], i);
                        }
                        else
                        {
                            Debug.LogError($"'{currentCommand.Id}' requires a string and an int parameter!");
                            return;
                        }
                    }
                }
            }
        }
    }
}