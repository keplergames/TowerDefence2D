using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
[System.Serializable]
[AddComponentMenu("Primitives/Cube")]
public class ParametricCube : ParametricPrimitive
{
	public int _subdivisionsDepth = 1;
	protected int subdivisionsDepth = 1;
	
	protected float width = 1.0f;
	protected float height = 1.0f;
	protected float depth = 1.0f;
	
	public float _width = 1.0f;
	public float _height = 1.0f;
	public float _depth = 1.0f;
	
	protected float demiWidth;
	protected float demiHeight;
	protected float demiDepth;
	
	
	protected void Update()
	{
		if( subdivisionsHeight != _subdivisionsHeight
		   || subdivisionsWidth != _subdivisionsWidth
		   || subdivisionsDepth != _subdivisionsDepth
		   || align != _align
		   || invert != _invert
		   || invertNormal != _invertNormal
		   || width != _width
          || height != _height
          || depth != _depth  )
		{
			
			subdivisionsHeight = _subdivisionsHeight;
		   	subdivisionsWidth = _subdivisionsWidth;
			subdivisionsDepth = _subdivisionsDepth;
		   	align = _align;
		   	invert = _invert;
		   	invertNormal = _invertNormal;
			width = _width;
			height = _height;
			depth = _depth;
			
			ShowMesh();
		}
    }
	
	override public void Reset()
	{
		base.Reset();
		
		_subdivisionsDepth = 1;
		_width = 1.0f;
		_height = 1.0f;
		_depth = 1.0f;
	}
	
