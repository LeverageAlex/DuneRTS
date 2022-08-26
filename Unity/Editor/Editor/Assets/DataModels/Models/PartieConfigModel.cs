using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartieConfigModel
{
    private CharacterValuesModel Noble;
    private CharacterValuesModel Mentat;
    private CharacterValuesModel BeneGeserit;
    private CharacterValuesModel Fighter;

    //public int numberOfRounds { get; }
    public int numberOfRounds;
    public int actionTimeUserClient { get; }
    public int actionTimeAiClient { get; }
    public float highGroundBonusRatio { get; }
    public float lowGroundMalusRatio { get; }
    public float kanlySuccessProbability { get; }
    public int spiceMinimum { get; }
    public string cellularAutomate { get; }
    public int sandWormSpeed { get; }
    public int sandWormSpawnDistance { get; }
    public float cloneProbability { get; }

    public int minPauseTime { get; }
    public int maxStrikes { get; }
    public float crashProbability { get; }


    public PartieConfigModel(CharacterValuesModel Noble, CharacterValuesModel Mentat, 
        CharacterValuesModel BeneGeserit, CharacterValuesModel Fighter,
        int numberOfRounds, int actionTimeUserClient, int actionTimeAiClient, float highGroundBonusRatio, float lowGroundMalusRatio,
        float kanlySuccessProbability, int spiceMinimum, string cellularAutomate, int sandWormSpeed, 
        int sandWormSpawnDistance, float cloneProbability, int minPauseTime, int maxStrikes, float crashProbability) {
        
        this.Noble = Noble;
        this.Mentat = Mentat;
        this.BeneGeserit = BeneGeserit;
        this.Fighter = Fighter;
        this.numberOfRounds = numberOfRounds;
        this.actionTimeUserClient = actionTimeUserClient;
        this.actionTimeAiClient = actionTimeAiClient;
        this.highGroundBonusRatio = highGroundBonusRatio;
        this.lowGroundMalusRatio = lowGroundMalusRatio;
        this.kanlySuccessProbability = kanlySuccessProbability;
        this.spiceMinimum = spiceMinimum;
        this.cellularAutomate = cellularAutomate;
        this.sandWormSpeed = sandWormSpeed;
        this.sandWormSpawnDistance = sandWormSpawnDistance;
        this.cloneProbability = cloneProbability;
        this.minPauseTime = minPauseTime;
        this.maxStrikes = maxStrikes;
        this.crashProbability = crashProbability;
    }

    public int getHealthPoints(CharacterType type) {
        return getModelOfType(type).healthPoints;
    }

    public int getHealingHP(CharacterType type) {
        return getModelOfType(type).healingHP;
    }

    public int getMovementPoints(CharacterType type) {
        return getModelOfType(type).movementPoints;
    }

    public int getActionPoint(CharacterType type) {
        return getModelOfType(type).actionPoints;
    }

    public int getAttackDamage(CharacterType type) {
        return getModelOfType(type).attackDamage;
    }

    public int getInventorySize(CharacterType type) {
        return getModelOfType(type).inventorySize;
    }

    private CharacterValuesModel getModelOfType(CharacterType type) {
        switch (type){
            case CharacterType.NOBLE:
                return Noble;
            case CharacterType.MENTAT:
                return Mentat;
            case CharacterType.BENE_GESSERIT:
                return BeneGeserit;
            case CharacterType.FIGHTER:
                return Fighter;
        }

        return null;    // Should never happen
    }

    public static PartieConfigModel GetDefaultPartieConfigModel() {
        return new PartieConfigModel(
            Noble: new CharacterValuesModel(100, 10, 5, 5, 5, 5),
            Mentat: new CharacterValuesModel(100, 10, 5, 5, 5, 5),
            BeneGeserit: new CharacterValuesModel(100, 10, 5, 5, 5, 5),
            Fighter: new CharacterValuesModel(100, 10, 5, 5, 5, 5),
            numberOfRounds: 3,
            actionTimeUserClient: 5000,
            actionTimeAiClient: 1000,
            highGroundBonusRatio: 0.5f,
            lowGroundMalusRatio: 0.5f,
            kanlySuccessProbability: 0.5f,
            spiceMinimum: 10,
            cellularAutomate: "3B5S",
            sandWormSpeed: 4,
            sandWormSpawnDistance: 3,
            cloneProbability: 0.2f,
            minPauseTime: 0,
            maxStrikes: 10,
            crashProbability: 0.25f
            ) ;
    }
}
