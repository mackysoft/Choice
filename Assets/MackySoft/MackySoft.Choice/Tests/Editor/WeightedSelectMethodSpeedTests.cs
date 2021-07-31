using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine;

namespace MackySoft.Choice.Tests {
	public class WeightedSelectMethodSpeedTests {

		static readonly int[] k_Iterations = new int[] { 1, 10, 100, 1000, 10000 };

		//[Test]
		public void CompareSpeedAllMethods ([Values(1,10,100,1000,10000)] int count) {
			var source = ItemEnumerableGenerator.GenerateEnumerable(count).ToArray();

			float[] values = new float[k_Iterations.Max()];
			for (int i = 0;values.Length > i;i++) {
				values[i] = Random.value;
			}

			Stopwatch stopwatch = new Stopwatch();
			foreach (IWeightedSelectMethod method in AllWeightedSelectMethods()) {
				for (int i = 0;k_Iterations.Length > i;i++) {
					int iteration = k_Iterations[i];

					stopwatch.Start();

					var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,method);

					for (int k = 0;iteration > k;k++) {
						weightedSelector.SelectItem(values[k]);
					}

					stopwatch.Stop();

					UnityEngine.Debug.Log($"{method.GetType()}: Count {count}, Iteration {iteration}, Speed {stopwatch.Elapsed.TotalMilliseconds}ms");
					stopwatch.Reset();
				}
			}

			Assert.Pass();
		}

		static IEnumerable<IWeightedSelectMethod> AllWeightedSelectMethods () {
			yield return WeightedSelectMethod.Linear;
			yield return WeightedSelectMethod.Binary;
			yield return WeightedSelectMethod.Alias;
		}

	}
}