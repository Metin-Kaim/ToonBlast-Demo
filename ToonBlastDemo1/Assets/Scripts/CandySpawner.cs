using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _candies;

    [SerializeField] Transform _candiesParent;

    private void Start()
    {
        StartCoroutine(SpawnerTimer());
    }

    public void SpawnRandomCandy(int x, int y)
    {
        GameObject newCandy = Instantiate(_candies[Random.Range(0, _candies.Length)], new Vector2(x, y + 10), Quaternion.identity, _candiesParent);

        Candy newCandyScript = newCandy.GetComponent<Candy>();
        newCandyScript.InstantiateCandy(x, y); //from Candy script

        newCandyScript.CandyCanFall = true;
    }

    IEnumerator SpawnerTimer()
    {
        for (int x = 0; x < GameManager.Instance.Width; x++)
        {
            for (int y = 0; y < GameManager.Instance.Height; y++)
            {
                yield return new WaitForSeconds(.05f);
                SpawnRandomCandy(x, y);
            }
        }

        GameManager.Instance.CheckForCombos();

        StartCoroutine(GameManager.Instance.ClickCoolDown(2.5f));
    }
}
