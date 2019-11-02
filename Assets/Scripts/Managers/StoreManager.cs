using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace Assets.Scripts.Managers
{
    public class StoreManager : MonoBehaviour
    {
        public Text GiftsTotal;
        [SerializeField]
        public List<Skin> Skins;
        public GameObject SkinPanel;
        public GameObject MainPanel;

        private int _totalGifts;
        private Skin equippedSkin;
        private int _currentSkin = 0;

        void Awake()
        {
            _totalGifts = PlayerPrefs.GetInt("totalgifts", 0);
            var currentSkinName = PlayerPrefs.GetString("equipped_skin_name", "Boy");
            equippedSkin = Skins.Single(s => s.Name == currentSkinName);

            GiftsTotal.text = _totalGifts.ToString();

            DisplaySkin(Skins[_currentSkin]);
        }

        public void PreviousSkin()
        {
            _currentSkin = _currentSkin - 1 < 0 ? Skins.Count - 1 : _currentSkin - 1;
            DisplaySkin(Skins[_currentSkin]);
        }

        public void NextSkin()
        {
            _currentSkin = (_currentSkin + 1) % Skins.Count;
            DisplaySkin(Skins[_currentSkin]);
        }

        private void DisplaySkin(Skin skin)
        {
            SkinPanel.transform.Find("SkinPreview").GetComponent<Image>().sprite = skin.Sprite;
            SkinPanel.transform.Find("Panel").Find("SkinNameAndPrice").GetComponent<Text>().text = $"{skin.Name}: {skin.Price}";
            var actionButtonParent = MainPanel.transform.Find("ActionButton");
            var actionButton = actionButtonParent.GetComponent<Button>();
            var actionButtonText = actionButton.GetComponentInChildren<Text>();

            actionButton.interactable = true;
            var buttonClickedEvent = new Button.ButtonClickedEvent();
            
            if (skin.Name == equippedSkin.Name)
            {
                actionButtonText.text = "Equipped!";
                buttonClickedEvent.RemoveAllListeners();
            }
            else if (skin.Price > 0 && !IsSkinUnlocked(skin.Name))
            {
                actionButtonText.fontStyle = FontStyles.Normal;
                if (skin.Price > _totalGifts)
                {
                    actionButton.interactable = false;
                    buttonClickedEvent.RemoveAllListeners();
                }
                else
                {
                    buttonClickedEvent.AddListener(() => UnlockSkin(skin));
                }

                actionButtonText.text = "Unlock";
            }
            else
            {
                actionButtonText.fontStyle = FontStyles.Normal;
                actionButtonText.text = "Equip";
                buttonClickedEvent.AddListener(() => EquipSkin(skin));
            }

            actionButton.onClick = buttonClickedEvent;
        }

        private void UnlockSkin(Skin skin)
        {
            _totalGifts -= skin.Price;
            PlayerPrefs.SetInt("totalgifts", _totalGifts);
            PlayerPrefs.SetInt($"{skin.Name.ToLower()}_skin_unlocked", 1);
            GiftsTotal.text = _totalGifts.ToString();
            DisplaySkin(skin);
        }

        private void EquipSkin(Skin skin)
        {
            equippedSkin = skin;
            PlayerPrefs.SetString("equipped_skin_name", skin.Name);
            DisplaySkin(skin);
        }

        private bool IsSkinUnlocked(string skinName)
        {
            return PlayerPrefs.GetInt($"{skinName.ToLower()}_skin_unlocked", 0) == 1;
        }
    }
}
