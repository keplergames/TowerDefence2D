using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
[System.Serializable]
[AddComponentMenu("Primitives/Cylinder")]
public class ParametricCylinder : ParametricPrimitive
{
	public int _subdivisionsTop = 1;
	public float _height = 1.0f;
	public float _radius = 1.0f;
	
	protected int subdivisionsTop = 1;
	protected float height = 1.0f;
	protected float radius = 1.0f;
	
	
	protected float demiHeight;
	
	protected void Update()
	{
		if( subdivisionsHeight != _subdivisionsHeight
		   || subdivisionsWidth != _subdivisionsWidth
		   || subdivisionsTop != _subdivisionsTop
		   || align != _align
		   || invert != _invert
		   || invertNormal != _invertNormal
		   || radius != _radius
           || height != _height  )
		{
			
			subdivisionsHeight = _subdivisionsHeight;
		   	subdivisionsWidth = _subdivisionsWidth;
			subdivisionsTop = _subdivisionsTop;
		   	align = _align;
		   	invert = _invert;
		   	invertNormal = _invertNormal;
			radius = _radius;
			height = _height;
			
			ShowMesh();
		}
	}
	
	override public void Reset()
	{
		base.Reset();
		
		_subdivisionsTop = 1;
		_height = 1.0f;
		_radius = 1.0f;
	}
	
