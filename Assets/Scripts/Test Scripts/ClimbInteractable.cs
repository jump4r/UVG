﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractable : XRBaseInteractable
{

    // Must Be replaced with Action Based Equivilant to work.
    // protected override void OnSelectEnter(XRBaseInteractor interactor)
    // {
    //     base.OnSelectEnter(interactor);
    //     if (interactor is XRDirectInteractor)
    //     {
    //         Climber.climbingHand = interactor.GetComponent<XRController>();
    //     }
    // }

    // protected override void OnSelectExit(XRBaseInteractor interactor)
    // {
    //     base.OnSelectExit(interactor);
    //     if (interactor is XRDirectInteractor)
    //     {
    //         if (Climber.climbingHand && Climber.climbingHand.name == interactor.name)
    //         {
    //             Climber.climbingHand = null;
    //         }
    //     }
    // }
}
