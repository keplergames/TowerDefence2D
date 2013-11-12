using UnityEditor;
using UnityEngine;
class ParametricPrimitiveMenu : MonoBehaviour
{
    [MenuItem ("GameObject/Create Primitive/Cube")]
    static void CreateCube ()
	{
		GameObject go = new GameObject("ParametricCube");
		go.AddComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		go.AddComponent<ParametricCube>();
    }
	
	[MenuItem ("GameObject/Create Primitive/Cylinder")]
    static void CreateCylinder ()
	{
        GameObject go = new GameObject("ParametricCylinder");
		go.AddComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		go.AddComponent<ParametricCylinder>();
    }
	
	[MenuItem ("GameObject/Create Primitive/Plane")]
    static void CreatePlane ()
	{
        GameObject go = new GameObject("ParametricPlane");
		go.AddComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		go.AddComponent<ParametricPlane>();
    }
	
	[MenuItem ("GameObject/Create Primitive/Sphere")]
    static void CreateSphere ()
	{
        GameObject go = new GameObject("ParametricSphere");
		go.AddComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		go.AddComponent<ParametricSphere>();
    }
}
