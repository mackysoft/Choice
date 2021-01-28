using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MackySoft.Choice {
	internal sealed class EmptyWeightedSelector<T> : IWeightedSelector<T> {

		public static readonly EmptyWeightedSelector<T> Instance = new EmptyWeightedSelector<T>();

		public IEnumerable<T> Keys => Enumerable.Empty<T>();

		public IEnumerable<float> Values => Enumerable.Empty<float>();

		public int Count => 0;

		ICollection<T> IDictionary<T,float>.Keys => throw new System.NotImplementedException();

		ICollection<float> IDictionary<T,float>.Values => throw new System.NotImplementedException();

		bool ICollection<KeyValuePair<T,float>>.IsReadOnly => false;

		public float this[T key] {
			get { return 0f; }
			set { }
		}

		public bool ContainsKey (T key) => false;

		public bool TryGetValue (T key,out float value) {
			value = 0f;
			return false;
		}

		public T SelectItem (float value) {
			return default;
		}

		public IEnumerator<KeyValuePair<T,float>> GetEnumerator () {
			return Enumerable.Empty<KeyValuePair<T,float>>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return GetEnumerator();
		}

		public void Add (T key,float value) {
			
		}

		public bool Remove (T key) {
			return false;
		}

		public void Add (KeyValuePair<T,float> item) {
			
		}

		public void Clear () {
			
		}

		public bool Contains (KeyValuePair<T,float> item) {
			return false;
		}

		public void CopyTo (KeyValuePair<T,float>[] array,int arrayIndex) {
			
		}

		public bool Remove (KeyValuePair<T,float> item) {
			return false;
		}
	}
}