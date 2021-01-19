using System;
using System.Collections.Generic;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice {

	public interface IReadOnlyWeightedSelector<out T> {

		/// <summary>
		/// Returns a selected item from weights.
		/// </summary>
		/// <param name="value">
		///	<para> Value for selecting an index. </para>
		///	<para> The value must be between 0.0f and 1.0f. </para>
		/// </param>
		T SelectItem (float value);

	}

	public class WeightedSelector {

		public static IReadOnlyWeightedSelector<T> Empty<T> () => EmptyWeightedSelector<T>.Instance;

	}

	internal sealed class EmptyWeightedSelector<T> : IReadOnlyWeightedSelector<T> {

		public static EmptyWeightedSelector<T> Instance = new EmptyWeightedSelector<T>();

		public T SelectItem (float value) {
			return default;
		}

	}

	public static class WeightedSelectorExtensions {

		/// <summary>
		/// Returns a selected item from weights.
		/// </summary>
		public static T SelectItem<T> (this IReadOnlyWeightedSelector<T> weightedSelector) {
			if (weightedSelector == null) {
				throw new ArgumentNullException(nameof(weightedSelector));
			}
			return weightedSelector.SelectItem(UnityEngine.Random.value);
		}

		#region ToReadOnlyWeightedSelector<TItem>

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TItem> (this IEnumerable<TItem> source,Func<TItem,float> weightSelector) {
			return ToReadOnlyWeightedSelector(source,weightSelector,WeightedSelectMethod.Linear);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TItem> (this IEnumerable<TItem> source,Func<TItem,float> weightSelector,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (weightSelector == null) {
				throw new ArgumentNullException(nameof(weightSelector));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}

			if (source is IReadOnlyList<TItem> readonlyList) {
				return new ReadOnlyWeightedSelector<TItem>(readonlyList,weightSelector,method);
			}
			if (source is IList<TItem> list) {
				return new ReadOnlyWeightedSelector<TItem>(list,weightSelector,method);
			}
			return new ReadOnlyWeightedSelector<TItem>(source,weightSelector,method);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TItem> (this IDictionary<TItem,float> source) {
			return ToReadOnlyWeightedSelector(source,WeightedSelectMethod.Linear);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TItem> (this IDictionary<TItem,float> source,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}

			return new ReadOnlyWeightedSelector<TItem>(source,method);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TItem> (this IReadOnlyDictionary<TItem,float> source) {
			return ToReadOnlyWeightedSelector(source,WeightedSelectMethod.Linear);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TItem> (this IReadOnlyDictionary<TItem,float> source,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}

			return new ReadOnlyWeightedSelector<TItem>(source,method);
		}

		class ReadOnlyWeightedSelector<TItem> : IReadOnlyWeightedSelector<TItem>, IDisposable {

			TemporaryArray<TItem> m_Items;
			TemporaryArray<float> m_Weights;
			IWeightedSelectMethod m_Method;

			public ReadOnlyWeightedSelector (IEnumerable<TItem> source,Func<TItem,float> weightSelector,IWeightedSelectMethod method) {
				m_Method = method;

				m_Items = TemporaryArray<TItem>.From(source);

				int count = m_Items.Length;
				m_Weights = TemporaryArray<float>.Create(count);
				for (int i = 0;m_Items.Capacity > i;i++) {
					if (count > i) {
						m_Weights[i] = weightSelector.Invoke(m_Items[i]);
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
				}
			}

			public ReadOnlyWeightedSelector (IList<TItem> source,Func<TItem,float> weightSelector,IWeightedSelectMethod method) {
				m_Method = method;

				int count = source.Count;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				for (int i = 0;m_Weights.Capacity > i;i++) {
					if (count > i) {
						TItem item = source[i];
						m_Items[i] = item;
						m_Weights[i] = weightSelector.Invoke(item);
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
				}
			}

			public ReadOnlyWeightedSelector (IReadOnlyList<TItem> source,Func<TItem,float> weightSelector,IWeightedSelectMethod method) {
				m_Method = method;

				int count = source.Count;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				for (int i = 0;m_Weights.Capacity > i;i++) {
					if (count > i) {
						TItem item = source[i];
						m_Items[i] = item;
						m_Weights[i] = weightSelector.Invoke(item);
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
				}
			}

			public ReadOnlyWeightedSelector (IDictionary<TItem,float> source,IWeightedSelectMethod method) {
				m_Method = method;

				int count = source.Count;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				int i = 0;
				foreach (var pair in source) {
					if (count > i) {
						m_Items[i] = pair.Key;
						m_Weights[i] = pair.Value;
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
					i++;
				}
			}

			public ReadOnlyWeightedSelector (IReadOnlyDictionary<TItem,float> source,IWeightedSelectMethod method) {
				m_Method = method;

				int count = source.Count;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				int i = 0;
				foreach (var pair in source) {
					if (count > i) {
						m_Items[i] = pair.Key;
						m_Weights[i] = pair.Value;
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
					i++;
				}
			}

			~ReadOnlyWeightedSelector () {
				Dispose();
			}

			public TItem SelectItem (float value) {
				if (m_IsDisposed) {
					return default;
				}
				int index = m_Method.SelectIndex(m_Weights.Array,value);
				return (index >= 0) ? m_Items[index] : default;
			}

			bool m_IsDisposed;

			public void Dispose () {
				if (!m_IsDisposed) {
					m_Items.Dispose();
					m_Weights.Dispose();
					m_Method = null;

					m_IsDisposed = true;
				}
			}

		}

		#endregion

		#region ToReadOnlyWeightedSelector<TItem,TSource>

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TSource, TItem> (this IEnumerable<TSource> source,Func<TSource,TItem> itemSelector,Func<TSource,float> weightSelector) {
			return ToReadOnlyWeightedSelector(source,itemSelector,weightSelector,WeightedSelectMethod.Linear);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TSource, TItem> (this IEnumerable<TSource> source,Func<TSource,TItem> itemSelector,Func<TSource,float> weightSelector,IWeightedSelectMethod method) {
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

			if (source is IReadOnlyList<TSource> readonlyList) {
				return new ReadOnlyWeightedSelector<TSource,TItem>(readonlyList,itemSelector,weightSelector,method);
			}
			if (source is IList<TSource> list) {
				return new ReadOnlyWeightedSelector<TSource,TItem>(list,itemSelector,weightSelector,method);
			}
			return new ReadOnlyWeightedSelector<TSource,TItem>(source,itemSelector,weightSelector,method);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TSource, TItem> (this IDictionary<TSource,float> source,Func<TSource,TItem> itemSelector) {
			return ToReadOnlyWeightedSelector(source,itemSelector,WeightedSelectMethod.Linear);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TSource, TItem> (this IDictionary<TSource,float> source,Func<TSource,TItem> itemSelector,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (itemSelector == null) {
				throw new ArgumentNullException(nameof(itemSelector));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}

			return new ReadOnlyWeightedSelector<TSource,TItem>(source,itemSelector,method);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TSource, TItem> (this IReadOnlyDictionary<TSource,float> source,Func<TSource,TItem> itemSelector) {
			return ToReadOnlyWeightedSelector(source,itemSelector,WeightedSelectMethod.Linear);
		}

		public static IReadOnlyWeightedSelector<TItem> ToReadOnlyWeightedSelector<TSource, TItem> (this IReadOnlyDictionary<TSource,float> source,Func<TSource,TItem> itemSelector,IWeightedSelectMethod method) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (itemSelector == null) {
				throw new ArgumentNullException(nameof(itemSelector));
			}
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}

			return new ReadOnlyWeightedSelector<TSource,TItem>(source,itemSelector,method);
		}

		class ReadOnlyWeightedSelector<TSource, TItem> : IReadOnlyWeightedSelector<TItem>, IDisposable {

			TemporaryArray<TItem> m_Items;
			TemporaryArray<float> m_Weights;
			IWeightedSelectMethod m_Method;

			public ReadOnlyWeightedSelector (IEnumerable<TSource> source,Func<TSource,TItem> itemSelector,Func<TSource,float> weightSelector,IWeightedSelectMethod method) {
				m_Method = method;

				using var sourceArray = TemporaryArray<TSource>.From(source);

				int count = sourceArray.Length;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				for (int i = 0;m_Items.Capacity > i;i++) {
					if (count > i) {
						TSource element = sourceArray[i];
						m_Items[i] = itemSelector(element);
						m_Weights[i] = weightSelector.Invoke(element);
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
				}
			}

			public ReadOnlyWeightedSelector (IList<TSource> source,Func<TSource,TItem> itemSelector,Func<TSource,float> weightSelector,IWeightedSelectMethod method) {
				m_Method = method;

				int count = source.Count;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				for (int i = 0;m_Weights.Capacity > i;i++) {
					if (count > i) {
						TSource element = source[i];
						m_Items[i] = itemSelector(element);
						m_Weights[i] = weightSelector(element);
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
				}
			}

			public ReadOnlyWeightedSelector (IReadOnlyList<TSource> source,Func<TSource,TItem> itemSelector,Func<TSource,float> weightSelector,IWeightedSelectMethod method) {
				m_Method = method;

				int count = source.Count;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				for (int i = 0;m_Weights.Capacity > i;i++) {
					if (count > i) {
						TSource element = source[i];
						m_Items[i] = itemSelector(element);
						m_Weights[i] = weightSelector(element);
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
				}
			}

			public ReadOnlyWeightedSelector (IDictionary<TSource,float> source,Func<TSource,TItem> itemSelector,IWeightedSelectMethod method) {
				m_Method = method;

				int count = source.Count;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				int i = 0;
				foreach (var pair in source) {
					if (count > i) {
						m_Items[i] = itemSelector(pair.Key);
						m_Weights[i] = pair.Value;
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
					i++;
				}
			}

			public ReadOnlyWeightedSelector (IReadOnlyDictionary<TSource,float> source,Func<TSource,TItem> itemSelector,IWeightedSelectMethod method) {
				m_Method = method;

				int count = source.Count;
				m_Items = TemporaryArray<TItem>.Create(count);
				m_Weights = TemporaryArray<float>.Create(count);
				int i = 0;
				foreach (var pair in source) {
					if (count > i) {
						m_Items[i] = itemSelector(pair.Key);
						m_Weights[i] = pair.Value;
					} else {
						m_Items[i] = default;
						m_Weights[i] = 0f;
					}
					i++;
				}
			}

			~ReadOnlyWeightedSelector () {
				Dispose();
			}

			public TItem SelectItem (float value) {
				if (m_IsDisposed) {
					return default;
				}
				int index = m_Method.SelectIndex(m_Weights.Array,value);
				return (index >= 0) ? m_Items[index] : default;
			}

			bool m_IsDisposed;

			public void Dispose () {
				if (!m_IsDisposed) {
					m_Items.Dispose();
					m_Weights.Dispose();
					m_Method = null;

					m_IsDisposed = true;
				}
			}

		}

		#endregion

	}
}