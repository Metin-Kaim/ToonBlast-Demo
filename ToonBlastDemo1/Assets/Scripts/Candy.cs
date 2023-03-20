using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Candy : MonoBehaviour
{
    int _x, _y;
    bool _isChecked = false;

    static List<List<int>> _chosenCandies = new();

    public bool CandyCanFall { get; set; }
    public bool IsChecked => _isChecked;
    public bool IsSelected { get; set; }


    [SerializeField] CandySpawner _candySpawner;

    private void Awake()
    {
        _candySpawner = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CandySpawner>();
    }

    private void Update()
    {
        if (CandyCanFall)
        {
            FallForCandy(_x, _y);
        }
    }
    private void OnMouseDown()
    {
        if (!_candySpawner.IsSpawnDone)
            return;

        _chosenCandies.Clear();

        _isChecked = true;//dikkat et

        _chosenCandies.Add(new List<int> { _x, _y }); // secilen ilk sekerin koordinatlarý alindi.

        CheckEveryDirections(_x, _y, _chosenCandies);

        //ilgili sekerle ayný yapida olan sekerler belirlenip listeye alindi. (_chosenCandies)

        if (_chosenCandies.Count > 1) // candies will explode
        {
            GameManager.Instance.CandiesDestroyNFall(_chosenCandies);
        }
        else // candies will not explode
        {
            _isChecked = false;
        }
    }

    public void InstantiateCandy(int x, int y)
    {
        _x = x;
        _y = y;
        _candySpawner.CandiesLocations[x, y] = gameObject;
    }

    private void FallForCandy(int x, int y)
    {
        if (Vector2.Distance(transform.position, new Vector2(x, y)) >= .005f)
            transform.position = Vector2.Lerp(transform.position, new Vector2(x, y), 3 * Time.deltaTime);
        else
        {
            transform.position = new Vector2(x, y);
            CandyCanFall = false;
        }
    }

    private void CheckForMatches(int x, int y, List<List<int>> chosenCandies)
    {
        if (y <= _candySpawner.Height - 1 && y >= 0)
        {
            if (x <= _candySpawner.Width - 1 && x >= 0)
            {
                GameObject otherCandy = _candySpawner.CandiesLocations[x, y];
                if (otherCandy != null)
                {
                    if (otherCandy.GetComponent<Candy>()._isChecked != true)
                    {
                        if (otherCandy.CompareTag(gameObject.tag))
                        {
                            otherCandy.GetComponent<Candy>()._isChecked = true;//bir sonraki sekerin degerleri degistiriliyor.

                            chosenCandies.Add(new List<int> { x, y }); // eslesen her sekeri listeye ekleme.

                            otherCandy.GetComponent<Candy>().CheckEveryDirections(x, y, chosenCandies);
                        }
                    }
                }
            }
        }
    }
    private void CheckEveryDirections(int x, int y, List<List<int>> chosenCandies)
    {
        CheckForMatches(x, y + 1, chosenCandies);//1,2 
        CheckForMatches(x, y - 1, chosenCandies);//1,0 // 1,1
        CheckForMatches(x + 1, y, chosenCandies);//2,1
        CheckForMatches(x - 1, y, chosenCandies);//0,1
    }
}
