using System;
using System.Runtime.CompilerServices;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice {

	/// <summary>
	/// <para> The fastest algorithm. </para>
	/// <para> It takes O(n) run time to set up, but the selection is performed in O(1) run time, </para>
	/// <para> where n is number of weights. </para>
	/// <para> Therefore, this is a very effective algorithm for selecting multiple items. </para>
	/// </summary>
	internal sealed class AliasWeightedSelectMethod : IWeightedSelectMethod, IDisposable {

		struct Alias {

			public int index;
			public float probability;

			public Alias (int index,float probability) {
				this.index = index;
				this.probability = probability;
			}

			public override string ToString () {
				return $"{{ [{index}] : {probability} }}";
			}

		}

		TemporaryArray<Alias> m_Aliases;

		~AliasWeightedSelectMethod () {
			Dispose();
		}

		public int SelectIndex (TemporaryArray<float> weights,float value) {
			if (value == 1f) {
				return weights.Length - 1;
			}
			float r = value * weights.Length;
			int i = (int)Math.Floor(r);
			Alias alias = m_Aliases[i];
			return ((r - i) > alias.probability) ? alias.index : i;
		}

		public void Calculate (TemporaryArray<float> weights) {
			int size = weights.Length;

			m_Aliases = TemporaryArray<Alias>.Create(size);
			for (int i = 0;i < size;i++) {
				m_Aliases[i] = new Alias(-1,1f);
			}

			if (size == 0) {
				return;
			}
			if (size == 1) {
				m_Aliases[0] = new Alias(0,1f);
				return;
			}

			using (var smalls = TemporaryArray<Alias>.CreateAsList(size))
			using (var bigs = TemporaryArray<Alias>.CreateAsList(size)) {
				float average = Sum(weights) / size;
				for (int i = 0;i < size;i++) {
					float weight = weights[i];
					Alias alias = new Alias(i,weight / average);
					if (weight < average) {
						smalls.Add(alias);
					} else {
						bigs.Add(alias);
					}
				}

				if ((smalls.Length == 0) || (bigs.Length == 0)) {
					return;
				}

				int si = 0, bi = 0;
				Alias? small = smalls[0];
				Alias? big = bigs[0];
				while (small.HasValue && big.HasValue) {
					m_Aliases[small.Value.index] = new Alias(big.Value.index,small.Value.probability);
					big = new Alias(big.Value.index,big.Value.probability - (1f - small.Value.probability));

					if (big.Value.probability < 1f) {
						small = big;
						bi++;
						big = (bi < bigs.Length) ? (Alias?)bigs[bi] : null;
					} else {
						si++;
						small = (si < smalls.Length) ? (Alias?)smalls[si] : null;
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static float Sum (TemporaryArray<float> weights) {
			float result = 0f;
			for (int i = 0;i < weights.Length;i++) {
				result += weights[i];
			}
			return result;
		}

		#region IDisposable Support

		bool m_IsDisposed;

		public void Dispose () {
			if (!m_IsDisposed) {
				m_Aliases.Dispose();

				m_IsDisposed = true;
			}
		}

		#endregion

	}
}