using UnityEngine;

public class PullDown : MonoBehaviour {

    public Rigidbody cube;

    public float downWardForce = 10f;
    
    void FixedUpdate()
    {
        //cube.velocity = new Vector3(0, -20, 0);
        cube.AddForce(0, downWardForce * Time.deltaTime, 0);
        
    }

}
