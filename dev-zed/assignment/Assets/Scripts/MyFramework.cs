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


public class MyFramework : MonoBehaviour
{
    [SerializeField] public Texture m_texture = null;

    // Start is called before the first frame update
    void Start()
    {
        System.IO.FileInfo loadfile = new System.IO.FileInfo("Assets/Samples/json/dong.json");
        string sJsonData = File.ReadAllText(loadfile.FullName);

        APIResponse pData = JsonUtility.FromJson<APIResponse>(sJsonData);

        for(int i=0; i<pData.data.Length; i++)
        {
            Debug.Log("11111111111111111111111111  " + pData.data[i].meta.동 + "  |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            for (int j=0; j<pData.data[i].roomtypes.Length; j++)
            {
                pData.data[i].roomtypes[j].polyGonData = ConvertPoly(pData.data[i].roomtypes[j].coordinatesBase64s);
                //Debug.Log(pData.data[i].roomtypes[j].polyGonData.Length);
                //Debug.Log(pData.data[i].roomtypes[j].polyGonData[0].x);


                Debug.Log("11111111111111111111111111    " + pData.data[i].roomtypes[j].meta.룸타입id + "   ------------------------------------------------");



                for (int k=0; k< pData.data[i].roomtypes[j].polyGonData.Length; k++)
                {
                    Debug.Log(pData.data[i].roomtypes[j].polyGonData[k].x + " : " + pData.data[i].roomtypes[j].polyGonData[k].y + " : " + pData.data[i].roomtypes[j].polyGonData[k].z);
                }



                Debug.Log("2222222222222222222222222    " + pData.data[i].roomtypes[j].meta.룸타입id + "   ------------------------------------------------");
            }

            Debug.Log("222222222222222222222222222  " + pData.data[i].meta.동 + "  |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
        }

        //Debug.Log(pData.data[0].roomtypes[0].coordinatesBase64s[0].Length);
        //----------------------여기까지가 제이슨 정리













        Instantiate(Resources.Load("Prefabs/DongObject") as GameObject, new Vector3(0, 0), Quaternion.identity);

        for (int i = 0; i < pData.data.Length; i++)
        {
            GameObject dongObject = Instantiate(Resources.Load("Prefabs/DongObject") as GameObject, new Vector3(0, pData.data[i].meta.지면높이, 0), Quaternion.identity);
            dongObject.name = pData.data[i].meta.동 + "DongName";

            for (int j = 0; j < pData.data[i].roomtypes.Length; j++)
            {
                GameObject roomObject = Instantiate(Resources.Load("Prefabs/RoomObject") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
                roomObject.name = pData.data[i].roomtypes[j].meta.룸타입id + "RoomID";
                roomObject.transform.parent = dongObject.transform;

                Vector3[] totalVertices = pData.data[i].roomtypes[j].polyGonData;
                int[] triangles = new int[totalVertices.Length];

                for (int k=0; k< triangles.Length; k++)
                {
                    triangles[k] = k;
                }




                Vector2[] uvs = null;




                if(pData.data[i].roomtypes[j].meta.룸타입id == 13725)
                {
                    uvs = new Vector2[pData.data[i].roomtypes[j].polyGonData.Length];



                    //int[] temp = new int[4] { 1, 11, 10, 0 };

                    //for (int k = 0; k < uvs.Length; k++)
                    //{
                    //    uvs[k] = new Vector2((float)(temp[k % 4] / 10f), (float)(temp[k % 4] % 10f));
                    //}









                    float[] tempFloatU = new float[] { 3 / 4f, 1f, 1f };
                    float[] tempFloatV = new float[] { 0f, 1f, 0f };

                    for (int k = 0; k < 24; k++)
                    {
                        uvs[k] = new Vector2(tempFloatU[k % 3], tempFloatV[k % 3]);
                    }







                    float[] tempFloatUType1Front = new float[] { 2/4f, 0f, 0f };
                    float[] tempFloatVType1Front = new float[] { 0f, 0f, 1f };

                    float[] tempFloatUType2Front = new float[] {0f, 2/4f, 2/4f };
                    float[] tempFloatVType2Front = new float[] { 1f, 1f, 0f };


                    float[] tempFloatUType1Other = new float[] { 3/4f, 2/4f, 2/4f };
                    float[] tempFloatVType1Other = new float[] { 0f, 0f, 1f };

                    float[] tempFloatUType2Other = new float[] { 2/4f, 3/4f, 3/4f };
                    float[] tempFloatVType2Other = new float[] { 1f, 1f, 0f };

                    for (int k=24; k<uvs.Length; k=k+3)
                    {



                        if (checkAngle(pData.data[i].roomtypes[j].polyGonData[k], pData.data[i].roomtypes[j].polyGonData[k+1], pData.data[i].roomtypes[j].polyGonData[k+2]))

                        {
                            if (pData.data[i].roomtypes[j].polyGonData[k].y > 10f)
                            {
                                uvs[k] = new Vector2(tempFloatUType2Front[0], tempFloatVType2Front[0]);
                                uvs[k+1] = new Vector2(tempFloatUType2Front[1], tempFloatVType2Front[1]);
                                uvs[k+2] = new Vector2(tempFloatUType2Front[2], tempFloatVType2Front[2]);
                                Debug.Log("ww1 : " + pData.data[i].roomtypes[j].polyGonData[k]);
                                Debug.Log("ww1 : " + pData.data[i].roomtypes[j].polyGonData[k+1]);
                                Debug.Log("ww1 : " + pData.data[i].roomtypes[j].polyGonData[k+2]);
                            }
                            else
                            {
                                uvs[k] = new Vector2(tempFloatUType1Front[0], tempFloatVType1Front[0]);
                                uvs[k + 1] = new Vector2(tempFloatUType1Front[1], tempFloatVType1Front[1]);
                                uvs[k + 2] = new Vector2(tempFloatUType1Front[2], tempFloatVType1Front[2]);
                                Debug.Log("ww2 : " + pData.data[i].roomtypes[j].polyGonData[k]);
                                Debug.Log("ww2 : " + pData.data[i].roomtypes[j].polyGonData[k+1]);
                                Debug.Log("ww2 : " + pData.data[i].roomtypes[j].polyGonData[k+2]);
                            }
                        }
                        else
                        {
                            if (pData.data[i].roomtypes[j].polyGonData[k].y > 10f)
                            {
                                //uvs[k] = new Vector2(tempFloatUType2Front[0], tempFloatVType2Front[0]);
                                //uvs[k + 1] = new Vector2(tempFloatUType2Front[1], tempFloatVType2Front[1]);
                                //uvs[k + 2] = new Vector2(tempFloatUType2Front[2], tempFloatVType2Front[2]);
                                uvs[k] = new Vector2(tempFloatUType2Other[0], tempFloatVType2Other[0]);
                                uvs[k + 1] = new Vector2(tempFloatUType2Other[1], tempFloatVType2Other[1]);
                                uvs[k + 2] = new Vector2(tempFloatUType2Other[2], tempFloatVType2Other[2]);
                                Debug.Log("ww3 : " + pData.data[i].roomtypes[j].polyGonData[k]);
                                Debug.Log("ww3 : " + pData.data[i].roomtypes[j].polyGonData[k+1]);
                                Debug.Log("ww3 : " + pData.data[i].roomtypes[j].polyGonData[k+2]);
                            }
                            else
                            {
                                //uvs[k] = new Vector2(tempFloatUType1Front[0], tempFloatVType1Front[0]);
                                //uvs[k + 1] = new Vector2(tempFloatUType1Front[1], tempFloatVType1Front[1]);
                                //uvs[k + 2] = new Vector2(tempFloatUType1Front[2], tempFloatVType1Front[2]);
                                uvs[k] = new Vector2(tempFloatUType1Other[0], tempFloatVType1Other[0]);
                                uvs[k + 1] = new Vector2(tempFloatUType1Other[1], tempFloatVType1Other[1]);
                                uvs[k + 2] = new Vector2(tempFloatUType1Other[2], tempFloatVType1Other[2]);
                                Debug.Log("ww4 : " + pData.data[i].roomtypes[j].polyGonData[k]);
                                Debug.Log("ww4 : " + pData.data[i].roomtypes[j].polyGonData[k+1]);
                                Debug.Log("ww4 : " + pData.data[i].roomtypes[j].polyGonData[k+2]);
                            }
                        }
                    }






                }
                else
                {
                    uvs = new Vector2[pData.data[i].roomtypes[j].polyGonData.Length];

                    int[] temp = new int[4] { 1, 11, 10, 0 };

                    for (int k = 0; k < uvs.Length; k++)
                    {
                        uvs[k] = new Vector2((float)(temp[k % 4] / 10f), (float)(temp[k % 4] % 10f));
                    }
                }







                Mesh mesh = new Mesh();

                mesh.vertices = totalVertices;
                mesh.triangles = triangles;
                mesh.uv = uvs;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();


                roomObject.GetComponent<MeshFilter>().mesh = mesh;

                Material newMaterial = Resources.Load("Materials/Materials/buildingTester_d", typeof(Material)) as Material;

                newMaterial.SetTextureScale("Wefwefwef", new Vector2(10f, 3));

                roomObject.GetComponent<MeshRenderer>().material = newMaterial;

                //Material material = new Material(Shader.Find("buildingTester_d"));
                ////material.SetTexture("_MainTex", m_texture);
                //roomObject.GetComponent<MeshRenderer>().material = material;

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

    public bool checkAngle(Vector3 a, Vector3 b, Vector3 c)
    {
        var dir = Vector3.Cross(a - b, c - b);
        Vector3 norm = Vector3.Normalize(dir);


        Debug.Log("CalculateAngle(norm) : " + CalculateAngle(norm));


        if (CalculateAngle(norm) >= 180f && CalculateAngle(norm) <= 220f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float CalculateAngle(Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - new Vector3(0f, 1f, 0f)).eulerAngles.z;
    }
}
