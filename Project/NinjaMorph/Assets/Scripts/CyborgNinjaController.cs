using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CyborgNinjaAnimationController))]
public class CyborgNinjaController : MonoBehaviour {

	// ANIMATIONS
	CyborgNinjaAnimationController animationController;

	// CONTROLS
	public KeyCode attackKeyCode = KeyCode.Slash;
	public KeyCode toggleWeaponKeyCode = KeyCode.Period;

	// HEALTH
	public float health = 100.0f;
	public float maxHealth = 100.0f;
	public float healthRegenerationRate = 0.01f;

	// ENERGY
	public float maxEnergy = 100.0f;
	public float energyRegenerationRate = 0.01f;

	// STYLES
	GUIStyle healthBarStyle = new GUIStyle ();

	// TEXTURES
	Texture2D healthBarTexture;

	// Get the animation controller
	void Start () {
		animationController = GetComponent<CyborgNinjaAnimationController> ();
		healthBarTexture = new Texture2D(1, 1);
		healthBarTexture.SetPixel (0, 0, new Color(1.0f, 0.92f, 0.016f, 0.2f));
		healthBarTexture.Apply ();
		healthBarStyle.normal.background = healthBarTexture;
	}
	
	// Determine what the ninja should be doing.
	void FixedUpdate () {
		// ANIMATION TOGGLES
		animationController.attacking = animationController.attacking || Input.GetKey (attackKeyCode);
		animationController.weapon ^= Input.GetKeyDown (toggleWeaponKeyCode);
		animationController.dead = health <= 0;
	}

	// Draw the health and energy GUI
	void OnGUI() {
		// HEALTH BAR CONTAINER
		float healthBarContainerWidth = 250.0f;
		float healthBarContainerHeight = 30.0f;
		float healthBarContainerTop = GUI.skin.window.padding.top;
		float healthBarContainerLeft = Screen.width - healthBarContainerWidth - GUI.skin.window.padding.right;

		// HEALTH BAR 
		float healthBarWidth = (health / maxHealth) * (healthBarContainerWidth - GUI.skin.box.padding.horizontal);
		float healthBarHeight = healthBarContainerHeight - GUI.skin.box.padding.vertical;
		float healthBarTop = GUI.skin.box.padding.top;
		float healthBarLeft = GUI.skin.box.padding.left;

		// HEALTH BAR GUI
		GUI.BeginGroup (new Rect (healthBarContainerLeft, healthBarContainerTop, healthBarContainerWidth, healthBarContainerHeight));
			GUI.Box(new Rect(0, 0, healthBarContainerWidth, healthBarContainerHeight), "");
			GUI.Box(new Rect(healthBarLeft, healthBarTop, healthBarWidth, healthBarHeight), "", healthBarStyle);
		GUI.EndGroup ();
	}
}
