using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public class NeuralNetworkController : MonoBehaviour
{
    [SerializeField] CellController cc;
    public NeuralNetwork neuralNetwork;

    float4[] actions;
    float4[] inputs;
    float4 sensorData;

    float lastSensorUse;
    float lastBrainUse;

    void OnEnable()
    {
        inputs = new float4[NeuralNetwork.numInputs / 4];
        actions = new float4[2];
    }

    [BurstCompile]
    private void Update()
    {
        lastSensorUse += Time.deltaTime;
        lastBrainUse += Time.deltaTime;


        int i;
        int n = 0;
        for (i = 0; i < neuralNetwork.Memory.Length; i++)
        {
            inputs[n] = neuralNetwork.Layers[neuralNetwork.Memory[i].x].Memory[neuralNetwork.Memory[i].y];
            if (i / 4 > 0 && i % 4 == 0) n++;
        }

        //inputs[n++] = new float4(
        //    cc.Size / 100f,
        //    cc.Rb.velocity.magnitude / 10f,
        //    Vector2.Dot(cc.Rb.velocity, transform.up),
        //    System.MathF.Tanh(cc.Rb.angularVelocity / 900f)
        //    );

        if (lastSensorUse > actions[0].y)
        {
            sensorData = cc.Sensors.Scan();
            lastSensorUse = 0;
        }

        inputs[n++] = sensorData;

        if (lastBrainUse > actions[0].z)
        {
            actions = neuralNetwork.FeedForward(inputs);
            actions[0].w = math.clamp(actions[0].w, 0, 1);
            actions[0].x = math.clamp(actions[0].x, -1, 1);

            lastBrainUse = 0;

            /*
            ActiveBrain = Brains[actions[1] switch
            {
                _ when actions[1].w >= actions[1].x && actions[1].w >= actions[1].y && actions[1].w >= actions[1].z => 0,
                _ when actions[1].x >= actions[1].y && actions[1].x >= actions[1].z => 1,
                _ when actions[1].y >= actions[1].z => 2,
                _ => 3
            }];
            */
        }

        var sizePow = math.pow(cc.Size + 1, 2f);
        var powTime = Time.deltaTime / sizePow;
        var thrust = actions[0].w * 500f * powTime;
        var torque = actions[0].x * 500f * powTime;
        cc.Rb.AddForce(thrust * transform.up);
        cc.Rb.AddTorque(torque);
    }
}
