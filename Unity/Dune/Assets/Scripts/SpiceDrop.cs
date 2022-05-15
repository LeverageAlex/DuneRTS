using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


/**
 * Primarly used for triggering Spice-Particle Effect on SpiceDrop
 * 
 */
public class SpiceDrop : MonoBehaviour
{

    public Animator animator;
    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        AudioController.instance.Play("SpiceSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        //Check if animation is still running
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpiceBlow"))
        {
            // Avoid any reload.
        }
        else 
        {
            //Spawns landing effect and removes this script from GameObject instance
            GameObject p = (GameObject) Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(p, 3);
            Destroy(this);
        }
    }
}
