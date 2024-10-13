using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet2Script : MonoBehaviour
{
    private float timed = 0;
    public Vector2 vel;
    public LayerMask groundLayer;
    public GameObject bullet_hole;
    private Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timed += Time.deltaTime;
        if(timed > 6){
            Destroy(gameObject);
        }
        vel = myRigidbody.velocity;
        RaycastHit2D hit = collide_check();
        if(hit){
            Vector2 a = hit.point;
            Instantiate(bullet_hole,a,quaternion.RotateZ(0));
            Destroy(gameObject);
        }
    }
    private RaycastHit2D collide_check(){
        return Physics2D.Raycast(transform.position,vel,vel.magnitude*Time.deltaTime*3,groundLayer);
    }
    private void OnDrawGizmos(){
        Gizmos.DrawRay(transform.position,vel*Time.deltaTime*3);
    }
}
