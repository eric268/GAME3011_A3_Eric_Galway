using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    HashSet<Vector2> set;
    // Start is called before the first frame update
    void Start()
    {
        set = new HashSet<Vector2>();
        set.Add(new Vector2(-1, -1));
        set.Add(new Vector2(0, 0));
        set.Add(new Vector2(2, 2));

        if (set.Contains(new Vector2(0, 0)))
        {
            print("found");
        }
        else
            print("Not Found");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
