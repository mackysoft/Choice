using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice.Tests {
	public class EnumerableConversionTests {

		class Item {
			public float weight;
		}
		
		[Test]
		public void EnumerableToTemporaryArray () {
			Item[] source = GenerateItemEnumerable().ToArray();

			EnumerableConversion.EnumerableToTemporaryArray(source.Select(x => x),x => x.weight,out var items,out var weights);
		
			for (int i = 0;source.Length > i;i++) {
				var element = source[i];
				Assert.AreEqual(element,items[i]);
				Assert.AreEqual(element.weight,weights[i]);
			}

			items.Dispose();
			weights.Dispose();
		}

		[Test]
		public void ReadOnlyListToTemporaryArray () {
			Item[] source = GenerateItemEnumerable().ToArray();

			EnumerableConversion.EnumerableToTemporaryArray(source,x => x.weight,out var items,out var weights);

			for (int i = 0;source.Length > i;i++) {
				var element = source[i];
				Assert.AreEqual(element,items[i]);
				Assert.AreEqual(element.weight,weights[i]);
			}

			items.Dispose();
			weights.Dispose();
		}

		[Test]
		public void ListToTemporaryArray () {
			List<Item> source = GenerateItemEnumerable().ToList();

			EnumerableConversion.EnumerableToTemporaryArray(source,x => x.weight,out var items,out var weights);

			for (int i = 0;source.Count > i;i++) {
				var element = source[i];
				Assert.AreEqual(element,items[i]);
				Assert.AreEqual(element.weight,weights[i]);
			}

			items.Dispose();
			weights.Dispose();
		}

		[Test]
		public void DictionaryToTemporaryArray () {
			Dictionary<Item,float> source = GenerateDictionary().ToDictionary(p => p.Key,p => p.Value);

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
			Dictionary<Item,float> source = GenerateDictionary().ToDictionary(p => p.Key,p => p.Value);

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

		static IEnumerable<Item> GenerateItemEnumerable () {
			for (int i = 0;100 > i;i++) {
				yield return new Item { weight = Random.value };
			}
		}

		static IEnumerable<KeyValuePair<Item,float>> GenerateDictionary () {
			for (int i = 0;100 > i;i++) {
				yield return new KeyValuePair<Item,float>(new Item(),Random.value);
			}
		}

	}
}