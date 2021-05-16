using System;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice {

	/// <summary>
	/// <para> The binary search algorithm that is faster than linear scan by preprocessing to store the current sum of weights. </para>
	/// <para> It has an additional storage cost of O(n),  </para>
	/// <para> but is accelerated by up to O(log(n)) for each selection, </para>
	/// <para> where n is number of weights. </para>
	/// </summary>
	internal sealed class BinaryWeightedSelectMethod : IWeightedSelectMethod, IDisposable {

		TemporaryArray<float> m_RunningTotals;
		TemporaryArray<int> m_Indicies;

		~BinaryWeightedSelectMethod () {
			Dispose();
		}

		public int SelectIndex (TemporaryArray<float> weights,float value) {
			float targetDistance = m_RunningTotals[m_RunningTotals.Length - 1] * value;
			int low = 0;
			int high = m_RunningTotals.Length;

			while (low < high) {
				int mid = (int)Math.Floor((low + high) / 2f);
				float distance = m_RunningTotals[mid];
				if (distance < targetDistance) {
					low = mid + 1;
				} else if (distance > targetDistance) {
					high = mid;
				} else {
					return m_Indicies[mid];
				}
			}

			return m_Indicies[low];
		}

		public void Calculate (TemporaryArray<float> weights) {
			m_RunningTotals.Dispose();
			m_Indicies.Dispose();
			m_RunningTotals = TemporaryArray<float>.CreateAsList(weights.Length);
			m_Indicies = TemporaryArray<int>.Create(weights.Length);

			float sum = 0f;
			int naturalNumberIteration = 0;
			for (int i = 0;i < m_Indicies.Length;i++) {
				float weight = weights[i];
				if (weight <= 0f) {
					continue;
				}
				sum += weight;
				m_RunningTotals.Add(sum);
				m_Indicies[naturalNumberIteration] = i;
				naturalNumberIteration++;
			}
		}

		#region IDisposable Support

		bool m_IsDisposed;

		public void Dispose () {
			if (!m_IsDisposed) {
				m_RunningTotals.Dispose();
				m_Indicies.Dispose();

				m_IsDisposed = true;
			}
		}

		#endregion

	}
}