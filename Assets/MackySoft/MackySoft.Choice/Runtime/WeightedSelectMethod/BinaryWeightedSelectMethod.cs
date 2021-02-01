using System;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice {

	/// <summary>
	/// <para> The binary search algorithm that is faster than linear scan by preprocessing to store the current sum of weights. </para>
	/// <para> It has an additional storage cost of O(n),  </para>
	/// <para> but is accelerated by up to O(log(n)) for each selection, </para>
	/// <para> where n is number of weights. </para>
	/// </summary>
	internal sealed class BinaryWeightedSelectMethod : IWeightedSelectMethod {

		public static readonly BinaryWeightedSelectMethod Instance = new BinaryWeightedSelectMethod();

		public int SelectIndex (TemporaryArray<float> weights,float value) {
			CumulativeSum(weights,out var runningTotals,out var indicies);
			
			using (runningTotals)
			using (indicies) {
				float targetDistance = value * runningTotals[runningTotals.Length - 1];
				int low = 0;
				int high = runningTotals.Length;

				while (low < high) {
					int mid = (int)Math.Floor((low + high) / 2f);
					float distance = runningTotals[mid];
					if (distance < targetDistance) {
						low = mid + 1;
					} else if (distance > targetDistance) {
						high = mid;
					} else {
						return indicies[mid];
					}
				}

				return indicies[low];
			}
		}

		static void CumulativeSum (TemporaryArray<float> weights,out TemporaryArray<float> runningTotals,out TemporaryArray<int> indicies) {
			runningTotals = TemporaryArray<float>.CreateAsList(weights.Length);
			indicies = TemporaryArray<int>.Create(weights.Length);
			float sum = 0f;
			int nonZeroIteration = 0;
			for (int i = 0;i < indicies.Length;i++) {
				float weight = weights[i];
				if (weight <= 0f) {
					continue;
				}
				sum += weight;
				runningTotals.Add(sum);
				indicies[nonZeroIteration] = i;
				nonZeroIteration++;
			}
		}

	}
}