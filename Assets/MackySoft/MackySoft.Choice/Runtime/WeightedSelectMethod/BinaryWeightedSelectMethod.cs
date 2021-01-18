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

		public int SelectIndex (float[] weights,float value) {
			if (weights == null) {
				throw new ArgumentNullException(nameof(weights));
			}

			using TemporaryArray<float> runningTotals = CumulativeSum(weights);

			float targetDistance = value * runningTotals[runningTotals.Length - 1];
			int low = 0;
			int high = weights.Length;

			while (low < high) {
				int mid = (int)Math.Round((low + high) / 2f);
				float distance = runningTotals[mid];
				if (distance < targetDistance) {
					low = mid + 1;
				} else if (distance > targetDistance) {
					high = mid;
				} else {
					return mid;
				}
			}

			return low;
		}

		static TemporaryArray<float> CumulativeSum (float[] values) {
			var results = TemporaryArray<float>.Create(values.Length);
			float sum = 0f;
			for (int i = 0;i < results.Length;i++) {
				sum += results[i];
				results[i] = sum;
			}
			return results;
		}

	}
}