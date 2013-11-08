using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
[System.Serializable]
[AddComponentMenu("Primitives/Plane")]
public class ParametricPlane : ParametricPrimitive
{
	public float _height = 1.0f;
	public float _width = 1.0f;
	
	protected float height = 1.0f;
	protected float width = 1.0f;
	
	protected float demiHeight;
	protected float demiWidth;
	
	protected Vector3 topLeft;
	protected Vector3 topRight;
	protected Vector3 bottomLeft;
	protected Vector3 bottomRight;
	
	protected void Update()
	{
		if( subdivisionsHeight != _subdivisionsHeight
		   || subdivisionsWidth != _subdivisionsWidth
		   || align != _align
		   || invert != _invert
		   || invertNormal != _invertNormal
		   || width != _width
           || height != _height  )
		{
			
			subdivisionsHeight = _subdivisionsHeight;
		   	subdivisionsWidth = _subdivisionsWidth;
		   	align = _align;
		   	invert = _invert;
		   	invertNormal = _invertNormal;
			width = _width;
			height = _height;
			
			ShowMesh();
		}
	}
	
	override public void Reset()
	{
		base.Reset();
		
		_height = 1.0f;
		_width = 1.0f;
	}
	
	override public void ShowMesh()
	{
		// Input check
		if( subdivisionsWidth < 1 )
			subdivisionsWidth = 1;
		if( subdivisionsHeight < 1 )
			subdivisionsHeight = 1;
		if( height < 0.0f )
			height = 0.0f;
		if (width < 0.0f )
			width = 0.0f;
		
		demiWidth = width/2.0f;
		demiHeight = height/2.0f;
		
		switch( align )
		{
			default:
			case eAlign.alignY:
				topLeft = (!invert)?new Vector3( -demiWidth, 0.0f, demiHeight ):new Vector3( demiWidth, 0.0f, demiHeight );
				topRight = (!invert)?new Vector3( demiWidth, 0.0f, demiHeight ):new Vector3( -demiWidth, 0.0f, demiHeight );
				bottomLeft = (!invert)?new Vector3( -demiWidth, 0.0f, -demiHeight ):new Vector3( demiWidth, 0.0f, -demiHeight );
				bottomRight = (!invert)?new Vector3( demiWidth, 0.0f, -demiHeight ):new Vector3( -demiWidth, 0.0f, -demiHeight );
				break;
			case eAlign.alignX:
				topLeft = (!invert)?new Vector3( 0.0f, demiHeight, -demiWidth ):new Vector3( 0.0f, demiHeight, demiWidth );
				topRight = (!invert)?new Vector3( 0.0f, demiHeight, demiWidth ):new Vector3( 0.0f, demiHeight, -demiWidth );
				bottomLeft = (!invert)?new Vector3( 0.0f, -demiHeight, -demiWidth ):new Vector3( 0.0f, -demiHeight, demiWidth );
				bottomRight = (!invert)?new Vector3( 0.0f, -demiHeight, demiWidth ):new Vector3( 0.0f, -demiHeight, -demiWidth );
				break;
			case eAlign.alignZ:
				topLeft = (!invert)?new Vector3( demiWidth, demiHeight, 0.0f ):new Vector3( -demiWidth, demiHeight, 0.0f );
				topRight = (!invert)?new Vector3( -demiWidth, demiHeight, 0.0f ):new Vector3( demiWidth, demiHeight, 0.0f );
				bottomLeft = (!invert)?new Vector3( demiWidth, -demiHeight, 0.0f ):new Vector3( -demiWidth, -demiHeight, 0.0f );
				bottomRight = (!invert)?new Vector3( -demiWidth, -demiHeight, 0.0f ):new Vector3( demiWidth, -demiHeight, 0.0f );
				break;
		}
		
		
		normal = Vector3.Cross(Vector3.Normalize(topLeft - bottomLeft), Vector3.Normalize(bottomRight - bottomLeft));
		normal *= (invertNormal)?-1.0f:1.0f;
			
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();
		newNormals.Clear();
		
		mesh.Clear();
		
		float tileWidth = width / subdivisionsWidth;
		float tileHeight = height / subdivisionsHeight;
		Vector3 nBottomDirection = Vector3.Normalize(bottomLeft - topLeft);
		Vector3 nRightDirection = Vector3.Normalize(topRight - topLeft);
		
		for( int j = 0; j <= subdivisionsHeight; ++j )
		{
			for( int i = 0; i <= subdivisionsWidth; ++i )
			{
				newVertices.Add( topLeft + i*tileWidth*nRightDirection + j*tileHeight*nBottomDirection );
				
				newUV.Add( new Vector2( (i/(float)subdivisionsWidth), 1 - (j/(float)subdivisionsHeight) ) );
				
				newNormals.Add( normal );
			}
		}
		
		for( int j = 0; j < subdivisionsHeight; ++j )
		{
			for( int i = 0; i < subdivisionsWidth; ++i )
			{
				newTriangles.Add( i + (j + 1) * (subdivisionsWidth + 1) );
				if( !invertNormal )
				{
					newTriangles.Add( i + j * (subdivisionsWidth + 1) );
					newTriangles.Add( i + 1 + j * (subdivisionsWidth + 1) );
				}
				else
				{
					newTriangles.Add( i + 1 + j * (subdivisionsWidth + 1) );
					newTriangles.Add( i + j * (subdivisionsWidth + 1) );
				}
				
				newTriangles.Add( i + (j + 1) * (subdivisionsWidth + 1) );
				if( !invertNormal )
				{
					newTriangles.Add( i + 1 + j * (subdivisionsWidth + 1) );
					newTriangles.Add( i + 1 + (j + 1) * (subdivisionsWidth + 1) );
				}
				else
				{
					newTriangles.Add( i + 1 + (j + 1) * (subdivisionsWidth + 1) );
					newTriangles.Add( i + 1 + j * (subdivisionsWidth + 1) );
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
		return "ParametricPlane";
	}

}
