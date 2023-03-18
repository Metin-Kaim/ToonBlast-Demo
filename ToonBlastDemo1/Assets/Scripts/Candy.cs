using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Candy : MonoBehaviour
{
    int _x, _y;

    public bool CandyCanFall { get; set; }

    public void InstantiateCandy(int x, int y)
    {
        _x = x;
        _y = y;
        StartCoroutine(StartFalling());
    }

    private void Update()
    {
        if (CandyCanFall)
        {
            FallForCandy();
        }
    }

    private void FallForCandy()
    {
        if (Vector2.Distance(transform.position, new Vector2(_y, _x)) >= .005f)
            transform.position = Vector2.Lerp(transform.position, new Vector2(_y, _x), 3 * Time.deltaTime);
        else
        {
            transform.position = new Vector2(_y, _x);
            CandyCanFall = false;
        }
    }

    IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(1);
        FallForCandy();
    }

    private void OnMouseDown()
    {
        //Debug.Log($"Gameobject name is {gameObject.name} and its coordinates are {_y}-{_x}");
        //--------------------------------------------TODO-----------------------------------------------
        // TODO: seç ve patlat... !!! 
    }
}
