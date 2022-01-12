using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleUI : MonoBehaviour
{
	private void OnEnable()
	{
		Invoke("Deactive",1.0f);
	}

	private void Deactive()
	{
		this.gameObject.SetActive(false);
	}
}
