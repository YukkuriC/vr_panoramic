using UnityEngine;
 
[RequireComponent(typeof(MeshFilter))]
public class OctahedronSphere : MonoBehaviour {
 
    [Range(0, 6)]
    public int subdivisions = 0;
    public float radius = 1f;
 
    private void Awake () {
        GetComponent<MeshFilter>().mesh = OctahedronSphereCreator.Create(subdivisions, radius);
    }
}