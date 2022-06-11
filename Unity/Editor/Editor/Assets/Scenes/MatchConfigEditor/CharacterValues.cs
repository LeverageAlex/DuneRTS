using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This Component is a middleman between all Values of a Character and abstracts them from the 
 * subsequent input components. Interacting with the Max and Value properties changes the values in the UI
 */
public class CharacterValues : MonoBehaviour
{
    public int Health { set => _healthInput.Value = value; get => _healthInput.Value; }
    public int Damage { set => _damageInput.Value = value; get => _damageInput.Value; }
    public int Healing { set => _healingInput.Value = value; get => _healingInput.Value; }
    public int AP { set => _apInput.Value = value; get => _apInput.Value; }
    public int MP { set => _mpInput.Value = value; get => _mpInput.Value; }
    public int InvSize { set => _invInput.Value = value; get => _invInput.Value; }

    public int MaxDamage { set => _damageInput.ValueMax = value; get => _damageInput.ValueMax; }
    public int MaxHealth { set => _healthInput.ValueMax = value; get => _healthInput.ValueMax; }
    public int MaxHealing { set => _healingInput.ValueMax = value; get => _healingInput.ValueMax; }
    public int MaxAP { set => _apInput.ValueMax = value; get => _apInput.ValueMax; }
    public int MaxMP { set => _mpInput.ValueMax = value; get => _mpInput.ValueMax; }
    public int MaxInvSize { set => _invInput.ValueMax = value; get => _invInput.ValueMax; }

    private UpdateSlider _healthInput;
    private UpdateSlider _damageInput;
    private UpdateSlider _healingInput;
    private UpdateSlider _apInput;
    private UpdateSlider _mpInput;
    private UpdateSlider _invInput;


    public void Init() {
        transform.Find("Properties").GetComponent<PopulateSliders>().InitSliders();

        _healthInput = transform.Find("Properties").Find("Health").Find("Input").GetComponent<UpdateSlider>();
        _damageInput = transform.Find("Properties").Find("Damage").Find("Input").GetComponent<UpdateSlider>();
        _healingInput = transform.Find("Properties").Find("Healing").Find("Input").GetComponent<UpdateSlider>();
        _mpInput = transform.Find("Properties").Find("MovementPoints").Find("Input").GetComponent<UpdateSlider>();
        _apInput = transform.Find("Properties").Find("ActionPoints").Find("Input").GetComponent<UpdateSlider>();
        _invInput = transform.Find("Properties").Find("InventorySize").Find("Input").GetComponent<UpdateSlider>();
    }
}
