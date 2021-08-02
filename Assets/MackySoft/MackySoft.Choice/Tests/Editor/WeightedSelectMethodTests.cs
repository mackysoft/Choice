using System.Linq;
using UnityEngine;
using NUnit.Framework;

namespace MackySoft.Choice.Tests {
	public class WeightedSelectMethodTests {

		[Test]
		public void Linear_ReturnValidValue ([Random(0f,1f,10)] float value) {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Linear);
			Assert.IsNotNull(weightedSelector.SelectItem(value));
		}

		[Test, Repeat(100)]
		public void Linear_ReturnValidValue_0 () {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Linear);
			Assert.AreSame(source.FirstOrDefault(x => x.weight > 0f).item,weightedSelector.SelectItem(0f));
		}

		[Test, Repeat(100)]
		public void Linear_ReturnValidValue_1 () {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Linear);
			Assert.AreSame(source.LastOrDefault(x => x.weight > 0f).item,weightedSelector.SelectItem(1f));
		}

		[Test]
		public void Binary_ReturnValidValue ([Random(0f,1f,10)] float value) {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Binary);
			Assert.IsNotNull(weightedSelector.SelectItem(value));
		}

		[Test, Repeat(100)]
		public void Binary_ReturnValidValue_0 () {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Binary);
			Assert.AreSame(weightedSelector.FirstOrDefault(p => p.Value > 0f).Key,weightedSelector.SelectItem(0f));
		}

		[Test, Repeat(100)]
		public void Binary_ReturnValidValue_1 () {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Binary);
			Assert.AreSame(source.LastOrDefault(x => x.weight > 0f).item,weightedSelector.SelectItem(1f));
		}

		[Test]
		public void Alias_ReturnValidValue ([Random(0f,1f,10)] float value) {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Alias);
			Assert.IsNotNull(weightedSelector.SelectItem(value));
		}

		[Test, Repeat(100)]
		public void Alias_ReturnValidValue_0 () {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Alias);
			Assert.AreSame(weightedSelector.FirstOrDefault(p => p.Value > 0f).Key,weightedSelector.SelectItem(0f));
		}

		[Test, Repeat(100)]
		public void Alias_ReturnValidValue_1 () {
			var source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();
			var weightedSelector = source.ToWeightedSelector(x => x.item,x => x.weight,WeightedSelectMethod.Alias);
			Assert.AreSame(source.LastOrDefault(x => x.weight > 0f).item,weightedSelector.SelectItem(1f));
		}

	}
}