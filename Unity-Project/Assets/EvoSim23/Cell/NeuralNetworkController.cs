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

    const float sensorFee = 0;
    const float brainFee = 0;

    void OnEnable()
    {
        inputs = new float4[NeuralNetwork.numInputs / 4];
        actions = new float4[1];
        sensorData = new float4(0);
        lastSensorUse = 0;
        lastBrainUse = 0;
    }

    [BurstCompile]
    private void LateUpdate()
    {
        lastSensorUse += Time.deltaTime;
        lastBrainUse += Time.deltaTime;

        int n = 0;
        for (; n < neuralNetwork.Memory.Length; n++)
        {
            var layerIndex = neuralNetwork.Memory[n].x;
            var neurnIndex = neuralNetwork.Memory[n].y;
            inputs[n] = neuralNetwork.Layers[layerIndex].Results[neurnIndex];
        }

        inputs[n++] = new float4(
            cc.Size / 100f,
            cc.Rb.velocity.magnitude / 10f,
            Vector2.SignedAngle(cc.Rb.velocity, transform.up) / 180f,
            System.MathF.Tanh(cc.Rb.angularVelocity / 900f)
        );

        if (lastSensorUse > actions[0].y)
        {
            sensorData = cc.Sensors.Scan();
            lastSensorUse = 0;
            cc.Size -= sensorFee;
        }
        inputs[n++] = sensorData;

        if (lastBrainUse > actions[0].z)
        {
            actions = neuralNetwork.FeedForward(inputs);
            actions[0].w = math.clamp(actions[0].w, 0, 1);
            actions[0].x = math.clamp(actions[0].x, -1, 1);

            lastBrainUse = 0;
            cc.Size -= brainFee;

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

        var force = Time.deltaTime * 10f / math.sqrt(1 + cc.Size);
        var thrust = actions[0].w * force * 20f;
        var torque = actions[0].x * force;
        cc.Rb.AddForce(thrust * transform.up);
        cc.Rb.AddTorque(torque);

        //if (actions[1].w > 0)
        //{
        //    var childSize = cc.Size * 0.4f;
        //    var pos = transform.position - transform.up * SizeController.ToScale(cc.Size) * 1.8f;
        //    if (null != Physics2D.OverlapCircle(pos, SizeController.ToScale(childSize * 1.2f))) return;

        //    var child = cc.Pool.Get(); 
        //    child.transform.localEulerAngles = new Vector3(0f, 0f, transform.localEulerAngles.z + 180f); // flip
        //    child.transform.parent = transform.parent;
        //    child.transform.position = pos;
        //    child.Pool = cc.Pool;
        //    child.Size = childSize;
        //    child.NeuralNetwork = cc.NeuralNetwork.Clone() as NeuralNetwork;
        //    child.NeuralNetwork.Mutate(Utility.Gauss(cc.WorldData.Gauss));
        //    child.Renderer.color = cc.Renderer.color;
        //    cc.Size -= childSize * 0.1f;
        //}
    }
}
