using UnityEngine;

class GUIController : MonoBehaviour
{
    private HealthSystem zenBar;
	private ManaSystem airBar;
	private ManaSystem fireBar;
	private ManaSystem waterBar;

    public Rect ZenDimensions;
	public Vector2 size;
	public Vector2 position;
    public Texture ZenBackgroundTexture;
    public Texture ZenTexture;

    public Rect ManaBarDimens;
    public Rect ManaBarScrollerDimens;
    public Texture ManaBubbleTexture;
    public Texture ManaTexture;
    public float ManaBubbleTextureRotation;

    public void Start()
    {
		// Instantiate the Zen Bar
		Rect t = new Rect ();
		t.x = (position.x * Screen.currentResolution.width);
		t.y = (position.y * Screen.currentResolution.height);
		t.width = size.x;
		t.height = size.y;
		zenBar = new HealthSystem(t, false, ZenBackgroundTexture, ZenTexture, 0.0f);
		airBar = new ManaSystem(ManaBarDimens, ManaBarScrollerDimens, false, ManaBubbleTexture, ManaTexture, 0.0f);
		fireBar = new ManaSystem(ManaBarDimens, ManaBarScrollerDimens, false, ManaBubbleTexture, ManaTexture, 0.0f);
		waterBar = new ManaSystem(ManaBarDimens, ManaBarScrollerDimens, false, ManaBubbleTexture, ManaTexture, 0.0f);

		// Initialize the bars
        zenBar.Initialize();
		airBar.Initialize();
		fireBar.Initialize();
		waterBar.Initialize();
    }

    public void OnGUI()
    {
		// Draw the Zen Bar
        zenBar.DrawBar();
		// Draw the energy bars
		airBar.DrawBar();
		fireBar.DrawBar();
		waterBar.DrawBar();
    }

    public void Update()
    {
		// Update the Zen Bar
        zenBar.Update();
		// Update the energy bars
		airBar.Update();
		fireBar.Update();
		waterBar.Update();
    }
}
