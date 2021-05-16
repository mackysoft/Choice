using System;
using System.Collections;
using System.Collections.Generic;
using MackySoft.Choice.Internal;

namespace MackySoft.Choice {

	public class WeightedSelector<TItem> : IWeightedSelector<TItem>, IDisposable {

		TemporaryArray<TItem> m_Items;
		TemporaryArray<float> m_Weights;
		IWeightedSelectMethod m_Method;

		public int Count => m_Items.Length;

		public float this[TItem item] {
			get {
				int index = IndexOf(item);
				if (index == -1) {
					throw KeyNotFound();
				}
				return m_Weights[index];
			}
			set {
				int index = IndexOf(item);
				if (index == -1) {
					throw KeyNotFound();
				}
				m_Weights[index] = value;
				m_Method.Calculate(m_Weights);
			}
		}

		public WeightedSelector (IEnumerable<TItem> source,Func<TItem,float> weightSelector,IWeightedSelectMethod method) {
			EnumerableConversion.EnumerableToTemporaryArray(source,weightSelector,out m_Items,out m_Weights);
			m_Method = method;
			m_Method.Calculate(m_Weights);
		}

		public WeightedSelector (IEnumerable<KeyValuePair<TItem,float>> source,IWeightedSelectMethod method) {
			EnumerableConversion.DictionaryToTemporaryArray(source,out m_Items,out m_Weights);
			m_Method = method;
			m_Method.Calculate(m_Weights);
		}

		internal WeightedSelector (TemporaryArray<TItem> items,TemporaryArray<float> weights,IWeightedSelectMethod method) {
			if (items.Length != weights.Length) {
				throw new ArgumentException();
			}
			m_Items = items;
			m_Weights = weights;
			m_Method = method;
			m_Method.Calculate(m_Weights);
		}

		~WeightedSelector () {
			Dispose();
		}

		public bool TryGetValue (TItem item,out float value) {
			int index = IndexOf(item);
			if (index == -1) {
				value = 0f;
				return false;
			}
			value = m_Weights[index];
			return true;
		}

		public bool ContainsKey (TItem item) {
			return IndexOf(item) != -1;
		}

		public void Add (TItem item,float weight) {
			int index = IndexOf(item);
			if (index != -1) {
				throw KeyNotFound();
			}

			m_Items.Add(item);
			m_Weights.Add(weight);
			m_Method.Calculate(m_Weights);
		}

		public bool Remove (TItem item) {
			int index = IndexOf(item);
			if (index == -1) {
				return false;
			}
			m_Items.RemoveAt(index);
			m_Weights.RemoveAt(index);
			m_Method.Calculate(m_Weights);
			return true;
		}

		public void Clear () {
			m_Items.Clear();
			m_Weights.Clear();
			m_Method.Calculate(m_Weights);
		}

		public TItem SelectItem (float value) {
			int index = m_Method.SelectIndex(m_Weights,value);
			return (index >= 0) ? m_Items[index] : default;
		}

		int IndexOf (TItem item) {
			if (item != null) {
				for (int i = 0;m_Items.Length > i;i++) {
					if (item.Equals(m_Items[i])) {
						return i;
					}
				}
			}
			return -1;
		}

		Exception KeyNotFound () {
			return new KeyNotFoundException();
		}

		#region Dictionary<T,float> Implementation

		bool ICollection<KeyValuePair<TItem,float>>.IsReadOnly => false;

		ICollection<TItem> IDictionary<TItem,float>.Keys => throw new NotImplementedException();

		IEnumerable<TItem> IReadOnlyDictionary<TItem,float>.Keys => throw new NotImplementedException();
		
		ICollection<float> IDictionary<TItem,float>.Values => throw new NotImplementedException();

		IEnumerable<float> IReadOnlyDictionary<TItem,float>.Values => throw new NotImplementedException();

		void ICollection<KeyValuePair<TItem,float>>.Add (KeyValuePair<TItem,float> item) {
			Add(item.Key,item.Value);
		}

		bool ICollection<KeyValuePair<TItem,float>>.Contains (KeyValuePair<TItem,float> item) {
			throw new NotImplementedException();
		}

		void ICollection<KeyValuePair<TItem,float>>.CopyTo (KeyValuePair<TItem,float>[] array,int arrayIndex) {
			throw new NotImplementedException();
		}

		bool ICollection<KeyValuePair<TItem,float>>.Remove (KeyValuePair<TItem,float> item) {
			throw new NotImplementedException();
		}

		public IEnumerator<KeyValuePair<TItem,float>> GetEnumerator () {
			for (int i = 0;m_Items.Length > i;i++) {
				yield return new KeyValuePair<TItem,float>(m_Items[i],m_Weights[i]);
			}
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return GetEnumerator();
		}

		#endregion

		#region IDisposable Support

		bool m_IsDisposed;

		public void Dispose () {
			if (!m_IsDisposed) {
				m_Items.Dispose();
				m_Weights.Dispose();
				m_Method = null;

				m_IsDisposed = true;
			}
		}

		#endregion

	}
}