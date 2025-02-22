using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class MapSpwnerScript : MonoBehaviour
{
    [SerializeField]public List<MapClass> maps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("SpawnMap")]
    public void SpawnMap(){
        MapClass map = maps[UnityEngine.Random.Range(0, maps.Count)];
        float up_pos = Distributed_random.range(map.up_spn_rng_down,map.up_spn_rng_up,2);
        float hor_pos = Distributed_random.range(map.left_bound_pos,map.right_bound_pos,2);
        transform.position += Vector3.up * up_pos + Vector3.right * hor_pos;
        float ang = map.avalible_angles[UnityEngine.Random.Range(0,map.avalible_angles.Count)];
        GameObject instantiatedMap = Instantiate(map.map,transform.position,quaternion.RotateZ(ang));
        //instantiatedMap.

    }
}
