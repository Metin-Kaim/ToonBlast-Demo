using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Candy : MonoBehaviour
{
    int _x, _y;

    public bool isComboCheck;

    public static List<List<int>> _chosenCandies = new();
    public bool CandyCanFall { get; set; }
    public bool IsChecked { get; set; }
    public bool IsSelected { get; set; }


    private void Update()
    {
        if (CandyCanFall)
        {
            FallForCandy(_x, _y);
        }
    }
    private void OnMouseDown()
    {
        if (!GameManager.Instance.CanClick)
            return;

        GameManager.Instance.CanClick = false;

        CallForCheck(); //ilgili sekerle ayný yapida olan sekerler belirlenip listeye alindi. (_chosenCandies)

        if (_chosenCandies.Count > 1) // candies will explode
        {
            GameManager.Instance.CandiesDestroyNFall(_chosenCandies);
        }
        else // candies will not explode
        {
            GameManager.Instance.CanClick = true;
            IsChecked = false;
        }
    }

    public void InstantiateCandy(int x, int y)
    {
        _x = x;
        _y = y;
        GameManager.Instance.CandiesLocations[x, y] = gameObject;
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
        if (y <= GameManager.Instance.Height - 1 && y >= 0)//check for bounds
        {
            if (x <= GameManager.Instance.Width - 1 && x >= 0)//check for bounds
            {
                GameObject otherCandy = GameManager.Instance.CandiesLocations[x, y];
                if (otherCandy != null)
                {
                    if (otherCandy.GetComponent<Candy>().IsChecked != true)
                    {
                        if (otherCandy.CompareTag(gameObject.tag))
                        {
                            otherCandy.GetComponent<Candy>().IsChecked = true;//bir sonraki sekerin degerleri degistiriliyor.

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
        CheckForMatches(x, y + 1, chosenCandies);
        CheckForMatches(x, y - 1, chosenCandies);
        CheckForMatches(x + 1, y, chosenCandies);
        CheckForMatches(x - 1, y, chosenCandies);
    }

    public void CallForCheck() // Herhangi bir sekilde sekerleri kontrol etmek ve ayni turde olan sekerler uzerinde islem yapmak icin gerekli adimlari bulunduran method
    {
        _chosenCandies.Clear();//Listeyi temizle

        IsChecked = true;//kontrol edilen sekeri isaretle

        _chosenCandies.Add(new List<int> { _x, _y }); // secilen ilk sekerin koordinatlarýný al

        CheckEveryDirections(_x, _y, _chosenCandies);
    }
}
