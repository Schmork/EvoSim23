using System;
using System.Linq;
using NUnit.Framework;

public class UtilityTests
{
    //[Test]
    //public void Gauss_NormalDistributed()
    //{
    //    var numbers = new float[1000];
    //    for (int i = 0; i < numbers.Length; i++)
    //    {
    //        numbers[i] = Utility.Gauss();
    //    }
    //    var isNormal = IsNormal(numbers);
    //    Assert.IsTrue(isNormal);
    //}

    //public static bool IsNormal(float[] data, double levelOfSignificance = 0.05)
    //{
    //    int n = data.Length;
    //    double mu = data.Average();
    //    double sigma = Math.Sqrt(data.Sum(x => (x - mu) * (x - mu)) / (n - 1));

    //    float[] sortedData = data.OrderBy(x => x).ToArray();
    //    double D = double.MinValue;

    //    for (int i = 0; i < n; i++)
    //    {
    //        double Fn = (double)(i + 1) / n;
    //        double F = GaussianCDF(mu, sigma, sortedData[i]);
    //        double d = Math.Abs(Fn - F);
    //        if (d > D) D = d;
    //    }

    //    double alpha = levelOfSignificance;
    //    double ks = Math.Sqrt(-0.5 * Math.Log(alpha / 2));
    //    double ksThreshold = ks / Math.Sqrt(n);

    //    return D <= ksThreshold;
    //}

    //private static double GaussianCDF(double mu, double sigma, double x)
    //{
    //    return 0.5 * (1 + Erf((x - mu) / (sigma * Math.Sqrt(2))));
    //}

    //private static double Erf(double x)
    //{
    //    double a1 = 0.254829592;
    //    double a2 = -0.284496736;
    //    double a3 = 1.421413741;
    //    double a4 = -1.453152027;
    //    double a5 = 1.061405429;
    //    double p = 0.3275911;

    //    int sign = Math.Sign(x);
    //    x = Math.Abs(x);

    //    double t = 1.0 / (1.0 + p * x);
    //    double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

    //    return sign * y;
    //}
}
