using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class GameManager : Singleton<GameManager>
{
	private int deckCount = 18;

	private List<CCard> deckCards;

	private PlayerMySelf playerMySelf;
	private PlayerOpponent playerOpponent;

	private int pointsPlayer;
	private int pointsOpponent;

	private float destroyTimeCardThrowed = 1.5f;

	private bool cardsChecked;

	public CCard cardPrefab;

	public Transform deck;

	public Transform playerTransform;

	public Transform opponentTransform;

	public float OffsetCardPos;

	public Text txtPlayerPoints;
	public Text txtOpponentsPoints;

	public Text txtPlayerWin;
	public Text txtStatusGameOver;

	public GameObject gameOverPopUp;
	public PlayerMySelf PlayerMySelf => playerMySelf;
	public PlayerOpponent PlayerOpponent => playerOpponent;

	protected override void Awake()
	{
		base.Awake();

		deckCards = new List<CCard>();
		playerMySelf = FindObjectOfType<PlayerMySelf>();
		playerOpponent = FindObjectOfType<PlayerOpponent>();
	}

	// Start is called before the first frame update
	void Start()
	{
		CreateDeck();

		DistributeCards(playerTransform, 3, false);
		DistributeCards(opponentTransform, 3, true);
	}

	private void Update()
	{
		if (!cardsChecked)
		{
			if (playerMySelf.CardThrowed != null && playerOpponent.CardThrowed != null)
			{
				CheckCardsThrowed(playerMySelf.CardThrowed.card.typeCard, playerOpponent.CardThrowed.card.typeCard, TypeCard.Rock, TypeCard.Scissor);
				CheckCardsThrowed(playerMySelf.CardThrowed.card.typeCard, playerOpponent.CardThrowed.card.typeCard, TypeCard.Paper, TypeCard.Rock);
				CheckCardsThrowed(playerMySelf.CardThrowed.card.typeCard, playerOpponent.CardThrowed.card.typeCard, TypeCard.Scissor, TypeCard.Paper);
			}
		}

		if (playerMySelf.CardThrowed == null && playerOpponent.CardThrowed == null)
		{
			cardsChecked = false;
		}

		if (GetDeckCount() <= 0 && playerMySelf.CardsCount <= 0 && playerOpponent.CardsCount <= 0)
		{
			Invoke("GameOver", 2.0f);
		}
	}

	//Method for defining when the game is over
	public void GameOver()
	{
		if (GetDeckCount() <= 0 && playerMySelf.CardsCount <= 0 && playerOpponent.CardsCount <= 0)
		{
			//GameOver
			if (gameOverPopUp != null)
			{
				gameOverPopUp.SetActive(true);

				if (pointsOpponent == pointsPlayer)
				{
					//drawText.SetActive(true);
					CheckPlayerWin(txtStatusGameOver, false, true);
					return;
				}

				else if (pointsPlayer > pointsOpponent)
				{
					//playerWinText.SetActive(true);
					CheckPlayerWin(txtStatusGameOver, true);
					return;
				}

				else if (pointsOpponent > pointsPlayer)
				{
					//opponentWinText.SetActive(true);
					CheckPlayerWin(txtStatusGameOver, false);
					return;
				}

			}
		}
	}

	//Method of getting the amount of cards in the deck
	public int GetDeckCount()
	{
		return deckCards.Count;
	}

	//Create deck 
	//Spawn all cards wanted 
	//and set the type of each card
	private void CreateDeck()
	{
		for (int i = 0; i < deckCount; i++)
		{
			CCard currentCard = Instantiate(cardPrefab, deck);
			currentCard.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
			currentCard.frontCard.sortingOrder = i + 1;

			if (!deckCards.Contains(currentCard))
				deckCards.Add(currentCard);
		}

		GenerateCard(6, Resources.Load<Card>("Rock"));
		GenerateCard(6, Resources.Load<Card>("Paper"));
		GenerateCard(6, Resources.Load<Card>("Scissor"));
	}

	//Method of generating the deck of cards
	private void GenerateCard(int countCards, Card card)
	{
		if (deckCards.Count > 0)
		{
			for (int i = 0; i < countCards; i++)
			{
				deckCards[RandomGenerator.Instance.GetRandomNumber()].card = card;
			}
		}
	}

	//Method of dealing cards to each player at the initial stage
	private void DistributeCards(Transform currentPlayer, int quantityCardToDistribute, bool isOpponent)
	{
		CCard currentCard = null;

		if (isOpponent)
		{
			for (int i = 0; i < quantityCardToDistribute; i++)
			{
				currentCard = deckCards[i];
				deckCards.Remove(currentCard);
				currentCard.transform.parent = currentPlayer;

				Vector2 currentCardPos = currentCard.transform.localPosition;
				currentCardPos = new Vector3(1 + i * OffsetCardPos, 0.5f, 0);
				currentCard.transform.localPosition = currentCardPos;

				playerOpponent.CardTaked(currentCard);
			}
		}
		else
		{
			for (int i = 0; i < quantityCardToDistribute; i++)
			{
				currentCard = deckCards[i];
				deckCards.Remove(currentCard);
				currentCard.transform.parent = currentPlayer;

				Vector2 currentCardPos = currentCard.transform.localPosition;
				currentCardPos = new Vector3(1 + i * OffsetCardPos, 2f, 0);
				currentCard.transform.localPosition = currentCardPos;

				Quaternion currentCardRot = currentCard.transform.localRotation;
				currentCardRot = new Quaternion(0, 0, 0, 0);
				currentCard.transform.localRotation = currentCardRot;

				playerMySelf.CardTaked(currentCard);

			}
		}
	}

	//Method of giving a card every time the player passes the turn
	public void GiveCard(Transform cardDest, Player currentPlayer)
	{
		CCard cardToGive = deckCards[0];

		if (cardToGive != null)
		{
			deckCards.Remove(cardToGive);
			currentPlayer.CardTaked(cardToGive);

			cardToGive.transform.parent = null;
			cardToGive.transform.parent = cardDest;

			Vector2 currentCardPos = currentPlayer.CardThrowed.initPos;

			StartCoroutine(MoveTo(cardToGive.transform, cardToGive.transform.localPosition, currentCardPos, 2.0f));

			if (currentPlayer.GetType() == typeof(PlayerMySelf))
			{
				Quaternion currentCardRot = cardToGive.transform.localRotation;
				StartCoroutine(RotTo(cardToGive.transform, cardToGive.transform.localRotation, Quaternion.identity, 2.0f));
			}
		}
	}

	//Method to control the cards thrown
	private void CheckCardsThrowed(TypeCard playerTypeCard, TypeCard opponentTypeCard, TypeCard cardWinner, TypeCard cardLoser)
	{
		if (playerTypeCard == opponentTypeCard)
		{
			//print("Draw");

			Destroy(playerMySelf.CardThrowed.gameObject, destroyTimeCardThrowed);
			Destroy(playerOpponent.CardThrowed.gameObject, destroyTimeCardThrowed);

			CheckPlayerWin(txtPlayerWin, true, true);

			cardsChecked = true;
			return;

		}

		else if (playerTypeCard == cardWinner && opponentTypeCard == cardLoser)
		{
			//print("Player Win!");

			pointsPlayer += 1;

			if (txtPlayerPoints != null)
				txtPlayerPoints.text = pointsPlayer.ToString();

			CheckPlayerWin(txtPlayerWin, true);

			Destroy(playerMySelf.CardThrowed.gameObject, destroyTimeCardThrowed);
			Destroy(playerOpponent.CardThrowed.gameObject, destroyTimeCardThrowed);

			cardsChecked = true;
			return;

			//Metti turn over player a falso
		}

		else if (opponentTypeCard == cardWinner && playerTypeCard == cardLoser)
		{
			//print("Opponent Win!");

			pointsOpponent += 1;

			if (txtOpponentsPoints != null)
				txtOpponentsPoints.text = pointsOpponent.ToString();

			CheckPlayerWin(txtPlayerWin, false);

			Destroy(playerMySelf.CardThrowed.gameObject, destroyTimeCardThrowed);
			Destroy(playerOpponent.CardThrowed.gameObject, destroyTimeCardThrowed);

			cardsChecked = true;
			return;

			//Metti turn over player a falso
		}
	}

	//Method to check which player won the game or if they drew
	private void CheckPlayerWin(Text currentText, bool win, bool draw = false)
	{
		if (currentText == null) return;

		currentText.gameObject.SetActive(true);

		if (draw)
		{
			currentText.text = "<color=#000000> DRAW </color>";
		}
		else if (win)
		{
			currentText.text = "<color=#22FF00> YOU WIN </color>";

		}
		else if (!win)
		{
			currentText.text = "<color=#FF0000> YOU LOSE </color>";
		}

	}

	public IEnumerator MoveTo(Transform card, Vector2 startPos, Vector2 endPos, float time)
	{
		float t = 0.0f;

		while (t < 1f)
		{
			t += Time.deltaTime;
			card.localPosition = Vector2.Lerp(startPos, endPos, t * time);
			yield return null;
		}
	}

	public IEnumerator RotTo(Transform card, Quaternion startRot, Quaternion endRot, float time)
	{
		float t = 0.0f;

		while (t < 1f)
		{
			t += Time.deltaTime;
			card.localRotation = Quaternion.Slerp(startRot, endRot, t * time);
			yield return null;
		}
	}

	public void ReloadLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ExitLevel()
	{
		SceneManager.LoadScene(0);
	}

}
