using System;
using System.Runtime.CompilerServices;

namespace MackySoft.Choice {

	/// <summary>
	/// <para> The simplest algorithm that walks linearly along the weights. </para>
	/// <para> This method is an O(n) operation, where n is number of weights. </para>
	/// </summary>
	internal sealed class LinearWeightedSelectMethod : IWeightedSelectMethod {

		public static readonly LinearWeightedSelectMethod Instance = new LinearWeightedSelectMethod();
		
		public int SelectIndex (float[] weights,float value) {
			if (weights == null) {
				throw new ArgumentNullException(nameof(weights));
			}

			float remainingDistance = value * Sum(weights);
			for (int i = 0;i < weights.Length;i++) {
				float weight = weights[i];
				remainingDistance -= weight;
				if (remainingDistance < 0f) {
					return i;
				}
			}
			return -1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static float Sum (float[] values) {
			float result = 0f;
			for (int i = 0;i < values.Length;i++) {
				result += values[i];
			}
			return result;
		}

	}
}