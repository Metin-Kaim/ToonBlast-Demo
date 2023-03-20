using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    CandySpawner _candySpawner;

    private void Awake()
    {
        Instance = this;
        _candySpawner = GetComponent<CandySpawner>();
    }

    public void CandiesDestroyNFall(List<List<int>> chosenCandies)
    {
        DestroyAllChosenCandies(chosenCandies);
        FallUpperCandies(chosenCandies);
    }

    private void FallUpperCandies(List<List<int>> chosenCandies)
    {
        foreach (var candies in chosenCandies)
        {
            int sayac = 0;
            if (_candySpawner.CandiesLocations[candies[0], candies[1]] == null)
            {
                for (int i = 1; i < _candySpawner.Height - candies[1]; i++)// secilen sekerin ustundeki tum sekerlere tek tek erisim
                {
                    GameObject upperCandy = _candySpawner.CandiesLocations[candies[0], candies[1] + i];//bir ust sekere erisim.
                    if (upperCandy != null)
                    {

                        upperCandy.GetComponent<Candy>().InstantiateCandy(candies[0], candies[1] + i - 1 - sayac);
                        upperCandy.GetComponent<Candy>().CandyCanFall = true;
                        _candySpawner.CandiesLocations[candies[0], candies[1] + i] = null;
                    }
                    else
                    {
                        sayac++;
                    }
                }
            }
        }
    }

    private void DestroyAllChosenCandies(List<List<int>> chosenCandies)
    {
        foreach (var candies in chosenCandies)
        {
            Destroy(_candySpawner.CandiesLocations[candies[0], candies[1]]);
            _candySpawner.CandiesLocations[candies[0], candies[1]] = null;
        }
    }
}
