using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCard : MonoBehaviour
{
	public Card card;

	public bool taked;

	public SpriteRenderer frontCard;
	public SpriteRenderer backCard;

	public Vector2 initPos;

	private void Start()
	{
		if (card != null)
		{
			if (frontCard != null)
				frontCard.sprite = card.spriteCard;

			if (backCard != null)
				backCard.sortingOrder = frontCard.sortingOrder;
		}
	}
}
