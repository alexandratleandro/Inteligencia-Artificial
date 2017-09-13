﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonGenerator {

	private Vector2 startposition;
	private Vector2[] vertices2D;

	public void drawCurve (Dictionary<float,float>trackpoints,ProblemInfo info) {

		vertices2D = new Vector2[trackpoints.Count*2];
		startposition = new Vector2 (info.startPointX, info.startPointY);
		int index = 0;

		foreach (float key in trackpoints.Keys) {
			vertices2D[index++] = new Vector2(key,trackpoints[key]);
		}
		//in order for the mesh to have thickness
		index = vertices2D.Length-1;
		foreach (float key in trackpoints.Keys) {
			vertices2D[index--] = new Vector2(key,trackpoints[key]-0.2f);
		}


		Triangulator tr = new Triangulator(vertices2D);
		int[] indices = tr.Triangulate();

		Vector3[] vertices = new Vector3[vertices2D.Length];

		for (int i=0; i<vertices.Length; i++) {
			vertices[i] = new Vector3(vertices2D[i].x,vertices2D[i].y,0);
		}

		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals ();
		msh.RecalculateBounds ();
	
	
		GameObject curve = new GameObject ();
		Renderer rend = curve.AddComponent(typeof(MeshRenderer)) as Renderer;
		MeshFilter filter = curve.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;
		rend.material.SetColor("_Color", Color.black);
		curve.AddComponent (typeof(MeshCollider));
		}
}

public class Triangulator
{
	private List<Vector2> m_points = new List<Vector2>();
	
	public Triangulator (Vector2[] points) {
		m_points = new List<Vector2>(points);
	}
	
	public int[] Triangulate() {
		List<int> indices = new List<int>();
		
		int n = m_points.Count;
		if (n < 3)
			return indices.ToArray();
		
		int[] V = new int[n];
		if (Area() > 0) {
			for (int v = 0; v < n; v++)
				V[v] = v;
		}
		else {
			for (int v = 0; v < n; v++)
				V[v] = (n - 1) - v;
		}
		
		int nv = n;
		int count = 2 * nv;
		for (int m = 0, v = nv - 1; nv > 2; ) {
			if ((count--) <= 0)
				return indices.ToArray();
			
			int u = v;
			if (nv <= u)
				u = 0;
			v = u + 1;
			if (nv <= v)
				v = 0;
			int w = v + 1;
			if (nv <= w)
				w = 0;
			
			if (Snip(u, v, w, nv, V)) {
				int a, b, c, s, t;
				a = V[u];
				b = V[v];
				c = V[w];
				indices.Add(a);
				indices.Add(b);
				indices.Add(c);
				m++;
				for (s = v, t = v + 1; t < nv; s++, t++)
					V[s] = V[t];
				nv--;
				count = 2 * nv;
			}
		}
		
		indices.Reverse();
		return indices.ToArray();
	}
	
	private float Area () {
		int n = m_points.Count;
		float A = 0.0f;
		for (int p = n - 1, q = 0; q < n; p = q++) {
			Vector2 pval = m_points[p];
			Vector2 qval = m_points[q];
			A += pval.x * qval.y - qval.x * pval.y;
		}
		return (A * 0.5f);
	}
	
	private bool Snip (int u, int v, int w, int n, int[] V) {
		int p;
		Vector2 A = m_points[V[u]];
		Vector2 B = m_points[V[v]];
		Vector2 C = m_points[V[w]];
		if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
			return false;
		for (p = 0; p < n; p++) {
			if ((p == u) || (p == v) || (p == w))
				continue;
			Vector2 P = m_points[V[p]];
			if (InsideTriangle(A, B, C, P))
				return false;
		}
		return true;
	}
	
	private bool InsideTriangle (Vector2 A, Vector2 B, Vector2 C, Vector2 P) {
		float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
		float cCROSSap, bCROSScp, aCROSSbp;
		
		ax = C.x - B.x; ay = C.y - B.y;
		bx = A.x - C.x; by = A.y - C.y;
		cx = B.x - A.x; cy = B.y - A.y;
		apx = P.x - A.x; apy = P.y - A.y;
		bpx = P.x - B.x; bpy = P.y - B.y;
		cpx = P.x - C.x; cpy = P.y - C.y;
		
		aCROSSbp = ax * bpy - ay * bpx;
		cCROSSap = cx * apy - cy * apx;
		bCROSScp = bx * cpy - by * cpx;
		
		return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
	}
}