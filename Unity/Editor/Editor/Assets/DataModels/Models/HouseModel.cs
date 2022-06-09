using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HouseModel
{
    public GreatHouseType HouseType;
    public int FamilyAtomics;

    private List<CharacterModel> characters;

    private HouseModel() { }

    public CharacterModel GetCharacterModel(CharacterType type) {
        return characters.Where((CharacterModel model) => {return model.characterType == type; }).First();
    }
}
