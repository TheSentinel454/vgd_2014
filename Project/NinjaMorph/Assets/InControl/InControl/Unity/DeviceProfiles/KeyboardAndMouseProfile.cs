using System;
using System.Collections;
using UnityEngine;
using InControl;


namespace InControl
{
	// This custom profile is enabled by adding it to the Custom Profiles list
	// on the InControlManager script, which is attached to the InControl 
	// game object in this example scene.
	// 
	[AutoDiscover]
	public class KeyboardAndMouseProfile : UnityInputDeviceProfile
	{
		public KeyboardAndMouseProfile()
		{
			Name = "Keyboard/Mouse";
			Meta = "A keyboard and mouse combination profile appropriate for FPS.";

			// This profile only works on desktops.
			SupportedPlatforms = new[]
			{
				"Windows",
				"Mac",
				"Linux"
			};

			Sensitivity = 1.0f;
			LowerDeadZone = 0.0f;
			UpperDeadZone = 1.0f;

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Fire - Mouse",
					Target = InputControlType.RightTrigger,
					Source = MouseButton0
				},
				new InputControlMapping
				{
					Handle = "Fire - Keyboard",
					Target = InputControlType.RightTrigger,
					Source = KeyCodeButton( KeyCode.LeftControl )
				},
				new InputControlMapping
				{
					Handle = "Fire - Keyboard2",
					Target = InputControlType.RightTrigger,
					Source = KeyCodeButton( KeyCode.Return )
				},
				new InputControlMapping
				{
					Handle = "Jump",
					Target = InputControlType.Action1,
					Source = KeyCodeButton( KeyCode.Space )
				},
				new InputControlMapping
				{
					Handle = "Air Ninja",
					Target = InputControlType.Action4,
					Source = KeyCodeButton( KeyCode.Alpha1 )
				},
				new InputControlMapping
				{
					Handle = "Fire Ninja",
					Target = InputControlType.Action2,
					Source = KeyCodeButton( KeyCode.Alpha2 )
				},
				new InputControlMapping
				{
					Handle = "Water Ninja",
					Target = InputControlType.Action3,
					Source = KeyCodeButton( KeyCode.Alpha3 )
				},
				new InputControlMapping
				{
					Handle = "Run",
					Target = InputControlType.LeftTrigger,
					Source = KeyCodeButton( KeyCode.LeftShift )
				},
				new InputControlMapping
				{
					Handle = "Menu",
					Target = InputControlType.Menu,
					Source = KeyCodeButton( KeyCode.Escape )
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = KeyCodeButton( KeyCode.Escape )
				},
			};

			AnalogMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Move X",
					Target = InputControlType.LeftStickX,
					Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
				},
				new InputControlMapping
				{
					Handle = "Move Y",
					Target = InputControlType.LeftStickY,
					Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
				},
				new InputControlMapping {
					Handle = "Move X Alternate",
					Target = InputControlType.LeftStickX,
					Source = KeyCodeAxis( KeyCode.LeftArrow, KeyCode.RightArrow )
				},
				new InputControlMapping {
					Handle = "Move Y Alternate",
					Target = InputControlType.LeftStickY,
					Source = KeyCodeAxis( KeyCode.DownArrow, KeyCode.UpArrow )
				},
				new InputControlMapping
				{
					Handle = "Look X",
					Target = InputControlType.RightStickX,
					Source = MouseXAxis,
					Raw    = true,
					Scale  = 0.1f
				},
				new InputControlMapping
				{
					Handle = "Look Y",
					Target = InputControlType.RightStickY,
					Source = MouseYAxis,
					Raw    = true,
					Scale  = 0.1f
				},
				new InputControlMapping
				{
					Handle = "Zoom",
					Target = InputControlType.ScrollWheel,
					Source = MouseScrollWheel,
					Raw    = true,
					Scale  = 0.1f
				}
			};
		}
	}
}

