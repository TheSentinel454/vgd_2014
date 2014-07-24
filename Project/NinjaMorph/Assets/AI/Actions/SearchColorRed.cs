using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class SearchColorRed : RAINAction
{
    public SearchColorRed()
    {
        actionName = "SearchColorRed";
    }

    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		foreach (Light light in ai.Body.GetComponentsInChildren<Light> ()) {
			light.color = new Color(255, 0, 0);
			light.intensity = 0.05f;
		}
        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}