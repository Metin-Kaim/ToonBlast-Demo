using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    [SerializeField] GameObject[,] _candiesLocations;
    [SerializeField] GameObject[] _candies;
    [SerializeField] int _height, _width;

    private void Start()
    {
        _candiesLocations = new GameObject[_height, _width];

        CandiesSpawner();
    }

    private void CandiesSpawner()
    {
        StartCoroutine(SpawnerTimer());

    }

    private void SpawnRandomCandy(int x, int y)
    {
        GameObject newCandy = Instantiate(_candies[Random.Range(0, _candies.Length)], new Vector2(y, x + 10), Quaternion.identity);

        Candy newCandyScript = newCandy.GetComponent<Candy>();
        newCandyScript.InstantiateCandy(x, y);

        newCandyScript.CandyCanFall = true;

        _candiesLocations[x, y] = newCandy;
    }

    IEnumerator SpawnerTimer()
    {
        
        for (int x = 0; x < _height; x++)
        {
            for (int y = 0; y < _width; y++)
            {
                yield return new WaitForSeconds(.1f);
                SpawnRandomCandy(x, y);
            }
        }
    }
}
