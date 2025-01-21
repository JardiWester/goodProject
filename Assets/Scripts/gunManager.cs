using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class gunManager : hapticsPlayerParent
{
    pointToLineHaptics hapticsScript;
    GunShoot shootScript;
    bool hoverHaptics;
    bool activated = false;

    public NearFarInteractor rHand;
    public NearFarInteractor lHand;
    [SerializeField] protected controllerHandedness handedness;

    [SerializeField] float min = 10;
    [SerializeField] float div = 10;



    public void activate()
    {
        activated = true;
        stopHover();

        bool rController = false;
        if (rHand.interactablesSelected.Count > 0)
        {
            //Debug.Log(rHand.interactablesSelected[0]);
            foreach (IXRSelectInteractable interactor in rHand.interactablesSelected)
            {
                if (interactor.transform.CompareTag("gun"))
                {
                    rController = true;
                }
            }
        }
       
        bool lController = false;
        if (lHand.interactablesSelected.Count > 0)
        {
            foreach (IXRSelectInteractable interactor in lHand.interactablesSelected)
            {
                if (interactor.transform.CompareTag("gun"))
                {
                    lController = true;
                }
            }
        }
        

        

        if (lController && rController)
        {
            hapticsScript.handedness = controllerHandedness.both;
            shootScript.handedness = controllerHandedness.both;
        }
        else if (lController)
        {
            hapticsScript.handedness = controllerHandedness.left;
            shootScript.handedness = controllerHandedness.left;
        }
        else if (rController)
        {
            hapticsScript.handedness = controllerHandedness.right;
            shootScript.handedness = controllerHandedness.right;
        }
        else
        {
            hapticsScript.handedness = controllerHandedness.both;
            shootScript.handedness = controllerHandedness.both;
        }

        

        hapticsScript.enabled = true;
        shootScript.enabled = true;
    }

    public void deactivate()
    {
        activated = false;
        hapticsScript.enabled = false;
        shootScript.enabled = false;
    }

    public void hover()
    {
        if (!activated)
        {
            bool rController = false;
            if (rHand.interactablesHovered.Count > 0)
            {
                //Debug.Log(rHand.interactablesHovered[0]);
                foreach (IXRSelectInteractable interactor in rHand.interactablesHovered)
                {
                    if (interactor.transform.CompareTag("gun"))
                    {
                        rController = true;
                    }
                }
            }

            bool lController = false;
            if (lHand.interactablesHovered.Count > 0)
            {
                foreach (IXRSelectInteractable interactor in lHand.interactablesHovered)
                {
                    if (interactor.transform.CompareTag("gun"))
                    {
                        lController = true;
                    }
                }
            }




            if (lController && rController)
            {
                hapticsScript.handedness = controllerHandedness.both;
                shootScript.handedness = controllerHandedness.both;
            }
            else if (lController)
            {
                hapticsScript.handedness = controllerHandedness.left;
                shootScript.handedness = controllerHandedness.left;
            }
            else if (rController)
            {
                hapticsScript.handedness = controllerHandedness.right;
                shootScript.handedness = controllerHandedness.right;
            }
            else
            {
                hapticsScript.handedness = controllerHandedness.both;
                shootScript.handedness = controllerHandedness.both;
            }
            //Debug.Log(hapticsScript.handedness);
            //Debug.Log(lController + " " + rController);

            hoverHaptics = true;
        }
    }

    public void stopHover()
    {
        hoverHaptics = false;
        givingFeedback = false;
        StopAllCoroutines();
    }




    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hapticsScript = GetComponent<pointToLineHaptics>();
        shootScript = GetComponent<GunShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hoverHaptics && !givingFeedback)
        {
            float minTime = Mathf.Infinity;

            foreach (Target target in Target.targets)
            {
                if (target.remainingTime < minTime)
                {
                    minTime = target.remainingTime;
                }
            }
            minTime = (minTime - min) / div;
            //Debug.Log(minTime);

            if (minTime < 0)
            {
                minTime = 0;
            }
            if (minTime > 10)
            {
                minTime = 10;
            }

            hapticPattern.manualPattern.pulses[0].delay = minTime;

            playPattern(handedness);
        }
    }
}
