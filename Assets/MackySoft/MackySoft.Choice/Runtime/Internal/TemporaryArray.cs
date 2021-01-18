﻿using System;
using System.Collections.Generic;

namespace MackySoft.Choice.Internal {

	/// <summary>
	/// <para> Temporary array without allocation. </para>
	/// <para> This struct use <see cref="ArrayPool{T}"/> internally to avoid allocation and can be used just like a normal array. </para>
	/// <para> After using it, please call the Dispose(). </para>
	/// </summary>
	public struct TemporaryArray<T> : IDisposable {

		/// <summary>
		/// Create a temporary array of the specified length.
		/// </summary>
		public static TemporaryArray<T> Create (int length) {
			return new TemporaryArray<T>(ArrayPool<T>.Rent(length),length);
		}

		/// <summary>
		/// <para> Create a temporary array with a length of 0. </para>
		/// <para> The length can be increased by using the <see cref="Add(T)"/>. </para>
		/// </summary>
		/// <param name="prepare"> Length of the internal array to be prepared. </param>
		public static TemporaryArray<T> CreateAsList (int prepare) {
			return new TemporaryArray<T>(ArrayPool<T>.Rent(prepare),0);
		}

		/// <summary>
		/// Create a temporary array from the elements of <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		public static TemporaryArray<T> From (IEnumerable<T> source) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}

			T[] sourceArray = source.ToArrayFromPool(out int count);
			return new TemporaryArray<T>(sourceArray,count);
		}

		T[] m_Array;
		int m_Length;

		public int Length => m_Length;

		/// <summary>
		/// Length of internal array;
		/// </summary>
		public int Capacity => m_Array.Length;

		/// <summary>
		/// <para> Internal array. </para>
		/// <para> The length of internal array is always greater than or equal to <see cref="Length"/> property. </para>
		/// </summary>
		public T[] Array => m_Array;

		public T this[int index] {
			get => m_Array[index];
			set => m_Array[index] = value;
		}

		public TemporaryArray (T[] array,int length) {
			m_Array = array;
			m_Length = length;
		}

		/// <summary>
		/// Set item to current length and increase length.
		/// </summary>
		public void Add (T item) {
			if (m_Length >= m_Array.Length) {
				return;
			}
			m_Array[m_Length] = item;
			m_Length++;
		}

		public void Dispose () {
			ArrayPool<T>.Return(ref m_Array);
			m_Length = 0;
		}

	}

	public static class TemporaryArrayExtensions {

		/// <summary>
		/// Create a temporary array from the elements of <see cref="IEnumerable{T}"/>.
		/// </summary>
		public static TemporaryArray<T> ToTemporaryArray<T> (this IEnumerable<T> source) {
			return TemporaryArray<T>.From(source);
		}

	}
}