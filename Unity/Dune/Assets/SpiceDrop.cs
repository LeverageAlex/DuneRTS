using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SpiceDrop : MonoBehaviour
{

    public Animator animator;
    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpiceBlow"))
        {
            // Avoid any reload.
        }
        else 
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(this);
        }
    }
}
