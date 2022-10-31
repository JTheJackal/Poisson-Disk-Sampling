using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSampling : MonoBehaviour
{

    [SerializeField] private float radius = 1;
    [SerializeField] private int permittedAttempts = 30;
    [SerializeField] private float displayRadius  = 1;
    [SerializeField] private GameObject[] worldObjects;
    [SerializeField] private GameObject groundPlane;

    private Vector2 regionSize;
    private float offsetX;
    private float offsetY;
    private float offsetZ;

    List<Vector2> points;

    private void OnValidate() {

        offsetX     = (groundPlane.transform.localScale.x)/2;
        offsetZ     = (groundPlane.transform.localScale.z)/2;
        offsetY     = (groundPlane.transform.localScale.y)/2;
        regionSize  = new Vector2(groundPlane.transform.localScale.x, groundPlane.transform.localScale.z);

        points  = PoissonDiskSampling.GeneratePoints(radius, regionSize, permittedAttempts);
    }

    private void Start() {

        // Use points to place objects
        foreach(Vector2 point in points){

            Instantiate(worldObjects[Random.Range(0, worldObjects.Length)], new Vector3(point.x - offsetX, groundPlane.transform.position.y + offsetY, point.y - offsetZ), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        }
    }
    
    private void OnDrawGizmos() {
        
        Gizmos.DrawWireCube(new Vector3(groundPlane.transform.position.x, groundPlane.transform.position.y + offsetY, groundPlane.transform.position.z), new Vector3(groundPlane.transform.localScale.x, 0, groundPlane.transform.localScale.z));

        if(points != null){

            foreach(Vector2 point in points){

                Gizmos.DrawSphere(new Vector3(point.x - offsetX, groundPlane.transform.position.y + offsetY, point.y - offsetZ), displayRadius);
            }
        }
    }
    
}