	override public void ShowMesh()
	{
		// Input check
		if( subdivisionsWidth < 1 )
			subdivisionsWidth = 1;
		if( subdivisionsHeight < 1 )
			subdivisionsHeight = 1;
		if( subdivisionsDepth < 1 )
			subdivisionsDepth = 1;
		if( width < 0.0f )
			width = 0.0f;
		if (height < 0.0f )
			height = 0.0f;
		if( depth < 0.0f )
			depth = 0.0f;
		
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();
		newNormals.Clear();
		
		mesh.Clear();
		
		demiHeight = height/2.0f;
		demiWidth = width/2.0f;
		demiDepth = depth/2.0f;
		
		
		/**
	     *z	  a________d
		 *   /:       /|
		 * b/_:_____c/ |
		 * |  :		|  |
		 * |  e	- - |- h
	 	 * | /	Top	| /
		 * f________|/g
		 * 
		 * * * * * * * * x
		*
	   * y
	   
	    **/
		
		Vector3 a;
		Vector3 b;
		Vector3 c;
		Vector3 d;
		Vector3 e;
		Vector3 f;
		Vector3 g;
		Vector3 h;
		
		a = Vector3.zero;
		
		switch( align )
		{
			default:
			case eAlign.alignY:
				a.x -= demiWidth;
				a.y -= demiDepth;
				a.y *= (invert)?-1.0f:1.0f;
				a.z += demiHeight;
				a.z *= (invert)?-1.0f:1.0f;
			
				b = a;
				b.y = demiDepth;
				b.y *= (invert)?-1.0f:1.0f;
				
				c = b;
				c.x = demiWidth;
				
				d = c;
				d.y = -demiDepth;
				d.y *= (invert)?-1.0f:1.0f;
				
				e = a;
				e.z = -demiHeight;
				e.z *= (invert)?-1.0f:1.0f;
				
				f = b;
				f.z = -demiHeight;
				f.z *= (invert)?-1.0f:1.0f;
				
				g = c;
				g.z = -demiHeight;
				g.z *= (invert)?-1.0f:1.0f;
				
				h = d;
				h.z = -demiHeight;
				h.z *= (invert)?-1.0f:1.0f;
				break;
			case eAlign.alignX:
				a.z -= demiWidth;
				a.x -= demiDepth;
				a.x *= (invert)?-1.0f:1.0f;
				a.y += demiHeight;
				a.y *= (invert)?-1.0f:1.0f;
			
				b = a;
				b.x = demiDepth;
				b.x *= (invert)?-1.0f:1.0f;
				
				c = b;
				c.z = demiWidth;
				
				d = c;
				d.x = -demiDepth;
				d.x *= (invert)?-1.0f:1.0f;
				
				e = a;
				e.y = -demiHeight;
				e.y *= (invert)?-1.0f:1.0f;
				
				f = b;
				f.y = -demiHeight;
				f.y *= (invert)?-1.0f:1.0f;
				
				g = c;
				g.y = -demiHeight;
				g.y *= (invert)?-1.0f:1.0f;
				
				h = d;
				h.y = -demiHeight;
				h.y *= (invert)?-1.0f:1.0f;
				break;
			case eAlign.alignZ:
				a.y -= demiWidth;
				a.z -= demiDepth;
				a.z *= (invert)?-1.0f:1.0f;
				a.x += demiHeight;
				a.x *= (invert)?-1.0f:1.0f;
			
				b = a;
				b.z = demiDepth;
				b.z *= (invert)?-1.0f:1.0f;
				
				c = b;
				c.y = demiWidth;
				
				d = c;
				d.z = -demiDepth;
				d.z *= (invert)?-1.0f:1.0f;
				
				e = a;
				e.x = -demiHeight;
				e.x *= (invert)?-1.0f:1.0f;
				
				f = b;
				f.x = -demiHeight;
				f.x *= (invert)?-1.0f:1.0f;
				
				g = c;
				g.x = -demiHeight;
				g.x *= (invert)?-1.0f:1.0f;
				
				h = d;
				h.x = -demiHeight;
				h.x *= (invert)?-1.0f:1.0f;
				break;
		}
		
		
		Vector3 nab = Vector3.Normalize(b - a);
		Vector3 nad = Vector3.Normalize(d - a);
		Vector3 nae = Vector3.Normalize(e - a);
		
		Vector3 normal = Vector3.zero;
		Vector2 positionUV = Vector2.zero;

		////// begin bcgf
		normal = Vector3.zero;
		switch( align )
		{
			default:
			case eAlign.alignY:
				normal.y = 1.0f;
				break;
			case eAlign.alignX:
				normal.x = 1.0f;
				break;
			case eAlign.alignZ:
				normal.z = 1.0f;
				break;
		}
		normal *= (invert)?-1.0f:1.0f;
		normal *= (invertNormal)?-1.0f:1.0f;
		positionUV = Vector2.zero;
		positionUV.x = 1.0f/4.0f;
		positionUV.y = 2.0f/3.0f;
		
		DrawFace(b, width, height, subdivisionsWidth, subdivisionsHeight, nad, nae, normal, positionUV);
		////// end bcgf
		
		////// begin abfe
		normal = Vector3.zero;
		switch( align )
		{
			default:
			case eAlign.alignY:
				normal.x = -1.0f;
				break;
			case eAlign.alignX:
				normal.z = -1.0f;
				break;
			case eAlign.alignZ:
				normal.y = -1.0f;
				break;
		}
		normal *= (invertNormal)?-1.0f:1.0f;
		positionUV = Vector2.zero;
		positionUV.y = 2.0f/3.0f;
		
		DrawFace(a, depth, height, subdivisionsDepth, subdivisionsHeight, nab, nae, normal, positionUV);
		////// end abfe
		
		////// begin adcb
		normal = Vector3.zero;
		switch( align )
		{
			default:
			case eAlign.alignY:
				normal.z = 1.0f;
				break;
			case eAlign.alignX:
				normal.y = 1.0f;
				break;
			case eAlign.alignZ:
				normal.x = 1.0f;
				break;
		}
		normal *= (invert)?-1.0f:1.0f;
		normal *= (invertNormal)?-1.0f:1.0f;
		positionUV = Vector2.zero;
		positionUV.x = 1.0f/4.0f;
		positionUV.y = 1.0f;
		
		DrawFace(a, width, depth, subdivisionsWidth, subdivisionsDepth, nad, nab, normal, positionUV);
		////// end adcb
		
		////// begin fghe
		normal = Vector3.zero;
		switch( align )
		{
			default:
			case eAlign.alignY:
				normal.z = -1.0f;
				break;
			case eAlign.alignX:
				normal.y = -1.0f;
				break;
			case eAlign.alignZ:
				normal.x = -1.0f;
				break;
		}
		normal *= (invert)?-1.0f:1.0f;
		positionUV = Vector2.zero;
		positionUV.x = 1.0f/4.0f;
		positionUV.y = 1.0f/3.0f;
		
		DrawFace(f, width, depth, subdivisionsWidth, subdivisionsDepth, nad, -nab, normal, positionUV);
		////// end fghe
		
		////// begin cdhg
		normal = Vector3.zero;
		switch( align )
		{
			default:
			case eAlign.alignY:
				normal.x = 1.0f;
				break;
			case eAlign.alignX:
				normal.z = 1.0f;
				break;
			case eAlign.alignZ:
				normal.y = 1.0f;
				break;
		}
		normal *= (invertNormal)?-1.0f:1.0f;
		positionUV = Vector2.zero;
		positionUV.x = 2.0f/4.0f;
		positionUV.y = 2.0f/3.0f;
		
		DrawFace(c, depth, height, subdivisionsDepth, subdivisionsHeight, -nab, nae, normal, positionUV);
		////// end cdhg
		
		////// begin daeh
		normal = Vector3.zero;
		switch( align )
		{
			default:
			case eAlign.alignY:
				normal.y = -1.0f;
				break;
			case eAlign.alignX:
				normal.x = -1.0f;
				break;
			case eAlign.alignZ:
				normal.z = -1.0f;
				break;
		}
		normal *= (invert)?-1.0f:1.0f;
		normal *= (invertNormal)?-1.0f:1.0f;
		positionUV = Vector2.zero;
		positionUV.x = 3.0f/4.0f;
		positionUV.y = 2.0f/3.0f;
		
		DrawFace(d, width, height, subdivisionsWidth, subdivisionsHeight, -nad, nae, normal, positionUV);
		////// end daeh
		
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
		mesh.normals = newNormals.ToArray();
		
		meshFilter.mesh = mesh;
	}
	
