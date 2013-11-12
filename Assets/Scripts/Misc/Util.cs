using UnityEngine;
using System.Collections;

public static class Util {
	
	/// <summary>
	/// Returns true if the testValue is between minValue and maxValue inclusive.
	/// </summary>
    public static bool InRange<T> (this T testValue, T minValue, T maxValue) where T : System.IComparable<T> {
        return testValue.CompareTo(minValue) >= 0 && testValue.CompareTo(maxValue) <= 0;
    }
	
	public static Vector2 zeroVector2 = new Vector2 (0.001f, 0.001f);
	public static Vector3 zeroVector3 = new Vector3 (0.001f, 0.001f, 0.001f);
	
	public static Color transparent = new Color (0, 0, 0, 0);
	
	public static Vector2 ScreenToWorld (Vector2 screenPosition, Camera camera) {
		return camera.ScreenToWorldPoint (screenPosition);
	}

	/// <summary>
	/// Selects an object, if it has been in this position.
	/// </summary>
	/// <returns>
	/// The object.
	/// </returns>
	/// <param name='screenPos'>
	/// Screen position.
	/// </param>
	public static GameObject SelectObject (Vector2 screenPos, Camera camera) {
		
		Ray ray = camera.ScreenPointToRay( screenPos );
        RaycastHit hit;

        if( Physics.Raycast( ray, out hit ) )
            return hit.collider.gameObject;

        return null;
    }
	
	public static float Interpolate (float xVar, float xMin, float xMax, float yMin, float yMax){
		return ( (xVar - xMin) / (xMax - xMin) ) * (yMax - yMin) + yMin;	
	}
	
	public static float ValueOf (float percent, float number) {
		return ( (percent * number) / 100);
	}
	
	public static float PercentOf (float value, float maxValue) {
		return ( (value / maxValue) * 100 );	
	}
	
	public static float GetX (this GameObject go ) {
		return go.transform.position.x;
	}
	
	public static float GetY ( this GameObject go ) {
		return go.transform.position.y;	
	}
	
	public static float GetZ ( this GameObject go ) {
		return go.transform.position.z;	
	}
	
	public static void SetXYZ ( this GameObject go, float x, float y, float z ) {
		go.transform.position = new Vector3 ( x, y, z );
	}
	
	public static void SetXY ( this GameObject go, float x, float y ) {
		go.SetXYZ ( x, y, go.GetZ() );
	}
	
	public static void SetXZ ( this GameObject go, float x, float z ) {
		go.SetXYZ ( x, go.GetY (), z );	
	}
	
	public static void SetYZ ( this GameObject go, float y, float z ) {
		go.SetXYZ ( go.GetX(), y, z );	
	}
	
	public static void SetX ( this GameObject go, float x ) {
		go.SetXYZ ( x, go.GetY(), go.GetZ() );	
	}
	
	public static void SetY ( this GameObject go, float y ) {
		go.SetXYZ ( go.GetX(), y, go.GetZ() );	
	}
	
	public static void SetZ ( this GameObject go, float z ) {
		go.SetXYZ ( go.GetX(), go.GetY(), z);	
	}
	
	// Local Positions
	public static float GetLocalX (this GameObject go ) {
		return go.transform.localPosition.x;
	}
	
	public static float GetLocalY ( this GameObject go ) {
		return go.transform.localPosition.y;	
	}
	
	public static float GetLocalZ ( this GameObject go ) {
		return go.transform.localPosition.z;	
	}
	
	public static void SetLocalXYZ ( this GameObject go, float x, float y, float z ) {
		go.transform.localPosition = new Vector3 ( x, y, z );
	}
	
	public static void SetLocalXY ( this GameObject go, float x, float y ) {
		go.SetLocalXYZ ( x, y, go.GetLocalZ() );
	}
	
	public static void SetLocalXZ ( this GameObject go, float x, float z ) {
		go.SetLocalXYZ ( x, go.GetLocalY (), z );	
	}
	
	public static void SetLocalYZ ( this GameObject go, float y, float z ) {
		go.SetLocalXYZ ( go.GetLocalX(), y, z );	
	}
	
	public static void SetLocalX ( this GameObject go, float x ) {
		go.SetLocalXYZ ( x, go.GetLocalY(), go.GetLocalZ() );	
	}
	
	public static void SetLocalY ( this GameObject go, float y ) {
		go.SetLocalXYZ ( go.GetLocalX(), y, go.GetLocalZ() );	
	}
	
	public static void SetLocalZ ( this GameObject go, float z ) {
		go.SetLocalXYZ ( go.GetLocalX(), go.GetLocalY(), z);	
	}
	
	
	public static void DestroyChildren ( GameObject gameobject ) {
		Transform[] transforms = gameobject.GetComponentsInChildren<Transform>();
		
		for ( int i = 0; i < transforms.Length; i++) {
			if ( transforms[i] != gameobject.transform )
				GameObject.Destroy (transforms[i].gameObject);
		}
	}
	
	public static void InstantiateInPlace ( this Object go ) {
		
	}
}
