using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	protected bool win;
	protected int currentPoints;
	protected CCard cardThrowed;

	protected List<CCard> myCards;

	public int CardsCount => myCards.Count;

	public bool turnOver;
	public CCard CardThrowed => cardThrowed;

	public bool TurnOver => turnOver;

	protected virtual void Awake()
	{
		myCards = new List<CCard>();
	}

	protected virtual void Start()
	{

	}

	protected virtual void Update()
	{

	}

	//Method of pulling paper on the table
	protected virtual void PlayCard(Vector2 cardPlayedDest)
	{
		if (cardThrowed != null)
		{
			myCards.Remove(cardThrowed);
			cardThrowed.initPos = cardThrowed.transform.localPosition;
			cardThrowed.transform.parent = null;

			Vector2 currentPosCard = cardThrowed.transform.localPosition;
			StartCoroutine(GameManager.Instance.MoveTo(cardThrowed.transform, currentPosCard, cardPlayedDest, 3.0f));

			Quaternion currentRotCard = cardThrowed.transform.localRotation;
			StartCoroutine(GameManager.Instance.RotTo(cardThrowed.transform, currentRotCard, Quaternion.identity, 3.0f));
		
		}
	}

	//Adds a card to the player's hand
	public void CardTaked(CCard getCard)
	{
		myCards.Add(getCard);
	}

	//Method to take the card from the deck every time you finish your turn
	protected virtual void GiveCard()
	{
		if (GameManager.Instance.GetDeckCount() > 0)
		{
			if (GetType() == typeof(PlayerMySelf))
			{
				GameManager.Instance.GiveCard(GameManager.Instance.playerTransform, this as PlayerMySelf);
			}
			else
			{
				GameManager.Instance.GiveCard(GameManager.Instance.opponentTransform, this as PlayerOpponent);
			}
		}
	}

	protected virtual void PassesTurn()
	{
		turnOver = false;
	}
}