	void DrawFace(Vector3 top, float width, float height, int subWidth, int subHeight, Vector3 nu, Vector3 nv, Vector3 normal, Vector2 positionUV)
	{
		int startId = newVertices.Count;
		float tileWidth = width/(float)subWidth;
		float tileHeight = height/(float)subHeight;
		Vector3 position;
		Vector2 tempUV;
		
		for( int i = 0; i <= subHeight; ++i )
		{
			for( int j = 0; j <= subWidth; ++j )
			{
				position = top;
				position += i*nv*tileHeight;
				position += j*nu*tileWidth;
				
				tempUV.x = positionUV.x + (j/(float)subWidth)/4.0f;
				tempUV.y = positionUV.y - (i/(float)subHeight)/3.0f;
				
				newVertices.Add( position );
				newNormals.Add( normal );
				newUV.Add( tempUV );
				
				if( j != subWidth && i != subHeight )
				{
					int posId = startId + j + i*(subWidth + 1);
					
					newTriangles.Add( posId );
					if( !invertNormal )
					{
						newTriangles.Add( posId + 1 + subWidth + 1);
						newTriangles.Add( posId + subWidth + 1 );
					}
					else
					{
						newTriangles.Add( posId + subWidth + 1 );
						newTriangles.Add( posId + 1 + subWidth + 1);
					}
					
					newTriangles.Add( posId );
					if( !invertNormal )
					{
						newTriangles.Add( posId + 1 );
						newTriangles.Add( posId + 1 + subWidth + 1 );
					}
					else
					{
						newTriangles.Add( posId + 1 + subWidth + 1 );
						newTriangles.Add( posId + 1 );
					}
				}
			}
		}
	}
	
	protected override string getName()
	{
		return "ParametricCube";
	}
}
