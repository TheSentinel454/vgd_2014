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
public class SetHealth : RAINAction
{
	// Private Attributes
	private DamageObject dmgObj;

	/// <summary>
	/// Initializes a new instance of the <see cref="SetHealth"/> class.
	/// </summary>
    public SetHealth()
    {
        actionName = "SetHealth";
    }

	/// <summary>
	/// Start the specified ai.
	/// </summary>
	/// <param name="ai">Ai.</param>
    public override void Start(AI ai)
    {
		// Get the Damage Object
		dmgObj = ai.Body.GetComponentInChildren<DamageObject>();
        base.Start(ai);
    }

	/// <summary>
	/// Execute the specified ai.
	/// </summary>
	/// <param name="ai">Ai.</param>
    public override ActionResult Execute(AI ai)
    {
		// Update the health for this AI
		ai.WorkingMemory.SetItem<float>("health", dmgObj.health);
        return ActionResult.SUCCESS;
    }

	/// <summary>
	/// Stop the specified ai.
	/// </summary>
	/// <param name="ai">Ai.</param>
    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}