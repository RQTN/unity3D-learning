using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParticleSea : MonoBehaviour {
    public ParticleSystem particleSystem;
    public Camera camera;

    private ParticleSystem.Particle[] particles;
    public int particleNum = 300;

    private float[] particleAngle;          // 每个粒子的角度
    private float[] particleR;              // 每个粒子的半径

    private float minRadius = 8.0f;
    private float maxRadius = 10.0f;


    void Start()
    {
        particleAngle = new float[particleNum];
        particleR = new float[particleNum];
        particles = new ParticleSystem.Particle[particleNum];
        particleSystem.maxParticles = particleNum;
        particleSystem.Emit(particleNum);
        particleSystem.GetParticles(particles);

        for(int i = 0; i < particleNum; ++i)
        {
            float midRadius = (minRadius + maxRadius) / 2;
            float lhs = Random.Range(minRadius, midRadius);
            float rhs = Random.Range(midRadius, maxRadius);
            float r = Random.Range(lhs, rhs);
            float angle = Random.Range(0.0f, 360.0f);

            particleR[i] = r;
            particleAngle[i] = angle;
        }
    }


    void Update()
    {
        for(int i = 0; i < particleNum; ++i)
        {
            if (i % 2 == 0) 
            {
                particleAngle[i] += (i % 5 + 1) * 0.1f;
            }
            else
            {
                particleAngle[i] -= (i % 5 + 1) * 0.1f;
            }
            particleAngle[i] = particleAngle[i] % 360;
            float rad = particleAngle[i] / 180 * Mathf.PI;
            particles[i].position = new Vector3(particleR[i] * Mathf.Cos(rad), 
                particleR[i] * Mathf.Sin(rad), 0.0f);
        }

        particleSystem.SetParticles(particles, particleNum);
    }

}
