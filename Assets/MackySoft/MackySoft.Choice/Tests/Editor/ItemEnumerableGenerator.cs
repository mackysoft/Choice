using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.Choice.Tests {

	public class Item {

		public int id;

		public override string ToString () {
			return "Index: " + id.ToString();
		}

	}

	public struct ItemEntry {

		public Item item;
		public float weight;

		public override string ToString () {
			return $"{{ {item}: Weight {weight} }}";
		}

	}

	public class ItemEnumerableGenerator {

		public static IEnumerable<ItemEntry> GenerateEnumerable (int count) {
			for (int i = 0;count > i;i++) {
				yield return new ItemEntry {
					item = new Item { id = i },
					weight = Random.Range(0,10)
				};
			}
		}

		public static IEnumerable<KeyValuePair<Item,float>> GenerateDictionary (int count) {
			for (int i = 0;count > i;i++) {
				yield return new KeyValuePair<Item,float>(
					new Item { id = i },
					Random.Range(0,10)
				);
			}
		}

	}

}