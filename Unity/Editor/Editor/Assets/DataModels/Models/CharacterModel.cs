using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel: CharacterInterface {
    public int CHARACTER_ID { get; }

    public CharacterType characterType { get; set; }
    public string name { get; set; }
    public GreatHouseType house { get; set; }
    public int healthPoints { get; set; }
    public int damagePoints { get; set; }
    public int actionPoints { get; set; }
    public int movementPoints { get; set; }
    public int spice { get; set; }
    public bool wasLoud { get; set; }

    public CharacterModel(int CHARACTER_ID) {
        this.CHARACTER_ID = CHARACTER_ID;
    }

    public CharacterModel(int CHARACTER_ID, CharacterType characterType, string name, GreatHouseType house, int healthPoints, int damagePoints) {
        this.CHARACTER_ID = CHARACTER_ID;
        this.characterType = characterType;
        this.name = name;
        this.house = house;
        this.healthPoints = healthPoints;
        this.damagePoints = damagePoints;
    }
}
