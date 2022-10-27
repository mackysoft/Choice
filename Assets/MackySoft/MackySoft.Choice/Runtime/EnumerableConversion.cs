using System;
using System.Collections.Generic;

namespace MackySoft.Choice.Internal {

	public static class EnumerableConversion {

		internal static class IdentityFunction<T> {
			public static readonly Func<T,T> Instance = x => x;
		}

		#region IEnumerable<TItem>

		public static void EnumerableToTemporaryArray<TItem> (
			IEnumerable<TItem> source,
			Func<TItem,float> weightSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			if (source is IReadOnlyList<TItem> readonlyList) {
				ReadOnlyListToTemporaryArrayInternal(readonlyList,IdentityFunction<TItem>.Instance,weightSelector,out items,out weights);
			}
			else if (source is IList<TItem> list) {
				ListToTemporaryArrayInternal(list,IdentityFunction<TItem>.Instance,weightSelector,out items,out weights);
			}
			else {
				EnumerableToTemporaryArrayInternal(source,weightSelector,out items,out weights);
			}
		}

		static void EnumerableToTemporaryArrayInternal<TItem> (
			IEnumerable<TItem> source,
			Func<TItem,float> weightSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			items = TemporaryArray<TItem>.From(source);

			int count = items.Length;
			weights = TemporaryArray<float>.Create(count);
			for (int i = 0;count > i;i++) {
				weights[i] = weightSelector(items[i]);
			}
		}

		#endregion

		#region IEnumerable<KeyValuePair<TItem,float>>

		public static void DictionaryToTemporaryArray<TItem> (
			IEnumerable<KeyValuePair<TItem,float>> source,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			DictionaryToTemporaryArray(source,IdentityFunction<TItem>.Instance,out items,out weights);
		}

		#endregion

		#region IEnumerable<TSource>

		public static void EnumerableToTemporaryArray<TSource, TItem> (
			IEnumerable<TSource> source,
			Func<TSource,TItem> itemSelector,
			Func<TSource,float> weightSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			if (source is IReadOnlyList<TSource> readonlyList) {
				ReadOnlyListToTemporaryArrayInternal(readonlyList,itemSelector,weightSelector,out items,out weights);
			}
			else if (source is IList<TSource> list) {
				ListToTemporaryArrayInternal(list,itemSelector,weightSelector,out items,out weights);
			}
			else {
				EnumerableToTemporaryArrayInternal(source,itemSelector,weightSelector,out items,out weights);
			}
		}

		static void ListToTemporaryArrayInternal<TSource, TItem> (
			IList<TSource> source,
			Func<TSource,TItem> itemSelector,
			Func<TSource,float> weightSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			int count = source.Count;
			items = TemporaryArray<TItem>.Create(count);
			weights = TemporaryArray<float>.Create(count);
			for (int i = 0;count > i;i++) {
				TSource element = source[i];
				items[i] = itemSelector(element);
				weights[i] = weightSelector(element);
			}
		}

		static void ReadOnlyListToTemporaryArrayInternal<TSource, TItem> (
			IReadOnlyList<TSource> source,
			Func<TSource,TItem> itemSelector,
			Func<TSource,float> weightSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			int count = source.Count;
			items = TemporaryArray<TItem>.Create(count);
			weights = TemporaryArray<float>.Create(count);
			for (int i = 0;count > i;i++) {
				TSource element = source[i];
				items[i] = itemSelector(element);
				weights[i] = weightSelector(element);
			}
		}

		static void EnumerableToTemporaryArrayInternal<TSource, TItem> (
			IEnumerable<TSource> source,
			Func<TSource,TItem> itemSelector,
			Func<TSource,float> weightSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			using var sourceArray = TemporaryArray<TSource>.From(source);

			int count = sourceArray.Length;
			items = TemporaryArray<TItem>.Create(count);
			weights = TemporaryArray<float>.Create(count);
			for (int i = 0;count > i;i++) {
				TSource element = sourceArray[i];
				items[i] = itemSelector(element);
				weights[i] = weightSelector(element);
			}
		}

		#endregion

		#region IEnumerable<KeyValuePair<TSource,float>>

		public static void DictionaryToTemporaryArray<TSource, TItem> (
			IEnumerable<KeyValuePair<TSource,float>> source,
			Func<TSource,TItem> itemSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			if (source is IReadOnlyDictionary<TSource,float> readonlyDictionary) {
				ReadOnlyDictionaryToTemporaryArrayInternal(readonlyDictionary,itemSelector,out items,out weights);
			}
			else if (source is IDictionary<TSource,float> dictionary) {
				DictionaryToTemporaryArrayInternal(dictionary,itemSelector,out items,out weights);
			}
			else {
				EnumerableToTemporaryArrayInternal(source,itemSelector,out items,out weights);
			}
		}

		static void ReadOnlyDictionaryToTemporaryArrayInternal<TSource, TItem> (
			IReadOnlyDictionary<TSource,float> source,
			Func<TSource,TItem> itemSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			int count = source.Count;
			items = TemporaryArray<TItem>.Create(count);
			weights = TemporaryArray<float>.Create(count);

			int i = 0;
			foreach (var pair in source) {
				items[i] = itemSelector(pair.Key);
				weights[i] = pair.Value;
				i++;
			}
		}

		static void DictionaryToTemporaryArrayInternal<TSource, TItem> (
			IDictionary<TSource,float> source,
			Func<TSource,TItem> itemSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			int count = source.Count;
			items = TemporaryArray<TItem>.Create(count);
			weights = TemporaryArray<float>.Create(count);

			int i = 0;
			foreach (var pair in source) {
				items[i] = itemSelector(pair.Key);
				weights[i] = pair.Value;
				i++;
			}
		}

		static void EnumerableToTemporaryArrayInternal<TSource, TItem> (
			IEnumerable<KeyValuePair<TSource,float>> source,
			Func<TSource,TItem> itemSelector,
			out TemporaryArray<TItem> items,
			out TemporaryArray<float> weights
		) {
			items = TemporaryArray<TItem>.CreateAsList(8);
			weights = TemporaryArray<float>.CreateAsList(8);

			int count = 0;
			foreach (var pair in source) {
				items.Add(itemSelector(pair.Key));
				weights.Add(pair.Value);
				count++;
			}
		}

		#endregion

	}
}