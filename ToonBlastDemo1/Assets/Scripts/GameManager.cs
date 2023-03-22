using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int _height, _width;

    CandySpawner _candySpawner;
    GameObject[,] _candiesLocations;

    public bool CanClick { get; set; }
    public int Height => _height;
    public int Width => _width;

    public GameObject[,] CandiesLocations => _candiesLocations;

    private void Awake()
    {
        Instance = this;
        _candySpawner = GetComponent<CandySpawner>();
    }

    private void Start()
    {
        _candiesLocations = new GameObject[Width, Height];
    }

    public void CandiesDestroyNFall(List<List<int>> chosenCandies)
    {
        DestroyAllChosenCandies(chosenCandies);
        FallUpperCandies(chosenCandies);
        SpawnNewCandies(chosenCandies);
        CheckForCombos();
    }
    private void DestroyAllChosenCandies(List<List<int>> chosenCandies)
    {
        foreach (var candies in chosenCandies)
        {
            Destroy(CandiesLocations[candies[0], candies[1]]);
            CandiesLocations[candies[0], candies[1]] = null;
        }
    }
    private void FallUpperCandies(List<List<int>> chosenCandies)
    {
        foreach (var candies in chosenCandies)
        {
            int sayac = 0;
            //Patlatýlan sekerin ustune yeni bir seker gelmis fakat candies henuz bu sekere erisememis ise bu sekeri pas gec. Mesela ust uste 2 seker olsun. Alttaki seker 1, ustteki seker 2 olsun. Eger 1. sekere once basildiysa 1. seker ustte bulunan tum sekerleri asagi indirecek. Boylelike hem kendi yeri hem de 2. sekerin yeri dolabilecek. Eger 2. sekerin yeri 1. seker araciligiyla dolmussa 2. seker tekrar ustundeki sekerleri dusurme islemi yapmayacak. "!!! Alttaki if icin gecerlidir. !!!"
            if (CandiesLocations[candies[0], candies[1]] == null)
            {
                for (int i = 1; i < Height - candies[1]; i++)// secilen sekerin ustundeki tum sekerlere tek tek erisim
                {
                    GameObject upperCandy = CandiesLocations[candies[0], candies[1] + i];//bir ust sekere erisim.
                    if (upperCandy != null)
                    {
                        upperCandy.GetComponent<Candy>().InstantiateCandy(candies[0], candies[1] + i - 1 - sayac);
                        upperCandy.GetComponent<Candy>().CandyCanFall = true;
                        CandiesLocations[candies[0], candies[1] + i] = null;
                    }
                    else
                    {
                        sayac++;
                    }
                }
            }
        }
    }
    private void SpawnNewCandies(List<List<int>> chosenCandies)
    {
        foreach (var candies in chosenCandies)
        {
            for (int i = 0; i < Height - candies[1]; i++)// secilen sekerin ustundeki tum sekerlere tek tek erisim
            {
                GameObject upperCandy = CandiesLocations[candies[0], candies[1] + i];//bir ust sekere erisim. !Once kendi yerine bakiyor. 
                if (upperCandy == null)
                {
                    _candySpawner.SpawnRandomCandy(candies[0], candies[1] + i);
                    break;
                }
            }
        }
    }
    public void CheckForCombos()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Candy currentCandy = CandiesLocations[x, y].GetComponent<Candy>();

                if (currentCandy.IsChecked == false)
                {
                    currentCandy.CallForCheck();
                    //ilgili sekere ait eslesen sekerlerin listesi olustu
                    if (Candy._chosenCandies.Count >= 7)//bomb
                        foreach (var candies in Candy._chosenCandies)
                        {
                            CandiesLocations[candies[0], candies[1]].GetComponent<SpriteRenderer>().color = Color.red;
                        }
                    else if (Candy._chosenCandies.Count >= 5)//rocket
                        foreach (var candies in Candy._chosenCandies)
                        {
                            CandiesLocations[candies[0], candies[1]].GetComponent<SpriteRenderer>().color = Color.blue;
                        }
                    else
                    {
                        foreach (var candies in Candy._chosenCandies)
                        {
                            CandiesLocations[candies[0], candies[1]].GetComponent<Candy>().IsChecked = false;
                            CandiesLocations[candies[0], candies[1]].GetComponent<SpriteRenderer>().color = Color.white;

                        }
                    }
                }
            }
        }
        IsCheckCleaner();
        StartCoroutine(ClickCoolDown(.4f));
    }
    public void IsCheckCleaner()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                CandiesLocations[x, y].GetComponent<Candy>().IsChecked = false;
            }
        }
    }
    public IEnumerator ClickCoolDown(float time)
    {
        CanClick = false;
        yield return new WaitForSeconds(time);
        CanClick = true;
    }
}
