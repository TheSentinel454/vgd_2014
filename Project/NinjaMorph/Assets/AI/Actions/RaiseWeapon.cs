/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class RaiseWeapon : RAINAction {

	Animator animator;
	float weaponLayerWeight;
	float accessoryLayerWeight;

    public RaiseWeapon() {
        actionName = "RaiseWeapon";
    }

    public override void Start(AI ai) {
		animator = ai.Body.GetComponent<Animator> ();
		weaponLayerWeight = animator.GetLayerWeight (1);
		accessoryLayerWeight = animator.GetLayerWeight (2);
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai) {
		if (weaponLayerWeight < 1) {
			weaponLayerWeight += 0.05f;
			animator.SetLayerWeight (1, weaponLayerWeight);
		}

		if (accessoryLayerWeight < 1) {
			accessoryLayerWeight += 0.05f;
			animator.SetLayerWeight (2, accessoryLayerWeight);
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai) {
        base.Stop(ai);
    }
}