//Code modified from http://docs.unity3d.com/ScriptReference/GL.LoadPixelMatrix.html

using UnityEngine;
using System.Collections;

public class linedraw : MonoBehaviour {

    public Material mat;
	// Draws a red triangle using pixels as coordinates to paint on.
	void OnPostRender() {
		if (!mat) {
			Debug.LogError("Please Assign a material on the inspector");
		}
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadPixelMatrix();
		GL.Color(Color.red);
		GL.Begin(GL.TRIANGLES);
		GL.Vertex3(0,0,0);
		GL.Vertex3(0,Screen.height/2,0);
		GL.Vertex3(Screen.width/2,Screen.height/2,0);
		GL.End();
		GL.PopMatrix();
	}
    void Update()
    {
        OnPostRender();
    }
}
