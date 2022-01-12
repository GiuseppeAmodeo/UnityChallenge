using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpponent : Player
{
	delegate void PlayerOpponentState();

	private PlayerOpponentState currentState;

	protected override void Awake()
	{
		base.Awake();
		currentState = OnWaitState;
	}

	protected override void Start()
	{
		base.Start();
		turnOver = true;
	}

	protected override void Update()
	{
		base.Update();

		currentState();
	}

	//Final State Machine for IA 
	#region FSM
	private void OnWaitState()
	{
		//In this state the AI waits for the player's moves
		if (GameManager.Instance.PlayerMySelf.TurnOver)
		{
			currentState = OnStartState;

			turnOver = false;
			cardThrowed = myCards[Random.Range(0, myCards.Count)];
		}
	}

	private void OnStartState()
	{
		//In this state the AI throws a random card among those in his hand
		PlayCard(new Vector2(3.0f, 2.0f));
		currentState = OnExitState;
	}

	private void OnExitState()
	{
		//In this state the AI draws a card from the deck
		if (!turnOver)
			Invoke("GiveCard", 1.0f);

		turnOver = true;

		//se turn over e' falso switcho stato
		if (!GameManager.Instance.PlayerMySelf.turnOver)
			currentState = OnWaitState;
	}
	#endregion
}
