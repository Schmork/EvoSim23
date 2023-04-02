using System;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
[Serializable]
public class Layer : ICloneable
{
    public ActivationFunction[] Functions;
    public float4[] Biases;
    public float4[] Weights;
    public float4[] Results;

    public Layer(int neuronCount, int inputLength, bool isInputLayer = false)
    {
        int i = neuronCount / 4;
        Functions = new ActivationFunction[isInputLayer ? 0 : neuronCount];
        Biases = new float4[i];
        Results = new float4[i];
        Weights = new float4[i * inputLength];

        if (!isInputLayer)
        {
            for (i = 0; i < Functions.Length; i++)
            {
                Functions[i] = RandomFunction();
            }
        }

        for (i = 0; i < Biases.Length; i++)
        {
            Biases[i] = RandomInitialValue();
        }

        for (i = 0; i < Weights.Length; i++)
        {
            Weights[i] = RandomInitialValue();
        }
    }

    public static ActivationFunction RandomFunction()
    {
        var functions = Enum.GetValues(typeof(ActivationFunction)) as ActivationFunction[];
        var randomIndex = Utility.Random.NextInt(functions.Length);
        return functions[randomIndex];
    }

    static float4 RandomInitialValue()
    {
        return new float4(Utility.Gauss(WorldData.GaussStd),
                          Utility.Gauss(WorldData.GaussStd),
                          Utility.Gauss(WorldData.GaussStd),
                          Utility.Gauss(WorldData.GaussStd));
    }

    [BurstCompile]
    public float4[] FeedForwardInput(float4[] input)
    {
        for (int i = 0; i < Biases.Length; i++)
        {
            Results[i] = Biases[i] + input[i];
        }
        return Results;
    }

    [BurstCompile]
    public float4[] FeedForward(float4[] input)
    {
        float sum;
        for (int i = 0; i < Biases.Length; i++)
        {
            var weightSum = Biases[i];
            for (int j = 0; j < input.Length; j++)
                weightSum += Weights[i * input.Length + j] * input[j];
            sum = math.csum(weightSum);
            Results[i].w = Activation.Evaluate(Functions[i + 0], sum);
            Results[i].x = Activation.Evaluate(Functions[i + 1], sum);
            Results[i].y = Activation.Evaluate(Functions[i + 2], sum);
            Results[i].z = Activation.Evaluate(Functions[i + 3], sum);
        }
        return Results;
    }

    Layer(float4[] results, float4[] weights, float4[] biases, ActivationFunction[] functions)
    {
        Results = results;
        Weights = weights;
        Biases = biases;
        Functions = functions;
    }

    public object Clone()
    {
        return new Layer(
               Results.Clone() as float4[],
               Weights.Clone() as float4[],
               Biases.Clone() as float4[],
               Functions.Clone() as ActivationFunction[]
               );
    }
}