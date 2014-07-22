using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class DetectWallsBehind : RAINAction
{
    public DetectWallsBehind()
    {
        actionName = "DetectWallsBehind";
    }

    public override void Start(AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		
		ai.WorkingMemory.SetItem<bool>("WallBehind", !ai.Navigator.CurrentGraph.IsPointOnGraph(ai.Body.transform.position + -ai.Body.transform.forward, 5));
        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}