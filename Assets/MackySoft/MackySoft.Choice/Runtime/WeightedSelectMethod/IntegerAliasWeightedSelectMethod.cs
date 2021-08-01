using System;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice {

	internal sealed class IntegerAliasWeightedSelectMethod : IWeightedSelectMethod, IDisposable {

		TemporaryArray<int> m_Indices;

		~IntegerAliasWeightedSelectMethod () {
			Dispose();
		}

		public int SelectIndex (TemporaryArray<float> weights,float value) {
			return ((m_Indices.Length != 0) && (value < 1f)) ? m_Indices[(int)(m_Indices.Length * value)] : (weights.Length - 1);
		}

		public void Calculate (TemporaryArray<float> weights) {
			if (weights.Length == 0) {
				return;
			}

			m_Indices = TemporaryArray<int>.CreateAsList(weights.Length * 8);
			for (var i = 0;i < weights.Length;i++) {
				int w = (int)weights[i];
				if (w < 1) {
					throw new ArgumentException();
				}
				else if (w > (int.MaxValue - m_Indices.Length)) {
					throw new ArgumentOutOfRangeException();
				}

				for (;w > 0;w--) {
					m_Indices.Add(i);
				}
			}
		}

		#region IDisposable Support

		bool m_IsDisposed;

		public void Dispose () {
			if (!m_IsDisposed) {
				m_Indices.Dispose();

				m_IsDisposed = true;
			}
		}

		#endregion

	}
}
