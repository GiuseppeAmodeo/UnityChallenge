using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMySelf : Player
{
	private Ray ray;
	private RaycastHit hit;
	private Camera cam;

	protected override void Awake()
	{
		base.Awake();
		cam = Camera.main;
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		//if (GameManager.Instance.PlayerOpponent.TurnOver)
		//{
		//	turnOver = false;
		//}

		if (GameManager.Instance.PlayerOpponent.TurnOver)
		{
			if (Input.GetMouseButton(0))
				ChooseCard();
		}
		else
		{
			print("Wait for the opponent turn!");
		}
	}

	//Method to allow the user to select the card to be launched
	private void ChooseCard()
	{
		ray = cam.ScreenPointToRay(Input.mousePosition);

		if (!turnOver)
		{
			if (Physics.Raycast(ray, out hit, 50.0f))
			{
				cardThrowed = hit.transform.GetComponent<CCard>();

				if (cardThrowed != null)
				{
					PlayCard(Vector2.zero);
					turnOver = true;

					Invoke("GiveCard",1.0f);
					Invoke("PassesTurn",2.0f);
				}
			}
		}
	}
}
