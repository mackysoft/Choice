using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using MackySoft.Choice.Tests;

namespace MackySoft.Choice.Internal.Tests {

	public class EnumerableConversionTests {

		[Test]
		public void EnumerableToTemporaryArray () {
			ItemEntry[] source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();

			EnumerableConversion.EnumerableToTemporaryArray(source.Select(x => x),x => x.item,x => x.weight,out var items,out var weights);
		
			for (int i = 0;source.Length > i;i++) {
				var element = source[i];
				Assert.AreEqual(element.item,items[i]);
				Assert.AreEqual(element.weight,weights[i]);
			}

			items.Dispose();
			weights.Dispose();
		}

		[Test]
		public void ReadOnlyListToTemporaryArray () {
			ItemEntry[] source = ItemEnumerableGenerator.GenerateEnumerable(100).ToArray();

			EnumerableConversion.EnumerableToTemporaryArray(source,x => x.item,x => x.weight,out var items,out var weights);

			for (int i = 0;source.Length > i;i++) {
				var element = source[i];
				Assert.AreEqual(element.item,items[i]);
				Assert.AreEqual(element.weight,weights[i]);
			}

			items.Dispose();
			weights.Dispose();
		}

		[Test]
		public void ListToTemporaryArray () {
			List<ItemEntry> source = ItemEnumerableGenerator.GenerateEnumerable(100).ToList();

			EnumerableConversion.EnumerableToTemporaryArray(source,x => x.item,x => x.weight,out var items,out var weights);

			for (int i = 0;source.Count > i;i++) {
				var element = source[i];
				Assert.AreEqual(element.item,items[i]);
				Assert.AreEqual(element.weight,weights[i]);
			}

			items.Dispose();
			weights.Dispose();
		}

		[Test]
		public void DictionaryToTemporaryArray () {
			Dictionary<Item,float> source = ItemEnumerableGenerator.GenerateDictionary(100).ToDictionary(p => p.Key,p => p.Value);

			EnumerableConversion.DictionaryToTemporaryArray(source,out var items,out var weights);

			int i = 0;
			foreach (var pair in source) {
				Assert.AreEqual(pair.Key,items[i]);
				Assert.AreEqual(pair.Value,weights[i]);
				i++;
			}

			items.Dispose();
			weights.Dispose();
		}

		[Test]
		public void DictionaryEnumerableToTemporaryArray () {
			Dictionary<Item,float> source = ItemEnumerableGenerator.GenerateDictionary(100).ToDictionary(p => p.Key,p => p.Value);

			EnumerableConversion.DictionaryToTemporaryArray(source.Select(x => x),out var items,out var weights);

			int i = 0;
			foreach (var pair in source) {
				Assert.AreEqual(pair.Key,items[i]);
				Assert.AreEqual(pair.Value,weights[i]);
				i++;
			}

			items.Dispose();
			weights.Dispose();
		}

	}
}