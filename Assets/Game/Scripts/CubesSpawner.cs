using UnityEngine;

public class CubesSpawner : MonoBehaviour
{
    public GameObject cube;
    void Start()
    {
        for (int i = 0; i < 300; i++)
        {
            GameObject newCube = Instantiate(cube, transform.position + new Vector3(Random.Range(-200f, 200f), 0, Random.Range(10f, 250f)), Quaternion.identity, transform );
            newCube.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        }
    }

}
