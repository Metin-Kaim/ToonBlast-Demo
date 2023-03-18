using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Candy : MonoBehaviour
{
    int _x, _y;

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

    private void OnMouseDown()
    {
        //Debug.Log($"Gameobject: {gameObject.name}\ncoordinates: {_x}-{_y}");
        //--------------------------------------------TODO-----------------------------------------------
        // TODO: seç ve patlat... !!! 

        CheckForMatches(_x, _y + 1);
        CheckForMatches(_x, _y - 1);
        CheckForMatches(_x + 1, _y);
        CheckForMatches(_x - 1, _y);

        Destroy(gameObject);
        _candySpawner.CandiesLocations[_x, _y] = null;

    }

    private void CheckForMatches(int x, int y)
    {
        if (y <= _candySpawner.Height - 1 && y >= 0)
            if (x <= _candySpawner.Width - 1 && x >= 0)
            {
                Destroy(_candySpawner.CandiesLocations[x, y]);
                _candySpawner.CandiesLocations[x, y] = null;
            }
    }
}
