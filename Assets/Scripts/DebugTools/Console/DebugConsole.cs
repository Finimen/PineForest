using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.WeatherSystem;

namespace Assets.Scripts.DebugTools
{
    //I think it need some logger to the file. JSON, TXT?

    public class DebugConsole : MonoBehaviour
    {
        enum DisplayType
        {
            None,
            Help,
            Autocomplete,
            Output
        }

        private static GUIStyle _logStyle;

        private bool _showConsole = false;
        private string _consoleInput;

        private DisplayType _displayType;

        private List<string> _commandOutput;

        private void Awake()
        {
            new DebugCommand("help", "Lists all available debug commands.", "help", () =>
            {
                _displayType = DisplayType.Help;
            });

            new DebugCommand("clean", "Cleans console output", "clean", () =>
            {
                if (_commandOutput == null)
                    _commandOutput = new List<string>();

                _commandOutput.Clear();
            });


            new DebugCommand("storages_full", "Fill all storages by max amount of resources", "storages_full", () =>
            {
                foreach (var storage in World.Storages)
                {
                    storage.FillFull();
                }

                if (_commandOutput == null)
                    _commandOutput = new List<string>();

                _commandOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] All storages was filled with resources.");

                _displayType = DisplayType.Output;
            });

            new DebugCommand("storages_clean", "Clean resources from storages", "storages_clean", () =>
            {
                foreach (var storage in World.Storages)
                {
                    storage.Clean();
                }

                if (_commandOutput == null)
                    _commandOutput = new List<string>();

                _commandOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] All resources was removed from storages.");

                _displayType = DisplayType.Output;
            });

