using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathDefinition : MonoBehaviour {

    public Transform[] Points;

    public IEnumerator<Transform> GetPathsEnumerator()
    {
        if (Points == null || Points.Length < 1)
        {
            yield break; // yield terminates straight away
        }
        var direction = 1;
        var index = 0; 
        while (true)
        {
            yield return Points[index];

            if (Points.Length ==1)
            {
                continue;
            }

            if(index <= 0)
            {
                direction = 1;
            } else if (index >= Points.Length - 1)
            {
                direction = -1;
            }

            index = index + direction;
        }

    }

    public void OnDrawGizmos()
    {
       

        if(Points == null || Points.Length < 2) // NB this was done because Points cant ever == null but it is possible so to get around this the OR operator is used with the LHS only being true if there is an instance of null therefore the RHS will never cause an error
        {
            return;
        }

        // stop unity from locking up when a path point is deleted - I think that because it always compiles in the background it tries to compile the path and it cant handle a null which is what happens if you delete a path point. so this code covers that 
        var points = Points.Where(t => t != null).ToList();
        if (points.Count < 2)
            return;
        //---

        for (var i = 1; i < points.Count; i++)
        {
            Gizmos.DrawLine(Points[i - 1].position, Points[i].position);
        }
    }
}
