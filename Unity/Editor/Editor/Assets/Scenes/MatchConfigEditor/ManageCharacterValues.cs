using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/**
 * Component for managing all Values of the Character system. Here all Values can be read, set and 
 * the slider range altered. No need to go down the hierarchy to the lower components.
 * 
 * Main Job is to scale the sliders to a common range; Also start of the Character initialization chain
 */
public class ManageCharacterValues : MonoBehaviour
{
    public readonly int MaxInventorySize = 10;
    public readonly int MaxAP = 10;
    public readonly int MaxMP = 10;
    public readonly int MaxHealth = 100;

    private int _currentMaxHealth;
    private int _currentMaxMP;
    private int _currentMaxAP;
    private int _currentMaxInv;

    public CharacterValues[] Init() {
        // initialize common maximum values
        CharacterValues[] ret = GetComponent<PopulateCharacters>().Init();
        UpdateMaxima(MaxHealth, MaxInventorySize, MaxAP, MaxMP);

        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        // check if there is a new max value, if so update reference
        int newHPMax = 0;
        bool updateMaxima = false;
        foreach (Transform t in transform) {
            newHPMax = Math.Max(newHPMax, t.GetComponent<CharacterValues>().Health);
            newHPMax = Math.Max(newHPMax, t.GetComponent<CharacterValues>().Damage);
            newHPMax = Math.Max(newHPMax, t.GetComponent<CharacterValues>().Healing);
        }
        newHPMax = Math.Max(newHPMax, MaxHealth); // maximum should never be less than in the configuration
        if (newHPMax != _currentMaxHealth) {
            updateMaxima = true;
        }
        int maxAPInput = GetCharacterValues().Select((CharacterValues values) => values.AP).Max();
        int maxMPInput = GetCharacterValues().Select((CharacterValues values) => values.AP).Max();
        int maxInvInput = GetCharacterValues().Select((CharacterValues values) => values.AP).Max();
        // norm the sliders on a maximum value. Should not be less than a defined value
        maxAPInput = Math.Max(maxAPInput, MaxAP);
        maxMPInput = Math.Max(maxMPInput, MaxMP);
        maxInvInput = Math.Max(maxInvInput, MaxInventorySize);

        if (maxAPInput != _currentMaxAP || maxMPInput != _currentMaxMP || maxInvInput != _currentMaxInv) {
            updateMaxima = true;
        }
        if (updateMaxima) UpdateMaxima(newHPMax, maxInvInput, maxAPInput, maxMPInput);
        
    }

    private void UpdateMaxima(int maxHelath, int maxInv, int maxAP, int maxMP) {
        _currentMaxHealth = maxHelath;
        _currentMaxAP = maxAP;
        _currentMaxMP = maxMP;
        _currentMaxInv = maxInv;
        foreach (Transform t in transform) {
            CharacterValues values = t.GetComponent<CharacterValues>();
            values.MaxAP = _currentMaxAP;
            values.MaxMP = _currentMaxMP;
            values.MaxInvSize = _currentMaxInv;
            values.MaxHealth = _currentMaxHealth;
            values.MaxDamage = _currentMaxHealth;
            values.MaxHealing = _currentMaxHealth;
        }
    }

    IEnumerable<CharacterValues> GetCharacterValues() {
        List<CharacterValues> values = new List<CharacterValues>();
        foreach (Transform t in transform) values.Add(t.GetComponent<CharacterValues>());
        return values;
    }
}
