using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class LowerWeapon : RAINAction {
	
	Animator animator;
	float weaponLayerWeight;
	float accessoryLayerWeight;
	
	public LowerWeapon() {
		actionName = "LowerWeapon";
	}
	
	public override void Start(AI ai) {
		animator = ai.Body.GetComponent<Animator> ();
		weaponLayerWeight = animator.GetLayerWeight (1);
		accessoryLayerWeight = animator.GetLayerWeight (2);
		base.Start(ai);
	}
	
	public override ActionResult Execute(AI ai) {
		if (weaponLayerWeight > 0) {
			weaponLayerWeight -= 0.05f;
			animator.SetLayerWeight (1, weaponLayerWeight);
		}

		if (accessoryLayerWeight > 0) {
			accessoryLayerWeight -= 0.05f;
			animator.SetLayerWeight (2, accessoryLayerWeight);
		}
		
		return ActionResult.SUCCESS;
	}
	
	public override void Stop(AI ai) {
		base.Stop(ai);
	}
}