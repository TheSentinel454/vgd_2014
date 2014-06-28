/*
  Team Y-Not
  
  Evan LaHurd
  Luke Tornquist
  Jonathan Yates
*/

using UnityEngine;
using System.Collections;

public class Torchelight : MonoBehaviour
{
	public GameObject TorchLight;
	public GameObject MainFlame;
	public GameObject BaseFlame;
	public GameObject Etincelles;
	public GameObject Fumee;
	public float MaxLightIntensity;
	public float IntensityLight;

	private ParticleSystem mainFlame;
	private ParticleSystem baseFlame;
	private ParticleSystem etincelles;
	private ParticleSystem fumee;

	void Start ()
	{
		TorchLight.light.intensity = IntensityLight;

		mainFlame = MainFlame.GetComponent<ParticleSystem> ();
		mainFlame.emissionRate = IntensityLight * 20f;

		baseFlame = BaseFlame.GetComponent<ParticleSystem> ();
		baseFlame.emissionRate = IntensityLight * 15f;

		etincelles = Etincelles.GetComponent<ParticleSystem>();
		etincelles.emissionRate = IntensityLight * 7f;

		fumee = Fumee.GetComponent<ParticleSystem> ();
		fumee.emissionRate = IntensityLight * 12f;
	}
	

	void Update ()
	{
		if (IntensityLight < 0)
		{
			IntensityLight = 0;
		}
		if (IntensityLight > MaxLightIntensity)
		{
			IntensityLight = MaxLightIntensity;
		}

		TorchLight.light.intensity = IntensityLight/2f+Mathf.Lerp(IntensityLight-0.1f,IntensityLight+0.1f,Mathf.Cos(Time.time*30));

		TorchLight.light.color=new Color(Mathf.Min(IntensityLight/1.5f,1f),Mathf.Min(IntensityLight/2f,1f),0f);
		mainFlame.emissionRate=IntensityLight*20f;
		baseFlame.emissionRate=IntensityLight*15f;
		etincelles.emissionRate=IntensityLight*7f;
		fumee.emissionRate=IntensityLight*12f;
	}
}
