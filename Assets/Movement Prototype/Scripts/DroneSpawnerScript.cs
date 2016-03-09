using UnityEngine;
using System.Collections;

public class DroneSpawnerScript : MonoBehaviour {

    public GameObject drone;
    public int spawnRate;
    int timeSincePreviousSpawn = 0;
    
    public void FixedUpdate()
    {
        if (timeSincePreviousSpawn < spawnRate)
        {
            timeSincePreviousSpawn++;
        }
    }

    public GameObject SpawnDrone()
    {
        if (timeSincePreviousSpawn == spawnRate)
        {
            GameObject droneInstance = (GameObject) Instantiate(drone, transform.position, transform.rotation);
            timeSincePreviousSpawn = 0;
            return droneInstance;
        }
        return null;
    }
	
}
