using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
[System.Serializable]
[AddComponentMenu("Primitives/Sphere")]
public class ParametricSphere : ParametricPrimitive
{
	public float _radius = 1.0f;
	protected float radius = 1.0f;
	
	protected void Update()
	{
		if( subdivisionsHeight != _subdivisionsHeight
		   || subdivisionsWidth != _subdivisionsWidth
		   || align != _align
		   || invert != _invert
		   || invertNormal != _invertNormal
		   || radius != _radius )
		{
			
			subdivisionsHeight = _subdivisionsHeight;
		   	subdivisionsWidth = _subdivisionsWidth;
		   	align = _align;
		   	invert = _invert;
		   	invertNormal = _invertNormal;
			radius = _radius;
			
			ShowMesh();
		}
    }
	
	override public void Reset()
	{
		base.Reset();
		
		_radius = 1.0f;
	}

	override public void ShowMesh()
	{
		// Input check
		if( subdivisionsWidth < 3 )
			subdivisionsWidth = 3;
		if( subdivisionsHeight < 3 )
			subdivisionsHeight = 3;
		if( radius < 0.0f )
			radius = 0.0f;
		
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();
		newNormals.Clear();
		
		mesh.Clear();
		
		float tileHeight = 180.0f / (float)subdivisionsHeight;
		float tileWidth = 360.0f / (float)subdivisionsWidth;
		
		Vector3 position;
		float l = (!invert)?-90.0f:90.0f;
		float m = 0.0f;

		for( int aa = 0; aa <= subdivisionsHeight; ++aa )
		{
			for( int bb = 0; bb < subdivisionsWidth; ++bb )
			{
				switch( align )
				{
					default:
					case eAlign.alignY:
						position.x = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Cos(m*Mathf.Deg2Rad);
						position.y = radius*Mathf.Sin(l*Mathf.Deg2Rad);
						position.z = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Sin(m*Mathf.Deg2Rad);
						break;
					case eAlign.alignX:
						position.z = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Cos(m*Mathf.Deg2Rad);
						position.x = radius*Mathf.Sin(l*Mathf.Deg2Rad);
						position.y = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Sin(m*Mathf.Deg2Rad);
						break;
					case eAlign.alignZ:
						position.y = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Cos(m*Mathf.Deg2Rad);
						position.z = radius*Mathf.Sin(l*Mathf.Deg2Rad);
						position.x = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Sin(m*Mathf.Deg2Rad);
						break;
				}
				newVertices.Add( position );
				
				normal = Vector3.Normalize( position );
				normal *= (invertNormal)?-1.0f:1.0f;
				newNormals.Add( normal );
				
				m += tileWidth;
			}
			
			m = 0.0f;

			switch( align ) // junction, hand patch to be certain it will collide with the first vertex of this longitude
			{
				default:
				case eAlign.alignY:
					position.x = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Cos(m*Mathf.Deg2Rad);
					position.y = radius*Mathf.Sin(l*Mathf.Deg2Rad);
					position.z = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Sin(m*Mathf.Deg2Rad);
					break;
				case eAlign.alignX:
					position.z = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Cos(m*Mathf.Deg2Rad);
					position.x = radius*Mathf.Sin(l*Mathf.Deg2Rad);
					position.y = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Sin(m*Mathf.Deg2Rad);
					break;
				case eAlign.alignZ:
					position.y = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Cos(m*Mathf.Deg2Rad);
					position.z = radius*Mathf.Sin(l*Mathf.Deg2Rad);
					position.x = radius*Mathf.Cos(l*Mathf.Deg2Rad)*Mathf.Sin(m*Mathf.Deg2Rad);
					break;
			}
			newVertices.Add( position );

			normal = Vector3.Normalize( position );
			normal *= (invertNormal)?-1.0f:1.0f;
			newNormals.Add( normal );
			
			l += (!invert)?tileHeight:-tileHeight;
		}
		
		Vector2 tempUV;
		for( int j = 0; j <= subdivisionsHeight; ++j )
		{
			for( int i = 0; i <= subdivisionsWidth; ++i )
			{
				tempUV.x = i/(float)(subdivisionsWidth);
				tempUV.y = j/(float)(subdivisionsHeight);
				newUV.Add( tempUV );
			}
		}
		
		int a;
		int b;
		int c;
		
		int tempFirstIdInCurrentLongitude;
		int tempFirstIdInNextLongitude;
		int tempNextIdInCurrentLatitude;
		
		for( int j = 0; j < subdivisionsHeight; ++j )
		{
			for( int i = 0; i < subdivisionsWidth; ++i )
			{
				tempFirstIdInCurrentLongitude	=  j 	  * (subdivisionsWidth + 1);
				tempFirstIdInNextLongitude		= (j + 1) * (subdivisionsWidth + 1);
				tempNextIdInCurrentLatitude		= (i + 1) % (subdivisionsWidth + 1);
				
				a = tempFirstIdInCurrentLongitude + i;
				
				if( j != subdivisionsHeight - 1 )
				{
					b = tempFirstIdInNextLongitude + i;
					c = tempFirstIdInNextLongitude + tempNextIdInCurrentLatitude;
					
					newTriangles.Add( a );
					if( (!invert && !invertNormal) || (invert && invertNormal) )
					{
						newTriangles.Add( b );
						newTriangles.Add( c );
					}
					else
					{
						newTriangles.Add( c );
						newTriangles.Add( b );
					}
				}
	
				if( j != 0 )
				{
					b = tempFirstIdInNextLongitude    + tempNextIdInCurrentLatitude;
					c = tempFirstIdInCurrentLongitude + tempNextIdInCurrentLatitude;
					
					newTriangles.Add( a );
					if( (!invert && !invertNormal) || (invert && invertNormal) )
					{
						newTriangles.Add( b );
						newTriangles.Add( c );
					}
					else
					{
						newTriangles.Add( c );
						newTriangles.Add( b );
					}
				}
			}
		}
			
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
		mesh.normals = newNormals.ToArray();
		
		meshFilter.mesh = mesh;
	}
	
	protected override string getName()
	{
		return "ParametricSphere";
	}
	
}
