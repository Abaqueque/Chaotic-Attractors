using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    public int numPoints = 400;
    public float colorCycleSpeed = 0.02f;
    public float trailTime = 0.2f;
    public Material mat;

    public GameObject[] points;

    private enum Attractor { LORENZ, AIZAWA, HALVORSEN, DADRAS, ROSSLER, SPROTTLINZ };

    private int numAttractors = 6;

    private Attractor currentAttractor = Attractor.LORENZ;

    Dictionary<Attractor, float> scaleFactor = new Dictionary<Attractor, float>() {
        {Attractor.LORENZ, 1},
        {Attractor.AIZAWA, 10},
        {Attractor.HALVORSEN, 2},
        {Attractor.DADRAS, 3},
        {Attractor.ROSSLER, 1},
        {Attractor.SPROTTLINZ, 5}
    };


    Vector3 LorenzUpdate(Vector3 pos)
    {
        float s = 10.0f;
        float p = 28.0f;
        float b = 8.0f / 3.0f;

        float dx = s * (pos.y - pos.x);
        float dy = pos.x * (p - pos.z) - pos.y;
        float dz = pos.x * pos.y - b * pos.z;
        float dt = Time.deltaTime;

        pos += new Vector3(dx, dy, dz) * dt;

        return pos;
    }

    Vector3 AizawaUpdate(Vector3 pos)
    {
        float a = 0.95f;
        float b = 0.7f;
        float c = 0.65f;
        float d = 3.5f;
        float e = 0.25f;
        float f = 0.1f;

        float dx = (pos.z - b) * pos.x - d * pos.y;
        float dy = d * pos.x + (pos.z - b) * pos.y;
        float dz1 = c + a * pos.z - (pos.z * pos.z * pos.z) / 3;
        float dz2 = -(pos.x * pos.x + pos.y * pos.y) * (1 + e * pos.z);
        float dz3 = f * pos.z * pos.x * pos.x * pos.x;
        float dz = dz1 + dz2 + dz3;
        float dt = Time.deltaTime;

        pos += new Vector3(dx, dy, dz) * dt;
        return pos;
    }


    Vector3 HalvorsenUpdate(Vector3 pos)
    {
        float a = 1.4f;

        float dx = (-a * pos.x - 4 * pos.y - 4 * pos.z - pos.y * pos.y);
        float dy = (-a * pos.y - 4 * pos.z - 4 * pos.x - pos.z * pos.z);
        float dz = (-a * pos.z - 4 * pos.x - 4 * pos.y - pos.x * pos.x);
        float dt = Time.deltaTime;

        pos += new Vector3(dx, dy, dz) * dt;
        return pos;
    }

    Vector3 DadrasUpdate(Vector3 pos)
    {
        float p = 3.0f;
        float o = 2.7f;
        float r = 1.7f;
        float c = 2.0f;
        float e = 9.0f;

        float dx = (pos.y - p * pos.x + o * pos.y * pos.z);
        float dy = (r * pos.y - pos.x * pos.z + pos.z);
        float dz = (c * pos.x * pos.y - e * pos.z);
        float dt = Time.deltaTime;

        pos += new Vector3(dx, dy, dz) * dt;
        return pos;
    }

    Vector3 RosslerUpdate(Vector3 pos)
    {
        float a = 0.1f;
        float b = 0.1f;
        float c = 14.0f;

        float dx = -pos.y - pos.z;
        float dy = a * pos.y + pos.x;
        float dz = b + pos.z * (-c + pos.x);
        float dt = Time.deltaTime * 3.0f;

        pos += new Vector3(dx, dy, dz) * dt;
        return pos;
    }

    Vector3 SprottLinzUpdate(Vector3 pos)
    { 
        float a = 0.4f;
        float b = 1.2f;
        float c = 1.0f;

        float dx = a * pos.y * pos.z;
        float dy = pos.x - b * pos.y;
        float dz = c - pos.x * pos.y;
        float dt = Time.deltaTime * 3.0f;

        pos += new Vector3(dx, dy, dz) * dt;
        return pos;
    }

    void ResetPoints()
    {
        for (int i = 0; i < numPoints; i++)
        {
            Destroy(points[i]);
            points[i] = new GameObject("Point");

            Transform ptTransform = points[i].transform;
            Vector3 offset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            ptTransform.position = Vector3.zero + offset;

            TrailRenderer trail = points[i].AddComponent<TrailRenderer>();
            trail.time = trailTime;
            trail.startWidth = 0.1f;
            trail.endWidth = 0.1f;
            trail.numCapVertices = 2;
            trail.material = mat;

        }
    }

    void AttractorUpdate()
    {
        if (Input.GetKeyDown("r"))
        {
            ResetPoints();
        }

        if (Input.GetKeyDown("e"))
        {
            ResetPoints();
            currentAttractor += 1;
            if ((int)currentAttractor >= numAttractors)
            {
                currentAttractor -= numAttractors;
            }
        }

        if (Input.GetKeyDown("q"))
        {
            ResetPoints();
            currentAttractor -= 1;
            if ((int)currentAttractor < 0)
            {
                currentAttractor += numAttractors;
            }
        }


        for (int i = 0; i < numPoints; i++)
        {
            points[i].transform.position /= scaleFactor[currentAttractor];

            Vector3 pos = points[i].transform.position;

            switch (currentAttractor)
            {
                case Attractor.LORENZ:
                    {
                        points[i].transform.position = LorenzUpdate(pos);
                        break;
                    }
                case Attractor.AIZAWA:
                    {
                        points[i].transform.position = AizawaUpdate(pos);
                        break;
                    }
                case Attractor.HALVORSEN:
                    {
                        points[i].transform.position = HalvorsenUpdate(pos);
                        break;
                    }
                case Attractor.DADRAS:
                    {
                        points[i].transform.position = DadrasUpdate(pos);
                        break;
                    }
                case Attractor.ROSSLER:
                    {
                        points[i].transform.position = RosslerUpdate(pos);
                        break;
                    }
                case Attractor.SPROTTLINZ:
                    {
                        points[i].transform.position = SprottLinzUpdate(pos);
                        break;
                    }
                default: break;
            }

            points[i].transform.position *= scaleFactor[currentAttractor];
        }
    }

    void ColorUpdate()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float hue = Mathf.Repeat(Time.time * colorCycleSpeed, 1f);
            Color color = Color.HSVToRGB(hue, 1f, 1f);
            points[i].GetComponent<TrailRenderer>().material.SetColor("_EmissionColor", color * 2f);
        }
    }

    void Start()
    {
        points = new GameObject[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new GameObject("Point");

            Transform ptTransform = points[i].transform;
            Vector3 offset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            ptTransform.position = Vector3.zero + offset;

            TrailRenderer trail = points[i].AddComponent<TrailRenderer>();
            trail.time = trailTime;
            trail.startWidth = 0.1f;
            trail.endWidth = 0.1f;
            trail.numCapVertices = 2;
            trail.material = mat;
        }

    }

    void Update()
    {
        AttractorUpdate();
        ColorUpdate();
    }
    
    
}
