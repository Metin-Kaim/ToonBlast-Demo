using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    [SerializeField] GameObject[,] _candiesLocations;
    [SerializeField] GameObject[] _candies;
    [SerializeField] int _height, _width;
    [SerializeField] Transform _candiesParent;

    public GameObject[,] CandiesLocations => _candiesLocations;
    public int Height => _height;
    public int Width => _width;
    public bool IsSpawnDone { get; private set; }

    private void Start()
    {
        _candiesLocations = new GameObject[_width, _height];

        StartCoroutine(SpawnerTimer());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            Destroy(CandiesLocations[0, 2]);
        }
    }

    public void SpawnRandomCandy(int x, int y)
    {
        GameObject newCandy = Instantiate(_candies[Random.Range(0, _candies.Length)], new Vector2(x, y + 10), Quaternion.identity, _candiesParent);

        Candy newCandyScript = newCandy.GetComponent<Candy>();
        newCandyScript.InstantiateCandy(x, y); //from Candy script

        newCandyScript.CandyCanFall = true;

        //_candiesLocations[x, y] = newCandy;
    }

    IEnumerator SpawnerTimer()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                yield return new WaitForSeconds(.05f);
                SpawnRandomCandy(x, y);
            }
        }
        yield return new WaitForSeconds(2f);
        IsSpawnDone = true;
    }

    // TODO: ReInitiate
    // TODO: rocket bomb and combos
}
