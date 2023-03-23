using System.Collections.Generic;
using UnityEngine;

public class Bomb : AbsEntity
{
    public override void CallForCheck()
    {
        _chosenCandies.Clear();//Listeyi temizle

        _chosenCandies.Add(new List<int> { _x, _y });

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (_y + j <= GameManager.Instance.Height - 1 && _y + j >= 0)//check for bounds
                {
                    if (_x + i <= GameManager.Instance.Width - 1 && _x + i >= 0)//check for bounds
                    {
                        if (i == 0 && j == 0)
                            continue;

                        _chosenCandies.Add(new List<int> { _x + i, _y + j });
                    }
                }
            }
        }
    }
}
