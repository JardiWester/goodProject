using System.Collections;
using System.Collections.Generic;
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


    //this runs when you press the select button on interactor on the gun
    //so this basically runs when you pick the gun up
    public void activate()
    {
        //activates is true when you're holding the gun
        activated = true;
        stopHover();

        //bools for which controllers should recieve haptics
        bool rController = false;
        bool lController = false;

        //if the right controller has anything selected:
        if (rHand.interactablesSelected.Count > 0)
        {
            //check if any of them are the gun:
            foreach (IXRSelectInteractable interactor in rHand.interactablesSelected)
            {
                if (interactor.transform.CompareTag("gun"))
                {
                    //if so, this controller should recieve haptics from the gun
                    rController = true;
                }
            }
        }
        
        //same for the left hand
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

        //if they're both selecting the gun:
        if (lController && rController)
        {
            //both controllers should recieve haptic feedback
            hapticsScript.handedness = controllerHandedness.both;
            //and both controllers should be able to shoot when the shoot button is pressed
            shootScript.handedness = controllerHandedness.both;
        }
        else if (lController)
        {
            //if only the left controller is selecting the gun
            //only the left controller should be considered
            hapticsScript.handedness = controllerHandedness.left;
            shootScript.handedness = controllerHandedness.left;
        }
        else if (rController)
        {
            //same for the right
            hapticsScript.handedness = controllerHandedness.right;
            shootScript.handedness = controllerHandedness.right;
        }
        else
        {
            //as a failsafe, default to using both controllers for everything
            //this should never happen, but just to be sure
            hapticsScript.handedness = controllerHandedness.both;
            shootScript.handedness = controllerHandedness.both;
        }
        //enable the guns functionality
        hapticsScript.enabled = true;
        shootScript.enabled = true;
    }

    //this should run when you let go of the gun
    public void deactivate()
    {
        //waaay simpler compared to activate XD
        //just turn everything off again
        activated = false;
        hapticsScript.enabled = false;
        shootScript.enabled = false;
    }

    //this should run hen you're hovering over the gun
    //so in vr, that means pointing to the gun
    public void hover()
    {
        //only if you're not holding it:
        if (!activated)
        {
            //do the same checks for what controller to use as before,
            //but check for selectInteractables instead

            bool rController = false;
            if (rHand.interactablesHovered.Count > 0)
            {
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


            //decide what controller is used here
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

            //make the pattern play in update:
            hoverHaptics = true;
        }
    }

    public void stopHover()
    {
        //just stop playing the pattern
        hoverHaptics = false;
        givingFeedback = false;
        StopAllCoroutines();
    }

    protected override void Start()
    {
        //run the start function of the hapticsPlayerParent
        //here is nothing in it, but i added this so i could
        //add anything to it without it breaking 
        base.Start();
        //find the other scripts
        hapticsScript = GetComponent<pointToLineHaptics>();
        shootScript = GetComponent<GunShoot>();
    }

    void Update()
    {
        //if it should play tha pattern, and if it isn't already playing the pattern
        if (hoverHaptics && !givingFeedback)
        {
            //the minimum minTime should be 10,
            //any longer than that and it shouldn't just play for ages
            float minTime = 10;

            foreach (Target target in Target.targets)
            {
                //check every target to see which is closest
                //or if none are closer than 10 seconds,
                //it'll just stay 10
                if (target.remainingTime < minTime)
                {
                    minTime = target.remainingTime;
                }
            }

            //this is the script i used to test what values are correct
            //i landed on -10 and /15, and i also used these in the
            //pointToLineHaptics script
            minTime = (minTime - min) / div;

            //because of the min, it could end up being negative 
            if (minTime < 0)
            {
                minTime = 0;
            }

            //play a pattern with the mintime as the delay
            hapticPattern.manualPattern.pulses[0].delay = minTime;
            playPattern(handedness);
        }
    }
}
