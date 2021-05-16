using System.Runtime.CompilerServices;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice {

	/// <summary>
	/// <para> The simplest algorithm that walks linearly along the weights. </para>
	/// <para> This method is an O(n) operation, where n is number of weights. </para>
	/// </summary>
	internal sealed class LinearWeightedSelectMethod : IWeightedSelectMethod {

		float m_TotalDistance;
		
		public int SelectIndex (TemporaryArray<float> weights,float value) {
			float remainingDistance = value * m_TotalDistance;
			for (int i = 0;i < weights.Length;i++) {
				float weight = weights[i];
				if (weight <= 0f) {
					continue;
				}

				remainingDistance -= weight;
				if (remainingDistance <= 0f) {
					return i;
				}
			}

			return -1;
		}

		public void Calculate (TemporaryArray<float> weights) {
			m_TotalDistance = Sum(weights);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static float Sum (TemporaryArray<float> weights) {
			float result = 0f;
			for (int i = 0;i < weights.Length;i++) {
				result += weights[i];
			}
			return result;
		}
		
	}
}