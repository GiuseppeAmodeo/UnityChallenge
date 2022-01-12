using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : Singleton<RandomGenerator>
{
	public int minNumber = 0;
	public int maxNumber = 18;

	public List<int> _validNumbers;

	protected override void Awake()
	{
		base.Awake();

		_validNumbers = new List<int>();

		for (int i = minNumber; i <= maxNumber; i++)
			_validNumbers.Add(i);
	}

	public int GetRandomNumber()
	{
		var nextIndex = Random.Range(0, _validNumbers.Count - 1);
		var result = _validNumbers[nextIndex];
		_validNumbers.RemoveAt(nextIndex);
		return result;
	}
}
