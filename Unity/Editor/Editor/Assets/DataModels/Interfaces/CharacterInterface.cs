using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterInterface {

    int CHARACTER_ID { get; }
    CharacterType characterType { get; set; }
    string name { get; set; }
    GreatHouseType house { get; set; }
    int healthPoints { get; set; }
    int damagePoints { get; set; }
    int actionPoints { get; set; }
    int movementPoints { get; set; }
    int spice { get; set; }
    bool wasLoud { get; set; }

}
