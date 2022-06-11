using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterValuesModel
{
    public int healthPoints { get; }
    public int healingHP { get; }
    public int movementPoints { get; }
    public int actionPoints { get; }
    public int attackDamage { get; }
    public int inventorySize { get; }

    public CharacterValuesModel(int healthPoints, int healingHP, int movementPoints, int actionPoints, int attackDamage, int inventorySize) {
        this.healthPoints = healthPoints;
        this.healingHP = healingHP;
        this.movementPoints = movementPoints;
        this.actionPoints = actionPoints;
        this.attackDamage = attackDamage;
        this.inventorySize = inventorySize;
    }
}