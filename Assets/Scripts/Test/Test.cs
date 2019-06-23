using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform parent;

    private bool isLOaded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isLOaded)
        {
            Loadl();
        }
           
    }

    private void Loadl()
    {
        var R = Resources.Load<GameObject>("Levels/Level_1");
        var o = Instantiate(R,parent);
        isLOaded = true;
        var tilemapWall = o.transform.Find("Wall").GetComponent<Tilemap>();
/*        tilemapWall.cellBounds.center
        Debug.Log("pos: " + o.transform.localPosition + " " + );*/
    }
    
    
}