	override public void ShowMesh()
	{
		// Input check
		if( subdivisionsWidth < 3 )
			subdivisionsWidth = 3;
		if( subdivisionsHeight < 1 )
			subdivisionsHeight = 1;
		if( subdivisionsTop < 1 )
			subdivisionsTop = 1;
		if( height < 0.0f )
			height = 0.0f;
		if (radius < 0.0f )
			radius = 0.0f;
		
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();
		newNormals.Clear();
		
		mesh.Clear();
		
		demiHeight = height/2.0f;
		
		float tileHeight = height / (float)subdivisionsHeight;
		float tileWidth = 360.0f / (float)subdivisionsWidth;
		float tileTop = radius / (float)subdivisionsTop;
		
		int nbVerticesTop = 1 + (subdivisionsTop)*( subdivisionsWidth + 1);
		int nbVerticesCylinder = (subdivisionsHeight + 1)*( subdivisionsWidth + 1);
		
		
		/**
		 * Vertex + Normals + UV
		 **/
		
		CreateTop(false, tileTop, tileWidth);
		CreateCylinder(tileHeight, tileWidth);
		CreateTop(true, tileTop, tileWidth);
		
		
		
		/**
		 * Triangles
		 **/
		int a;
		int b;
		int c;
		
		int tempFirstIdInCurrentLongitude;
		int tempFirstIdInNextLongitude;
		int tempNextIdInCurrentLatitude;
		
		// Bottom
		for( int j = 0; j < subdivisionsTop; ++j )
		{
			for( int i = 0; i < subdivisionsWidth; ++i )
			{
				tempFirstIdInCurrentLongitude	= 1 + ( j - 1) 	* (subdivisionsWidth + 1);
				tempFirstIdInNextLongitude		= 1 +   j * (subdivisionsWidth + 1);
				tempNextIdInCurrentLatitude		= (i + 1) % (subdivisionsWidth + 1);
				
				if( j == 0 )
					a = 0;
				else
					a = tempFirstIdInCurrentLongitude + i;
				

				b = tempFirstIdInNextLongitude + i;
				c = tempFirstIdInNextLongitude + tempNextIdInCurrentLatitude;
				
				newTriangles.Add( a );
				if( !invertNormal )
				{	
					newTriangles.Add( b );
					newTriangles.Add( c );
				}
				else
				{
					newTriangles.Add( c );
					newTriangles.Add( b );
				}
	
				if( j != 0 )
				{
					b = tempFirstIdInNextLongitude    + tempNextIdInCurrentLatitude;
					c = tempFirstIdInCurrentLongitude + tempNextIdInCurrentLatitude;
					
					newTriangles.Add( a );
					if( !invertNormal )
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
		
		// Cylinder
		for( int j = 0; j < subdivisionsHeight; ++j )
		{
			for( int i = 0; i < subdivisionsWidth; ++i )
			{
				tempFirstIdInCurrentLongitude	=  j 	  * (subdivisionsWidth + 1);
				tempFirstIdInNextLongitude		= (j + 1) * (subdivisionsWidth + 1);
				tempNextIdInCurrentLatitude		= (i + 1) % (subdivisionsWidth + 1);
				
				a = tempFirstIdInCurrentLongitude + i;
				

				b = tempFirstIdInNextLongitude + i;
				c = tempFirstIdInNextLongitude + tempNextIdInCurrentLatitude;
				
				newTriangles.Add( a + nbVerticesTop );
				if( !invertNormal )
				{
					newTriangles.Add( b + nbVerticesTop );
					newTriangles.Add( c + nbVerticesTop );
				}
				else
				{
					newTriangles.Add( c + nbVerticesTop );
					newTriangles.Add( b + nbVerticesTop );
				}
				
				b = tempFirstIdInNextLongitude    + tempNextIdInCurrentLatitude;
				c = tempFirstIdInCurrentLongitude + tempNextIdInCurrentLatitude;
				
				newTriangles.Add( a + nbVerticesTop );
				if( !invertNormal )
				{
					newTriangles.Add( b + nbVerticesTop );
					newTriangles.Add( c + nbVerticesTop );
				}
				else
				{
					newTriangles.Add( c + nbVerticesTop );
					newTriangles.Add( b + nbVerticesTop );
				}
			}
		}
		
		// Top
		for( int j = 0; j < subdivisionsTop; ++j )
		{
			for( int i = 0; i < subdivisionsWidth; ++i )
			{
				tempFirstIdInCurrentLongitude	= 1 + ( j - 1) 	* (subdivisionsWidth + 1);
				tempFirstIdInNextLongitude		= 1 +   j * (subdivisionsWidth + 1);
				tempNextIdInCurrentLatitude		= (i + 1) % (subdivisionsWidth + 1);
				
				if( j == 0 )
					a = 0;
				else
					a = tempFirstIdInCurrentLongitude + i;
				

				b = tempFirstIdInNextLongitude + i;
				c = tempFirstIdInNextLongitude + tempNextIdInCurrentLatitude;
				
				newTriangles.Add( a + nbVerticesTop +nbVerticesCylinder );
				if( !invertNormal )
				{
					newTriangles.Add( c + nbVerticesTop +nbVerticesCylinder );
					newTriangles.Add( b + nbVerticesTop +nbVerticesCylinder );
				}
				else
				{
					newTriangles.Add( b + nbVerticesTop +nbVerticesCylinder );
					newTriangles.Add( c + nbVerticesTop +nbVerticesCylinder );
				}
	
				if( j != 0 )
				{
					b = tempFirstIdInNextLongitude    + tempNextIdInCurrentLatitude;
					c = tempFirstIdInCurrentLongitude + tempNextIdInCurrentLatitude;
					
					newTriangles.Add( a + nbVerticesTop +nbVerticesCylinder );
					if( !invertNormal )
					{
						newTriangles.Add( c + nbVerticesTop +nbVerticesCylinder );
						newTriangles.Add( b + nbVerticesTop +nbVerticesCylinder );
					}
					else
					{
						newTriangles.Add( b + nbVerticesTop +nbVerticesCylinder );
						newTriangles.Add( c + nbVerticesTop +nbVerticesCylinder );
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
	
	void CreateTop(bool up, float tileTop, float tileWidth)
	{
		Vector3 position;
		float m = 0.0f;
		
		switch( align )
		{
			default:
			case eAlign.alignY:
				position.x = 0.0f;
				position.y = (up)?demiHeight:-demiHeight;
				position.z = 0.0f;
			
				normal.x = 0.0f;
				normal.y = (up)?1.0f:-1.0f;
				normal.z = 0.0f;
				break;
			case eAlign.alignX:
				position.x = (up)?demiHeight:-demiHeight;
				position.y = 0.0f;
				position.z = 0.0f;
			
				normal.x = (up)?1.0f:-1.0f;
				normal.y = 0.0f;
				normal.z = 0.0f;
				break;
			case eAlign.alignZ:
				position.x = 0.0f;
				position.y = 0.0f;
				position.z = (up)?demiHeight:-demiHeight;
			
				normal.x = 0.0f;
				normal.y = 0.0f;
				normal.z = (up)?1.0f:-1.0f;
				break;
		}
		
		normal *= (invertNormal)?-1.0f:1.0f;
		newVertices.Add( position );
		newNormals.Add( normal );

		for( int aa = 1; aa <= subdivisionsTop; ++aa)
		{
			for( int bb = 0; bb < subdivisionsWidth; ++bb )
			{
				switch( align )
				{
					default:
					case eAlign.alignY:
						position.x = aa*tileTop*Mathf.Cos((float)m*Mathf.Deg2Rad);
						position.y = (up)?demiHeight:-demiHeight;
						position.z = aa*tileTop*Mathf.Sin((float)m*Mathf.Deg2Rad);
						break;
					case eAlign.alignX:
						position.z = aa*tileTop*Mathf.Cos((float)m*Mathf.Deg2Rad);
						position.x = (up)?demiHeight:-demiHeight;
						position.y = aa*tileTop*Mathf.Sin((float)m*Mathf.Deg2Rad);
						break;
					case eAlign.alignZ:
						position.y = aa*tileTop*Mathf.Cos((float)m*Mathf.Deg2Rad);
						position.z = (up)?demiHeight:-demiHeight;
						position.x = aa*tileTop*Mathf.Sin((float)m*Mathf.Deg2Rad);
						break;
				}
				
				newVertices.Add( position );
				newNormals.Add( normal );

				m += tileWidth;
			}
			
			m = 0.0f;
			
			// junction, hand patch to be certain it will collide with the first vertex of this longitude
			switch( align )
			{
				default:
				case eAlign.alignY:
					position.x = aa*tileTop*Mathf.Cos((float)m*Mathf.Deg2Rad);
					position.y = (up)?demiHeight:-demiHeight;
					position.z = aa*tileTop*Mathf.Sin((float)m*Mathf.Deg2Rad);
					break;
				case eAlign.alignX:
					position.z = aa*tileTop*Mathf.Cos((float)m*Mathf.Deg2Rad);
					position.x = (up)?demiHeight:-demiHeight;
					position.y = aa*tileTop*Mathf.Sin((float)m*Mathf.Deg2Rad);
					break;
				case eAlign.alignZ:
					position.y = aa*tileTop*Mathf.Cos((float)m*Mathf.Deg2Rad);
					position.z = (up)?demiHeight:-demiHeight;
					position.x = aa*tileTop*Mathf.Sin((float)m*Mathf.Deg2Rad);
					break;
			}
			
			newVertices.Add( position );
			newNormals.Add( normal );
		}
		
		
		
		/////////////// UV
		Vector2 tempUV;
		float tempAngle = 0.0f;
		
		float radiusTop = 1.0f/6.0f;
		float tileTopUV = 1.0f/3.0f;
		
		tempUV.x = 0.5f;
		tempUV.y = ((up && !invert) || (!up && invert))?5.0f/6.0f:1.0f/6.0f;
		newUV.Add( tempUV );
		for( int j = 1; j <= subdivisionsTop; ++j )
		{
			tempAngle = 0.0f;
			for( int i = 0; i <= subdivisionsWidth; ++i )
			{
				tempUV.x = - ((j*Mathf.Sin( tempAngle*Mathf.Deg2Rad ))/subdivisionsTop)/6.0f;
				tempUV.x *= (invert)?-1.0f:1.0f;
				tempUV.x += 0.5f;
				tempUV.y = ((up && !invert) || (!up && invert))?2*tileTopUV + radiusTop + ((j*Mathf.Cos( tempAngle*Mathf.Deg2Rad ))/subdivisionsTop)/6.0f:radiusTop - ((j*Mathf.Cos( tempAngle*Mathf.Deg2Rad ))/subdivisionsTop)/6.0f;
				newUV.Add( tempUV );
				
				tempAngle += tileWidth;
			}
		}
	}
	
	void CreateCylinder(float tileHeight, float tileWidth)
	{
		Vector3 position;
		float l = -demiHeight;
		float m = 0.0f;
		
		for( int aa = 0; aa <= subdivisionsHeight; ++aa )
		{
			for( int bb = 0; bb < subdivisionsWidth; ++bb )
			{
				switch( align )
				{
					default:
					case eAlign.alignY:
						position.x = radius*Mathf.Cos((float)m*Mathf.Deg2Rad);
						position.y = l;
						position.z = radius*Mathf.Sin((float)m*Mathf.Deg2Rad);
					
						normal = position;
						normal.y = 0.0f;
						break;
					case eAlign.alignX:
						position.z = radius*Mathf.Cos((float)m*Mathf.Deg2Rad);
						position.x = l;
						position.y = radius*Mathf.Sin((float)m*Mathf.Deg2Rad);
					
						normal = position;
						normal.x = 0.0f;
						break;
					case eAlign.alignZ:
						position.y = radius*Mathf.Cos((float)m*Mathf.Deg2Rad);
						position.z = l;
						position.x = radius*Mathf.Sin((float)m*Mathf.Deg2Rad);
					
						normal = position;
						normal.z = 0.0f;
						break;
				}

				newVertices.Add( position );
				
				normal = Vector3.Normalize( normal );
				normal *= (invertNormal)?-1.0f:1.0f;
				newNormals.Add( normal );
			
				m += tileWidth;
			}
			
			m = 0.0f;
			
			// junction, hand patch to be certain it will collide with the first vertex of this longitude
			switch( align )
			{
				default:
				case eAlign.alignY:
					position.x = radius*Mathf.Cos((float)m*Mathf.Deg2Rad);
					position.y = l;
					position.z = radius*Mathf.Sin((float)m*Mathf.Deg2Rad);
				
					normal = position;
					normal.y = 0.0f;
					break;
				case eAlign.alignX:
					position.z = radius*Mathf.Cos((float)m*Mathf.Deg2Rad);
					position.x = l;
					position.y = radius*Mathf.Sin((float)m*Mathf.Deg2Rad);
				
					normal = position;
					normal.x = 0.0f;
					break;
				case eAlign.alignZ:
					position.y = radius*Mathf.Cos((float)m*Mathf.Deg2Rad);
					position.z = l;
					position.x = radius*Mathf.Sin((float)m*Mathf.Deg2Rad);
				
					normal = position;
					normal.z = 0.0f;
					break;
			}
			newVertices.Add( position );
			normal = Vector3.Normalize( normal );
			normal *= (invertNormal)?-1.0f:1.0f;
			newNormals.Add( normal );
			
			l += tileHeight;
		}	
		
		
		
		///////////////////// UV
		Vector2 tempUV;
		float tileTopUV = 1.0f/3.0f;
		
		for( int j = 0; j <= subdivisionsHeight; ++j )
		{
			for( int i = 0; i <= subdivisionsWidth; ++i )
			{
				tempUV.x = i/(float)(subdivisionsWidth);
				tempUV.x *= (invert)?-1.0f:1.0f;
				tempUV.y = tileTopUV + (j/(float)subdivisionsHeight)/3.0f;
				newUV.Add( tempUV );
			}
		}
	}
	
	protected override string getName()
	{
		return "ParametricCylinder";
	}
}
