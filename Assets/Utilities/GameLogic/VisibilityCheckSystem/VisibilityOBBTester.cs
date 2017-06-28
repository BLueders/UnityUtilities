using UnityEngine;
using System.Collections;

public class VisibilityOBBTester : VisibilityOBB
{
	public Renderer rend;

	void Start ()
	{
		rend.material.color = Color.red;
	}

	public override void OnBecomeVisible ()
	{
		rend.material.color = Color.blue;			
	}

	public override void OnStopVisible ()
	{
		rend.material.color = Color.red;
	}
}

