using Assets.Scripts.TranslatorSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InventorySystem
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInventoryUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _info;

        [SerializeField] private Button _switchMode;

        [SerializeField] private float _basicFontSize = 75;
        [SerializeField] private float _detailFontSize = 50;

        [Space(25)]
        [SerializeField] private Color _greenWhite;
        [SerializeField] private Color _greenBlack;
        [SerializeField] private Color _gray;
        [SerializeField] private Color _red;
        [SerializeField] private Color _yellow;

        private PlayerInventory _inventory;

        private Translator _translator;

        private bool _detail;

        private void Start()
        {
            _translator = FindObjectOfType<Translator>();

            _inventory = GetComponent<PlayerInventory>();

            _switchMode.onClick.AddListener(() => {
                _detail = !_detail;
                _switchMode.GetComponentInChildren<TMP_Text>().text = _detail ?
                _translator.Translate("ShowBasic") : _translator.Translate("ShowAll");
                UpdateUI();
                });

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_detail)
            {
                _info.text =
                $"<color=#{_greenWhite.ToHexString()}>{_translator.Translate("Villagers")}: {_inventory.Villagers}\n</color>" +
                $"<color=#{_yellow.ToHexString()}>{_translator.Translate("Unemployed")}: {_inventory.Unemployed}</color>\n" +
                $"<color=#{_greenBlack.ToHexString()}>{_translator.Translate("Builders")}: {_inventory.Builder}</color>\n" +
                $"<color=#{_greenBlack.ToHexString()}>{_translator.Translate("Loggers")}: {_inventory.Logger}</color>\n" +
                $"<color=#{_greenBlack.ToHexString()}>{_translator.Translate("Mason")}: {_inventory.Mason}</color>\n" +
                $"<color=#FFFFFF>{_inventory.Resources}</color>";
            }
            else
            {
                _info.text =
                $"{_translator.Translate("Villagers")}: {_inventory.Villagers}\n" +
                $"{_translator.Translate("Unemployed")}: {_inventory.Unemployed}\n" +
                $"{_inventory.Resources}";
            }

            _info.fontSize = _detail? _detailFontSize : _basicFontSize;
        }
    }
}