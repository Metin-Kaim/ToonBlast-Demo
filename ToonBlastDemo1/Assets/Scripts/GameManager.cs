using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int _height, _width;
    [SerializeField] GameObject _bomb;

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
        StartCoroutine(Devasm(chosenCandies));
    }
    IEnumerator Devasm(List<List<int>> chosenCandies)
    {
        yield return new WaitForSeconds(.2f);
        FallUpperCandies(chosenCandies);
        SpawnNewCandies(chosenCandies);
        CheckForCombos();
    }
    private void DestroyAllChosenCandies(List<List<int>> chosenCandies)
    {
        bool isBomb = _candiesLocations[chosenCandies[0][0], chosenCandies[0][1]].CompareTag("Bomb") ? true : false;
        foreach (var candies in chosenCandies)
        {
            CandiesLocations[candies[0], candies[1]].GetComponent<Animator>().SetTrigger("isExplode");

            Destroy(CandiesLocations[candies[0], candies[1]],.3f);
            CandiesLocations[candies[0], candies[1]] = null;
        }

        if (chosenCandies.Count >= 7 && !isBomb)
        {
            GameObject bomb = Instantiate(_bomb, new Vector3(chosenCandies[0][0], chosenCandies[0][1], 0), Quaternion.identity, _candySpawner.CandiesParent);
            bomb.GetComponent<AbsEntity>().InstantiateCandy(chosenCandies[0][0], chosenCandies[0][1]);
            chosenCandies.Remove(chosenCandies[0]);
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
                        upperCandy.GetComponent<AbsEntity>().InstantiateCandy(candies[0], candies[1] + i - 1 - sayac);
                        upperCandy.GetComponent<AbsEntity>().CandyCanFall = true;
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
                AbsEntity currentCandy = CandiesLocations[x, y].GetComponent<AbsEntity>();

                if (currentCandy.IsChecked == false && !currentCandy.CompareTag("Bomb"))
                {
                    currentCandy.CallForCheck();
                    //ilgili sekere ait eslesen sekerlerin listesi olustu
                    if (AbsEntity._chosenCandies.Count >= 7)//bomb
                        foreach (var candies in AbsEntity._chosenCandies)
                        {
                            CandiesLocations[candies[0], candies[1]].GetComponent<SpriteRenderer>().color = Color.red;
                        }
                    else
                    {
                        foreach (var candies in AbsEntity._chosenCandies)
                        {
                            CandiesLocations[candies[0], candies[1]].GetComponent<AbsEntity>().IsChecked = false;
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
                CandiesLocations[x, y].GetComponent<AbsEntity>().IsChecked = false;
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
