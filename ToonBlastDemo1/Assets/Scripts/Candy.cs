using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Candy : MonoBehaviour
{
    int _x, _y;
    bool isChecked = false;

    public bool CandyCanFall { get; set; }


    [SerializeField] CandySpawner _candySpawner;

    private void Awake()
    {
        _candySpawner = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CandySpawner>();
    }

    private void Update()
    {
        if (CandyCanFall)
        {
            FallForCandy();
        }
    }
    private void OnMouseDown()
    {
        if (!_candySpawner.IsSpawnDone)
            return;
        
        isChecked = true;

        CheckEveryDirections(_x, _y);

        Destroy(gameObject);
        _candySpawner.CandiesLocations[_x, _y] = null;
    }

    public void InstantiateCandy(int x, int y)
    {
        _x = x;
        _y = y;
    }

    private void FallForCandy()
    {
        if (Vector2.Distance(transform.position, new Vector2(_x, _y)) >= .005f)
            transform.position = Vector2.Lerp(transform.position, new Vector2(_x, _y), 3 * Time.deltaTime);
        else
        {
            transform.position = new Vector2(_x, _y);
            CandyCanFall = false;
        }
    }

    private void CheckForMatches(int x, int y)
    {
        if (y <= _candySpawner.Height - 1 && y >= 0)
        {
            if (x <= _candySpawner.Width - 1 && x >= 0)
            {
                GameObject otherCandy = _candySpawner.CandiesLocations[x, y];
                if (otherCandy != null)
                {
                    if (otherCandy.GetComponent<Candy>().isChecked != true)
                    {
                        if (otherCandy.CompareTag(gameObject.tag))
                        {
                            otherCandy.GetComponent<Candy>().isChecked = true;//bir sonraki þekerin deðerleri deðiþtiriliyor.
                            otherCandy.GetComponent<Candy>().CheckEveryDirections(x, y);
                            Destroy(otherCandy);
                            _candySpawner.CandiesLocations[x, y] = null;
                        }
                    }
                }
            }
        }
    }
    private void CheckEveryDirections(int x, int y)
    {
        CheckForMatches(x, y + 1);//1,2 
        CheckForMatches(x, y - 1);//1,0 // 1,1
        CheckForMatches(x + 1, y);//2,1
        CheckForMatches(x - 1, y);//0,1
    }
}
