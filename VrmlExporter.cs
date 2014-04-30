using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

//VrmlExporter.cs 
//this is exporter of VRML file(.wrl) writtened by aki426 reffering to ObjExporter.
//http://wiki.unity3d.com/index.php?title=ObjExporter
//Either call VrmlExporter.MeshToString() to generate a text string containing the wrl file
//or VrmlExporter.MeshToFile() to save it directly to a file. 
public class VrmlExporter
{
    public static string MeshToString (MeshFilter mf, float scale)
    {
        Mesh m = mf.mesh;
        Material[] mats = mf.renderer.sharedMaterials;
        Color col = mats [0].color;
        Vector3 basePosition = mf.transform.position;
            
        StringBuilder sb = new StringBuilder ();
            
        //shape name
        sb.Append ("# " + mf.name + " ");
        sb.Append (string.Format (
            "x:{0} y:{1} z:{2}",
            basePosition.x, basePosition.y, basePosition.z));
        sb.Append ("\r\n");
            
        //vertexs
        sb.Append (
            "Shape {\r\n" +
            "\tappearance Appearance {\r\n" +
            "\t\tmaterial Material {\r\n" +
            "\t\t}}\r\n");
            
        //points
        sb.Append (
            "\tgeometry IndexedFaceSet {\r\n" +
            "\t\tcoord Coordinate {\r\n" +
            "\t\t\tpoint [ ");
        foreach (Vector3 v in m.vertices) {
            sb.Append (string.Format (
                "{0:F6} {1:F6} {2:F6} ",
                v.x * scale + basePosition.x, v.y * scale + basePosition.y, v.z * scale + basePosition.z));
        }
        sb.Append ("]}\r\n");
            
        //color per Vertex
        sb.Append (
            "\t\tcolorPerVertex TRUE\r\n" +
            "\t\tcolor Color {\r\n" +
            "\t\t\tcolor [ ");
        for (int i = 0; i < m.vertices.Length; i++) {
            sb.Append (string.Format (
                "{0:F2} {1:F2} {2:F2} ",
                col.r, col.g, col.b));
        }
        sb.Append ("]}\r\n");
            
        //triangles
        sb.Append ("\t\tcoordIndex [ ");
        for (int material=0; material < m.subMeshCount; material ++) {
            int[] triangles = m.GetTriangles (material);
            for (int i=0; i<triangles.Length; i+=3) {
                sb.Append (string.Format (
                    "{0} {1} {2} -1 ", 
                    triangles [i], triangles [i + 1], triangles [i + 2])
                );
            }
        }
        sb.Append (
            "]}\r\n" +
            "}\r\n");
            
        return sb.ToString ();
    }
        
    public static void MeshToFile (MeshFilter mf, string filename, float scale)
    {
        using (StreamWriter sw = new StreamWriter(filename)) {
            sw.Write ("#VRML V2.0 utf8\r\n");
            sw.Write (MeshToString (mf, scale));
        }
    }
}

