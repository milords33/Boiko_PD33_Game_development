using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonBuy : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private AudioSource _buySound;
    [SerializeField] private PlayerMover _player;
    [SerializeField] private TextMeshProUGUI _textPrice;

    private int _price;

    private const string HEAL_POTION = "ButtonHealPotion";
    private const string MANA_POTION = "ButtonManaPotion";
    private const string INCREASE_HIT_POTION = "ButtonMaxHitPotion";
    private const string INCREASE_MANA_POTION = "ButtonSkillPotion";
    private const string POISON = "ButtonPoison";

    private void Awake()
    {
        _price = System.Convert.ToInt32(_textPrice.text);
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        if (_player.CoinsAmount >= _price)
        {
            _buySound.Play();
            _player.CoinsAmount -= _price;

            if (_button.name == HEAL_POTION)
                HealPotion();
            else if (_button.name == MANA_POTION)
                ManaPotion();
            else if (_button.name == INCREASE_HIT_POTION)
                IncreaseHitPotion();
            else if (_button.name == INCREASE_MANA_POTION)
                IncreaseManaPotion();
            else if (_button.name == POISON)
                Poison();
            else
                throw new System.Exception("Invalid name button");

            _buttonImage.color = Color.black;
            //_button.colors = ColorBlock.defaultColorBlock;

            _button.enabled = false;
        }
    }

    private void HealPotion()
    {
        _player.CurrentHitPoints = _player.MaxHitPoints;
        _shop.DestroyProducts("HealPotion");
    }

    private void ManaPotion()
    {
        _player.CurrentManaPoints = _player.MaxManaPoints;
        _shop.DestroyProducts("ManaPotion");
    }

    private void IncreaseHitPotion()
    {
        _player.ChangeSliderValue("Slider_HitPoints", 25);
        _shop.DestroyProducts("IncreaseHitPotion");
    }

    private void IncreaseManaPotion()
    {
        _player.ChangeSliderValue("Slider_ManaPoints", 100);
        _shop.DestroyProducts("IncreaseManaPotion");
    }

    private void Poison()
    {
        _player.AttackDamage += 25;
        _shop.DestroyProducts("Poison");
    }
}