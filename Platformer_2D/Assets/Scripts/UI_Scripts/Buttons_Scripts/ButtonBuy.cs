using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuy : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private PlayerMover _player;
    [SerializeField] private Text _text;

    private int _price;

    private const string HEAL_POTION = "ButtonHealPotion";
    private const string MANA_POTION = "ButtonManaPotion";
    private const string INCREASE_HIT_POTION = "ButtonMaxHitPotion";
    private const string INCREASE_MANA_POTION = "ButtonSkillPotion";
    private const string POISON = "ButtonPoison";

    private void Awake()
    {
        _price = System.Convert.ToInt32(_text.text);
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        if (_player.CoinsAmount >= _price)
        {
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
        }
    }

    private void HealPotion()
    {
        _player.CurrentHitPoints = _player.MaxHitPoints;
    }

    private void ManaPotion()
    {
        _player.CurrentManaPoints = _player.MaxManaPoints;
    }

    private void IncreaseHitPotion()
    {
        _player.ChangeSliderValue("Slider_HitPoints", 25);
    }

    private void IncreaseManaPotion()
    {
        _player.ChangeSliderValue("Slider_ManaPoints", 25);
    }

    private void Poison()
    {
        _player.AttackDamage += 25;
    }
}