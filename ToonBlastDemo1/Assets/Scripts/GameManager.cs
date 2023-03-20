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
        SpawnNewCandies(chosenCandies);

    }

    private void SpawnNewCandies(List<List<int>> chosenCandies)
    {
        foreach (var candies in chosenCandies)
        {
            for (int i = 0; i < _candySpawner.Height - candies[1]; i++)// secilen sekerin ustundeki tum sekerlere tek tek erisim
            {
                GameObject upperCandy = _candySpawner.CandiesLocations[candies[0], candies[1] + i];//bir ust sekere erisim. !Once kendi yerine bakiyor. 
                if (upperCandy == null)
                {
                    _candySpawner.SpawnRandomCandy(candies[0], candies[1] + i);
                    break;
                }
            }
        }
    }

    private void FallUpperCandies(List<List<int>> chosenCandies)
    {
        foreach (var candies in chosenCandies)
        {
            int sayac = 0;
            //Patlatýlan sekerin ustune yeni bir seker gelmis fakat candies henuz bu sekere erisememis ise bu sekeri pas gec. Mesela ust uste 2 seker olsun. Alttaki seker 1, ustteki seker 2 olsun. Eger 1. sekere once basildiysa 1. seker ustte bulunan tum sekerleri asagi indirecek. Boylelike hem kendi yeri hem de 2. sekerin yeri dolabilecek. Eger 2. sekerin yeri 1. seker araciligiyla dolmussa 2. seker tekrar ustundeki sekerleri dusurme islemi yapmayacak. "!!! Alttaki if icin gecerlidir. !!!"
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
