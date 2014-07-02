using UnityEngine;
using System.Collections;

public class DestroyGuiTextByTime : MonoBehaviour
{
	public float lifetime;
	private GUIText text;
	
	void Start()
	{
		// Check for a Gui Text
		text = (GUIText)GetComponent<GUIText> ();
		if (text != null)
		{
			// Set the message color
			text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
		}
		else
		{
			Debug.LogError("DestroyGuiTextByTime script attached to an object without a GUI Text!");
		}
	}
	
	void Update()
	{
		// Check for a Gui Text
		if (text != null)
		{
			// Get the current alpha
			float currentAlpha = text.color.a;
			// Set the alpha based on the time passed
			currentAlpha -= ((Time.deltaTime / lifetime) * 1.0f);
			// Check for alpha below zero
			if (currentAlpha < 0.0f)
			{
				// Make sure we set it to zero
				currentAlpha = 0.0f;
				// Now destroy the object
				Destroy(gameObject);
			}
			text.color = new Color(text.color.r, text.color.g, text.color.b, currentAlpha);
		}
	}
}