            new DebugCommand<int>("weather", "Sets weather to specific ID", "weather <id>", (x) =>
            {
                WeatherSystem.WeatherSystem weatherSystem = FindFirstObjectByType<WeatherSystem.WeatherSystem>();
                weatherSystem.SetWeather(x);

                if (_commandOutput == null)
                    _commandOutput = new List<string>();

                _commandOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] Weather was selected to {weatherSystem.GetCurrentWeather().Name}.");

                _displayType = DisplayType.Output;
            });

            new DebugCommand<int>("set_time", "Sets time", "set_time <hour>", (x) =>
            {
                ChangerDayAndNight dayAndNight = FindFirstObjectByType<ChangerDayAndNight>();
                dayAndNight.SetTime(x);
                if (_commandOutput == null)
                    _commandOutput = new List<string>();

                _commandOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] Time set to {x} hour.");

                _displayType = DisplayType.Output;
            });

            new DebugCommand<int>("set_duration", "Sets duration of the day", "set_duration <seconds>", (x) =>
            {
                ChangerDayAndNight dayAndNight = FindFirstObjectByType<ChangerDayAndNight>();
                if (_commandOutput == null)
                    _commandOutput = new List<string>();

                _commandOutput.Add($"[{System.DateTime.Now.ToString("hh:mm:ss tt")}] Day duration was set from {dayAndNight.DayDuration} to {x} seconds.");

                dayAndNight.SetDayDuration(x);

                _displayType = DisplayType.Output;
            });


            _displayType = DisplayType.None;
        }


        private void _OnShowDebugConsole()
        {
            _showConsole = true;
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.BackQuote) && !_showConsole)
            {
                _OnShowDebugConsole();
            }
        }
        private void OnGUI()
        {
            if (_logStyle == null)
            {
                _logStyle = new GUIStyle(GUI.skin.label);
                _logStyle.fontSize = 12;
            }

            if (_showConsole)
            {
                // add fake boxes in the background to increase the opacity
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

                // show main input field
                string newInput = GUI.TextField(new Rect(0, 0, Screen.width, 24), _consoleInput);

                // show log area
                float y = 24;
                GUI.Box(new Rect(0, y, Screen.width, Screen.height - 24), "");

                if (_displayType == DisplayType.Help)
                    _ShowHelp(y);
                else if (_displayType == DisplayType.Autocomplete)
                    _ShowAutocomplete(y, newInput);
                else if (_displayType == DisplayType.Output)
                    _ShowOutput(y);

                // reset display state to "none" if input changes
                if (_displayType != DisplayType.None && _consoleInput.Length != newInput.Length)
                    _displayType = DisplayType.None;

                if(_consoleInput.Length != newInput.Length && newInput.Length < 1)
                {
                   if(_commandOutput != null) _displayType = DisplayType.Output;
                }

                // update input variable
                _consoleInput = newInput;

                // check for special keys
                Event e = Event.current;
                if (e.isKey)
                {
                    if (e.keyCode == KeyCode.Tab)
                        _displayType = DisplayType.Autocomplete;
                    else if (e.keyCode == KeyCode.Return && _consoleInput.Length > 0)
                        _OnReturn();
                    else if (e.keyCode == KeyCode.Escape)
                    {
                        _showConsole = false;
                    }
                }
            }
        }

        private void _ShowHelp(float y)
        {
            foreach (DebugCommandBase command in DebugCommandBase.DebugCommands.Values)
            {
                GUI.Label(
                    new Rect(2, y, Screen.width, 20),
                    $"{command.Format} - {command.Description}",
                    _logStyle
                );
                y += 16;
            }
        }

        private void _ShowAutocomplete(float y, string newInput)
        {
            IEnumerable<string> autocompleteCommands =
                                DebugCommandBase.DebugCommands.Keys
                                .Where(k => k.StartsWith(newInput.ToLower()));
            foreach (string k in autocompleteCommands)
            {
                DebugCommandBase c = DebugCommandBase.DebugCommands[k];
                GUI.Label(
                    new Rect(2, y, Screen.width, 20),
                    $"{c.Format} - {c.Description}",
                    _logStyle
                );
                y += 16;
            }
        }

        private void _ShowOutput(float y)
        {
            foreach (string line in _commandOutput)
            {
                GUI.Label(new Rect(2, y, Screen.width, 20), line, _logStyle);
                y += 16;
            }
        }

        private void _OnReturn()
        {
            _HandleConsoleInput();
            _consoleInput = "";
        }

        private void _HandleConsoleInput()
        {
            // parse input
            string[] inputParts = _consoleInput.Split(' ');
            string mainKeyword = inputParts[0];
            // check against available commands
            DebugCommandBase command;
            if (DebugCommandBase.DebugCommands.TryGetValue(mainKeyword.ToLower(), out command))
            {
                // try to invoke command if it exists
                if (command is DebugCommand dc)
                    dc.Invoke();
                else
                {
                    if (inputParts.Length < 2)
                    {
                        Debug.LogError("Missing parameter!");
                        return;
                    }

                    if (command is DebugCommand<string> dcString)
                    {
                        dcString.Invoke(inputParts[1]);
                    }
                    else if (command is DebugCommand<int> dcInt)
                    {
                        int i;
                        if (int.TryParse(inputParts[1], out i))
                            dcInt.Invoke(i);
                        else
                        {
                            Debug.LogError($"'{command.Id}' requires an int parameter!");
                            return;
                        }
                    }
                    else if (command is DebugCommand<float> dcFloat)
                    {
                        float f;
                        if (float.TryParse(inputParts[1], out f))
                            dcFloat.Invoke(f);
                        else
                        {
                            Debug.LogError($"'{command.Id}' requires a float parameter!");
                            return;
                        }
                    }
                    else if (command is DebugCommand<string, int> dcStringInt)
                    {
                        int i;
                        if (int.TryParse(inputParts[2], out i))
                            dcStringInt.Invoke(inputParts[1], i);
                        else
                        {
                            Debug.LogError($"'{command.Id}' requires a string and an int parameter!");
                            return;
                        }
                    }
                }
            }
        }
    }
}