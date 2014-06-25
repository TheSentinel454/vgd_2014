using UnityEngine;
using System.Collections;

public class RequirementManager : MonoBehaviour {
	public Component[] requirements;
	public bool completedAllRequirements { get {
		RequirementManager requirementManager;
		foreach (Component subrequirement in requirements) {
			// If the subrequirement is a Requirement, it is not going to have a requirement manager attached.
			// Therefore, we can check if the subrequirement is complete
			if (subrequirement is Requirement && (subrequirement as Requirement).complete == false)
				return false;

			// When the component has a requirement manager, we need to check if all of the requirements have been completed.
			// If they have all been completed, then that requirement has been completed.
			requirementManager = subrequirement.GetComponent<RequirementManager>();
			if (requirementManager != this && requirementManager != null && requirementManager.completedAllRequirements == false) 
				return false;
		}

		return true;
	}}
}
