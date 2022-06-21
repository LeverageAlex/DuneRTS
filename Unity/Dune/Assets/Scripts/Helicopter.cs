using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    public Animator crashAnimator;
    public ParticleSystem crashParticle;
    public GameObject helicopterModel;

    private Vector3 target;

    private float walkSpeed = 3f;
    private int rotationOffset = 0;
    private Character CharacterToTransport;

    private helicopterState currentState;
    private enum helicopterState { moveToStorm, animatingInStorm, moveToTarget}





    public void InitHelicopter(Character charToTransport,Vector3 target, bool crash)
    {
        this.target = target;
        CharacterToTransport = charToTransport;
        charToTransport.gameObject.SetActive(false);
        CharacterTurnHandler.instance.HideSelectedArrow(true);

        MapManager.instance.placeObjectOnNode(CharacterToTransport.gameObject, (int)Mathf.Round(target.x), (int)Mathf.Round(target.z));
        MapManager.instance.RemoveObjectOnNode(CharacterToTransport.X, CharacterToTransport.Z);

        CharacterToTransport.ReplaceCharacterOnPosition((int)Mathf.Round(target.x), (int)Mathf.Round(target.z));
        AudioController.instance.Play("HelicopterFly");

        crashAnimator.enabled = false;
        helicopterModel.active = true;
        crashParticle.Pause();

        if (crash)
        {
            currentState = helicopterState.moveToStorm;
        }
        else
        {
            currentState = helicopterState.moveToTarget;
        }
    }

    // Start is called before the first frame update
    private void Update()
    {
        if (currentState == helicopterState.moveToStorm)
        {
            //Vector3 stormCoords = MapManager.instance.GetStormEyePosition();
            if(!MoveToPoint(MapManager.instance.GetStormEyePosition()))
            {
                //Entered StormEye
                currentState = helicopterState.animatingInStorm;
                StartCoroutine(heliStormAnimation());
            }
        }
        else if(currentState == helicopterState.moveToTarget)
        {
            if(!MoveToPoint(target))
            {
                //Entered target
                CharacterToTransport.gameObject.SetActive(true);

                CharacterTurnHandler.instance.HideSelectedArrow(false);
                CharacterTurnHandler.instance.updateSelectionArrow();
                AudioController.instance.StopPlaying("HelicopterFly");

                DespwanHelicopter();
            }
        }
    }


    IEnumerator heliStormAnimation()
    {
        //Start animation here
        //yield return new WaitForSeconds(2);
        currentState = helicopterState.moveToTarget;

        crashAnimator.enabled = true;
        crashAnimator.Play("helicopterCrash");

        crashParticle.Play();

        yield return null;
    }

    private void DespwanHelicopter()
    {
        helicopterModel.active = false;
        crashParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Destroy(gameObject, crashParticle.startLifetime);
    }


    /*
 * Will be called every frame to move towards points in walkpath
 * @return: whether movement is finished or needs to be recalled again
 */
    public bool MoveToPoint(Vector3 pointToGo)
    {
        Vector3 dir = pointToGo - transform.position;
        //Rotate Object towards movement direction
        transform.rotation = Quaternion.LookRotation(dir);
        transform.Rotate(Vector3.right, rotationOffset);

        transform.Translate(dir.normalized * walkSpeed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, pointToGo) <= 0.06f)
        {
            //NodeManager.instance.placeObjectOnNode(gameObject, (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));

            //NodeManager.instance.RemoveObjectOnNode(X, Z);

            //E. g. go To next Point
            return false;
        }
        return true;
    }
}
