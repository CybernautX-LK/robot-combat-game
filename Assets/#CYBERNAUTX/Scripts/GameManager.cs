using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [BoxGroup("Settings")]
    [SerializeField]
    [Range(1, 5)]
    private int roundsToWin = 3;

    [BoxGroup("Settings")]
    [SerializeField]
    [Range(3, 5)]
    private int countdownTimer = 5;

    [BoxGroup("Settings")]
    [SerializeField]
    [Range(30, 300)]
    private int roundDuration = 120;

    [BoxGroup("Settings")]
    [SerializeField]
    private List<Weapon> availableWeapons = new List<Weapon>();

    [BoxGroup("Settings")]
    [SerializeField]
    private Weapon[] playerWeapons;

    [BoxGroup("Settings")]
    [SerializeField]
    private Weapon[] opponentWeapons;

    [BoxGroup("References")]
    [SerializeField]
    private List<ItemSelection> playerWeaponSelections = new List<ItemSelection>();

    [BoxGroup("References")]
    [SerializeField]
    private List<ItemSelection> opponentWeaponSelections = new List<ItemSelection>();

    [BoxGroup("References")]
    [SerializeField]
    private ItemSelection roundsToWinSelection;

    private void Awake()
    {
        playerWeapons = new Weapon[playerWeaponSelections.Count];
        opponentWeapons = new Weapon[opponentWeaponSelections.Count];
    }

    private void OnEnable()
    {
        if (roundsToWinSelection != null)
            roundsToWinSelection.OnCurrentItemChangedEvent += OnRoundsToWinChanged;

        foreach (ItemSelection itemSelection in playerWeaponSelections)
        {
            itemSelection.OnCurrentItemChangedEvent += OnPlayerWeaponsChanged;
            //Debug.Log("Player Subscribed");
        }

        foreach (ItemSelection itemSelection in opponentWeaponSelections)
        {
            itemSelection.OnCurrentItemChangedEvent += OnOpponentWeaponsChanged;
            //Debug.Log("Opponent Subscribed");
        }
    }

    private void OnDisable()
    {
        if (roundsToWinSelection != null)
            roundsToWinSelection.OnCurrentItemChangedEvent -= OnRoundsToWinChanged;

        foreach (ItemSelection itemSelection in playerWeaponSelections)
        {
            itemSelection.OnCurrentItemChangedEvent -= OnPlayerWeaponsChanged;
            //Debug.Log("Player Unsubscribed");
        }

        foreach (ItemSelection itemSelection in opponentWeaponSelections)
        {
            itemSelection.OnCurrentItemChangedEvent -= OnOpponentWeaponsChanged;
            //Debug.Log("Opponent Unsubscribed");
        }
    }

    [Button]
    public void StartGame()
    {
        if (!SettingsValid()) return;
    }

    private bool SettingsValid()
    {
        bool playerUsesDifferentWeapons = playerWeapons.Distinct().Count() == playerWeapons.Count();
        bool opponentUsesDifferentWeapons = opponentWeapons.Distinct().Count() == opponentWeapons.Count();
        return playerUsesDifferentWeapons && opponentUsesDifferentWeapons;
    }

    private void OnPlayerWeaponsChanged(ItemSelection itemSelection)
    {
        //Debug.Log("Player Weapons Changed");

        Weapon weapon = GetWeaponByName(itemSelection.currentItem.name);

        if (weapon == null) return;

        int index = playerWeaponSelections.IndexOf(itemSelection);

        playerWeapons[index] = weapon;

        
    }

    private void OnOpponentWeaponsChanged(ItemSelection itemSelection)
    {
        //Debug.Log("Opponents Weapons Changed");

        Weapon weapon = GetWeaponByName(itemSelection.currentItem.name);

        if (weapon == null) return;

        int index = opponentWeaponSelections.IndexOf(itemSelection);

        opponentWeapons[index] = weapon;
    }

    private void OnRoundsToWinChanged(ItemSelection itemSelection)
    {
        int amountOfRounds = int.Parse(itemSelection.currentItem.name);
        roundsToWin = amountOfRounds;
    }



    private Weapon GetWeaponByName(string name)
    {
        Weapon weapon = availableWeapons.FirstOrDefault((x) => x.type.ToString() == name.Replace(" ", ""));

        if (weapon == null)
            return null;

        return weapon;
    }
}
