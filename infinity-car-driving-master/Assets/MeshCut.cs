using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;

public class MeshCut : MonoBehaviour
{
    public Vector3 CutPlanePoint;
    public Vector3 CutPlaneNormal;
    private Mesh OriginMesh;
    private List<Vector3> UpVetx = new List<Vector3>();
    private List<Vector3> DownVetx = new List<Vector3>();

    private List<int> UpTri = new List<int>();
    private List<int> DownTri = new List<int>();

    private List<Vector2> UpUV = new List<Vector2>();
    private List<Vector2> DownUV = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        OriginMesh = meshFilter.mesh;
        Mesh[] cutMeshes = new Mesh[2];
        cutMeshes = CutMesh(new Plane(CutPlaneNormal.normalized,CutPlanePoint));

        for (int i = 0; i < cutMeshes.Length; i++)
        {
            GameObject obj = new GameObject("Cut Mesh");
            obj.AddComponent<MeshFilter>().mesh = cutMeshes[i];
            obj.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
        }
        meshFilter.mesh.uv2 = null;
    }

    private Mesh[] CutMesh(Plane cutPlane)
    {
        Mesh[] CutMeshes = new Mesh[2];
        CutMeshes[0] = new Mesh();
        CutMeshes[1] = new Mesh();

        //���㲻�ཻ��Mesh���ϻ����£�ͨ��������ķ�������ˣ���Ƕȼ����Լ��㣬�����������ǣ���������Ƕ۽�
        for (int i = 0; i < OriginMesh.triangles.Length; i += 3) 
        {
            //һ�����������������
            bool v1 = cutPlane.GetSide(OriginMesh.vertices[OriginMesh.triangles[i]]);
            bool v2 = cutPlane.GetSide(OriginMesh.vertices[OriginMesh.triangles[i+1]]);
            bool v3 = cutPlane.GetSide(OriginMesh.vertices[OriginMesh.triangles[i+2]]);

            //���»����������

            if (v1 && v2 && v3)
            {
                //���1:��������
                GenTriangle(ref UpVetx, ref UpTri, ref UpUV, i, i + 1, i + 2);
                UpUV.Add(OriginMesh.uv[OriginMesh.triangles[i]]);
                UpUV.Add(OriginMesh.uv[OriginMesh.triangles[i+1]]);
                UpUV.Add(OriginMesh.uv[OriginMesh.triangles[i+2]]);
            }
            else if (!v1 && !v2 && !v3)
            {
                GenTriangle(ref DownVetx, ref DownTri,ref DownUV, i, i + 1, i + 2);
                //���1:��������
                DownUV.Add(OriginMesh.uv[OriginMesh.triangles[i]]);
                DownUV.Add(OriginMesh.uv[OriginMesh.triangles[i + 1]]);
                DownUV.Add(OriginMesh.uv[OriginMesh.triangles[i + 2]]);
            }
            else 
            {
                //���и����ཻ

                //�������������������������»���һ����������,�ó����������boolֵ��������㽻���ɺͽ���ɣ�
                bool boolcount = v1 ^ v2 ^ v3;
                //Ѱ�Ҷ��㣬��Ϊ�ܹ�����������һ�ߣ�����һ������һ��
                int StandloneVetx;
                int UnityVetx1;
                int UnityVetx2;
                Vector3 interaction1= Vector3.zero;
                Vector3 interaction2= Vector3.zero;

                if (v1 == boolcount)
                {
                    StandloneVetx = i;
                    UnityVetx1 = i + 1;
                    UnityVetx2 = i + 2;
                    interaction1 = GetInteraction(StandloneVetx, UnityVetx1);
                    interaction2 = GetInteraction(StandloneVetx, UnityVetx2);
                    //ע����Ҫ˳ʱ�����������ζ��㣬��Ϊ��ʱ���������棬���ɼ�
                    if (boolcount == true)
                    {
                        //��
                        //�����ɵĵ�һ��������
                        GenNewTriangle(ref UpVetx, ref UpTri, OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]], interaction1, interaction2);

                        //��
                        //�����ɵĵڶ���������
                        GenNewTriangle(ref DownVetx, ref DownTri, interaction1, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]], interaction2);

                        //�����ɵĵ�����������
                        GenNewTriangle(ref DownVetx, ref DownTri, interaction2, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]], OriginMesh.vertices[OriginMesh.triangles[UnityVetx2]]);
                    }
                    else
                    {
                        //��
                        GenNewTriangle(ref DownVetx, ref DownTri, OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]], interaction1, interaction2);
                        //��
                        //�����ɵĵڶ���������
                        GenNewTriangle(ref UpVetx, ref UpTri, interaction1, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]], interaction2);

                        //�����ɵĵ�����������
                        GenNewTriangle(ref UpVetx, ref UpTri, interaction2, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]], OriginMesh.vertices[OriginMesh.triangles[UnityVetx2]]);
                    }
                }
                else if (v2 == boolcount)
                {
                    StandloneVetx = i+1;
                    UnityVetx1 = i;
                    UnityVetx2 = i + 2;
                    interaction1 = GetInteraction(StandloneVetx, UnityVetx1);
                    interaction2 = GetInteraction(StandloneVetx, UnityVetx2);

                    if (boolcount == true)
                    {
                        //��
                        //�����ɵĵ�һ��������
                        GenNewTriangle(ref UpVetx, ref UpTri, OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]], interaction2, interaction1);

                        //��
                        //�����ɵĵڶ���������
                        GenNewTriangle(ref DownVetx, ref DownTri, interaction1,interaction2, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]]);

                        //�����ɵĵ�����������
                        GenNewTriangle(ref DownVetx, ref DownTri, interaction2, OriginMesh.vertices[OriginMesh.triangles[UnityVetx2]], OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]]);
                    }
                    else
                    {
                        //��
                        GenNewTriangle(ref DownVetx, ref DownTri, OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]], interaction2, interaction1);
                        //��
                        //�����ɵĵڶ���������
                        GenNewTriangle(ref UpVetx, ref UpTri, interaction1, interaction2, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]]);

                        //�����ɵĵ�����������
                        GenNewTriangle(ref UpVetx, ref UpTri, interaction2, OriginMesh.vertices[OriginMesh.triangles[UnityVetx2]], OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]]);
                    }
                }
                else
                {
                    StandloneVetx = i+2;
                    UnityVetx1 = i ;
                    UnityVetx2 = i + 1;
                    interaction1 = GetInteraction(StandloneVetx, UnityVetx1);
                    interaction2 = GetInteraction(StandloneVetx, UnityVetx2);

                    if (boolcount == true)
                    {
                        //��
                        //�����ɵĵ�һ��������
                        GenNewTriangle(ref UpVetx, ref UpTri, OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]], interaction1, interaction2);

                        //��
                        //�����ɵĵڶ���������
                        GenNewTriangle(ref DownVetx, ref DownTri, interaction1, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]], interaction2);

                        //�����ɵĵ�����������
                        GenNewTriangle(ref DownVetx, ref DownTri, interaction2, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]], OriginMesh.vertices[OriginMesh.triangles[UnityVetx2]]);
                    }
                    else
                    {
                        //��
                        GenNewTriangle(ref DownVetx, ref DownTri, OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]], interaction1, interaction2);
                        //��
                        //�����ɵĵڶ���������
                        GenNewTriangle(ref UpVetx, ref UpTri, interaction1, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]], interaction2);

                        //�����ɵĵ�����������
                        GenNewTriangle(ref UpVetx, ref UpTri, interaction2, OriginMesh.vertices[OriginMesh.triangles[UnityVetx1]], OriginMesh.vertices[OriginMesh.triangles[UnityVetx2]]);
                    }
                }
            }
        }
        CutMeshes[0].vertices = UpVetx.ToArray();
        CutMeshes[0].triangles = UpTri.ToArray();
        CutMeshes[0].uv = UpUV.ToArray();

        CutMeshes[1].vertices = DownVetx.ToArray();
        CutMeshes[1].triangles = DownTri.ToArray();
        CutMeshes[1].uv = DownUV.ToArray();

        return CutMeshes;
    }

    private Vector3 GetInteraction(int StandloneVetx, int UnityVetx) 
    {

        //�󽻵�ԭ��ȡƽ����һ������߳��������ӣ����䷨�߷���ͶӰ������ͶӰ�Ĵ�С�͵�λ����ͶӰ�Ĵ�С�ı�����ϵȷ��һ������
        Vector3 direct = OriginMesh.vertices[OriginMesh.triangles[UnityVetx]] - OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]];

        float d = Vector3.Dot(CutPlanePoint - OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]], CutPlaneNormal) / Vector3.Dot(direct.normalized, CutPlaneNormal);
        Vector3 intereaction = d * direct.normalized + OriginMesh.vertices[OriginMesh.triangles[StandloneVetx]];
        float x = Vector3.Magnitude(d * direct.normalized) / Vector3.Magnitude(direct);
        Vector2 uv = OriginMesh.uv[OriginMesh.triangles[StandloneVetx]] + (OriginMesh.uv[OriginMesh.triangles[UnityVetx]] - OriginMesh.uv[OriginMesh.triangles[StandloneVetx]]) * x;

        return intereaction;
    }

    private void GenTriangle(ref List<Vector3> vets, ref List<int> tris,ref List<Vector2> uv,int p1,int p2,int p3)
    {
        vets.Add(OriginMesh.vertices[OriginMesh.triangles[p1]]);
        tris.Add(vets.Count - 1);

        vets.Add(OriginMesh.vertices[OriginMesh.triangles[p2]]);
        tris.Add(vets.Count - 1);

        vets.Add(OriginMesh.vertices[OriginMesh.triangles[p3]]);
        tris.Add(vets.Count - 1);
    }

    private void GenNewTriangle(ref List<Vector3> vets, ref List<int> tris, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        vets.Add(p1);
        tris.Add(vets.Count - 1);

        vets.Add(p2);
        tris.Add(vets.Count - 1);

        vets.Add(p3);
        tris.Add(vets.Count - 1);
    }
}
