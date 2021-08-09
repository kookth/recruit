using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class APIResponse
{
    public bool success;
    public int code;
    public Data[] data;
}

[Serializable]
public class Data
{
    public Roomtypes[] roomtypes;
    public DataMeta meta;
}

[Serializable]
public class DataMeta
{
    public int bd_id;
    public string 동;
    public int 지면높이;
}

[Serializable]
public class Roomtypes
{
    public string[] coordinatesBase64s;
    public RoomtypesMeta meta;
    public Vector3[] polyGonData;
}

[Serializable]
public class RoomtypesMeta
{
    public int 룸타입id;
}


public class NewBehaviourScript : MonoBehaviour
{
    public Texture m_texture = null;

    // Start is called before the first frame update
    void Start()
    {
        System.IO.FileInfo loadfile = new System.IO.FileInfo("Assets/Samples/json/dong.json");
        string sJsonData = File.ReadAllText(loadfile.FullName);

        APIResponse pData = JsonUtility.FromJson<APIResponse>(sJsonData);

        for(int i=0; i<pData.data.Length; i++)
        {
            for(int j=0; j<pData.data[i].roomtypes.Length; j++)
            {
                pData.data[i].roomtypes[j].polyGonData = ConvertPoly(pData.data[i].roomtypes[j].coordinatesBase64s);
                //Debug.Log(pData.data[i].roomtypes[j].polyGonData.Length);
                //Debug.Log(pData.data[i].roomtypes[j].polyGonData[0].x);
            }
        }

        //Debug.Log(pData.data[0].roomtypes[0].coordinatesBase64s[0].Length);
        //----------------------여기까지가 제이슨 정리





        Instantiate(Resources.Load("Prefabs/DongObject") as GameObject, new Vector3(0, 0), Quaternion.identity);

        for (int i = 0; i < pData.data.Length; i++)
        {
            GameObject dongObject = Instantiate(Resources.Load("Prefabs/DongObject") as GameObject, new Vector3(0, 0), Quaternion.identity);
            dongObject.name = pData.data[i].meta.동 + "DongName";

            for (int j = 0; j < pData.data[i].roomtypes.Length; j++)
            {
                GameObject roomObject = Instantiate(Resources.Load("Prefabs/RoomObject") as GameObject, new Vector3(0, 0), Quaternion.identity);
                roomObject.name = pData.data[i].roomtypes[j].meta.룸타입id + "RoomID";
                roomObject.transform.parent = dongObject.transform;

                Vector3[] totalVertices = pData.data[i].roomtypes[j].polyGonData;
                int[] triangles = new int[totalVertices.Length*3 - 6];
                int indexCount = 0;

                for (int k=0; k< triangles.Length/3; k++)
                {
                    triangles[k * 3] = indexCount;
                    triangles[k * 3 + 1] = indexCount + 1;
                    triangles[k * 3 + 2] = indexCount + 2;
                    indexCount++;
                }

                Vector2[] uvs = new Vector2[] { new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f)};

                Mesh mesh = new Mesh();

                mesh.vertices = totalVertices;
                mesh.triangles = triangles;
                mesh.uv = uvs;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();


                roomObject.GetComponent<MeshFilter>().mesh = mesh;

                Material material = new Material(Shader.Find("Standard"));
                material.SetTexture("m_tex", m_texture);
                roomObject.GetComponent<MeshRenderer>().material = material;

                //Debug.Log(pData.data[i].roomtypes[j].polyGonData.Length);
                //Debug.Log(pData.data[i].roomtypes[j].polyGonData[0].x);
            }
        }












        //Material material = new Material(Shader.Find("Standard"));
        //GetComponent<MeshRenderer>().material = material;














    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3[] ConvertPoly(string[] coordinatesBase64s)
    {
        List<byte> byte64 = new List<byte>();
        for(int i=0; i< coordinatesBase64s.Length; i++)
        {
            byte64.AddRange(Convert.FromBase64String(coordinatesBase64s[i]));
        }

        int totalCount = byte64.Count / 12;

        Vector3[] polyGonData = new Vector3[totalCount];

        for (int i=0; i<totalCount; i++)
        {
            float x = BitConverter.ToSingle(byte64.ToArray(), i * 12);
            float y = BitConverter.ToSingle(byte64.ToArray(), i * 12 + 8);
            float z = BitConverter.ToSingle(byte64.ToArray(), i * 12 + 4);

            polyGonData[i] = new Vector3(x, y, z);
        }

        return polyGonData;
    }
}
