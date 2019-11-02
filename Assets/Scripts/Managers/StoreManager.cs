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
        private int _currentSkin = 1;

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
            var actionButtonText = MainPanel.transform.Find("ActionButton").GetComponentInChildren<Text>();
            if (skin.Name == equippedSkin.Name)
            {
                actionButtonText.text = "Equipped!";
            }
            else if (skin.Price > 0 && !IsSkinUnlocked(skin.Name))
            {
                actionButtonText.fontStyle = FontStyles.Normal;
                actionButtonText.text = "Unlock";
            }
            else
            {
                actionButtonText.fontStyle = FontStyles.Normal;
                actionButtonText.text = "Equip";
            }
        }

        private bool IsSkinUnlocked(string skinName)
        {
            return PlayerPrefs.GetInt($"{skinName.ToLower()}_skin_unlocked", 0) == 1;
        }
    }
}
