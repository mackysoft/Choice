using System;
using System.Collections.Generic;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice {

	public interface IReadOnlyWeightedSelector<T> : IReadOnlyDictionary<T,float> {

		/// <summary>
		/// Returns a selected item from weights.
		/// </summary>
		/// <param name="value">
		///	<para> Value for selecting an index. </para>
		///	<para> The value must be between 0.0f and 1.0f. </para>
		/// </param>
		T SelectItem (float value);

	}

	public interface IWeightedSelector<T> : IReadOnlyWeightedSelector<T>, IDictionary<T,float> {

	}

	public static class WeightedSelectorExtensions {

		/// <summary>
		/// Returns a random selected item from weights using <see cref="UnityEngine.Random.value"/>.
		/// </summary>
		public static T SelectItemWithUnityRandom<T> (this IReadOnlyWeightedSelector<T> weightedSelector) {
			if (weightedSelector == null) {
				throw new ArgumentNullException(nameof(weightedSelector));
			}
			return weightedSelector.SelectItem(UnityEngine.Random.value);
		}

		#region ToWeightedSelector<TItem>

		public static IWeightedSelector<TItem> ToWeightedSelector<TItem> (this IEnumerable<TItem> source,Func<TItem,float> weightSelector) {
			return ToWeightedSelector(source,weightSelector,WeightedSelectMethod.Linear);
		}

		public static IWeightedSelector<TItem> ToWeightedSelector<TItem> (this IEnumerable<TItem> source,Func<TItem,float> weightSelector,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (weightSelector == null) {
				throw new ArgumentNullException(nameof(weightSelector));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}
			return new WeightedSelector<TItem>(source,weightSelector,method);
		}

		public static IWeightedSelector<TItem> ToWeightedSelector<TItem> (this IEnumerable<KeyValuePair<TItem,float>> source) {
			return ToWeightedSelector(source,WeightedSelectMethod.Linear);
		}

		public static IWeightedSelector<TItem> ToWeightedSelector<TItem> (this IEnumerable<KeyValuePair<TItem,float>> source,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}
			return new WeightedSelector<TItem>(source,method);
		}

		#endregion

		#region ToWeightedSelector<TItem,TSource>

		public static IWeightedSelector<TItem> ToWeightedSelector<TSource, TItem> (this IEnumerable<TSource> source,Func<TSource,TItem> itemSelector,Func<TSource,float> weightSelector) {
			return ToWeightedSelector(source,itemSelector,weightSelector,WeightedSelectMethod.Linear);
		}

		public static IWeightedSelector<TItem> ToWeightedSelector<TSource, TItem> (this IEnumerable<TSource> source,Func<TSource,TItem> itemSelector,Func<TSource,float> weightSelector,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (itemSelector == null) {
				throw new ArgumentNullException(nameof(itemSelector));
			}
			if (weightSelector == null) {
				throw new ArgumentNullException(nameof(weightSelector));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}

			EnumerableConversion.EnumerableToTemporaryArray(source,itemSelector,weightSelector,out var items,out var weights);
			return new WeightedSelector<TItem>(items,weights,method);
		}

		public static IWeightedSelector<TItem> ToWeightedSelector<TSource, TItem> (this IEnumerable<KeyValuePair<TSource,float>> source,Func<TSource,TItem> itemSelector) {
			return ToWeightedSelector(source,itemSelector,WeightedSelectMethod.Linear);
		}

		public static IWeightedSelector<TItem> ToWeightedSelector<TSource, TItem> (this IEnumerable<KeyValuePair<TSource,float>> source,Func<TSource,TItem> itemSelector,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (itemSelector == null) {
				throw new ArgumentNullException(nameof(itemSelector));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}

			EnumerableConversion.DictionaryToTemporaryArray(source,itemSelector,out var items,out var weights);
			return new WeightedSelector<TItem>(items,weights,method);
		}

		#endregion

	}

}