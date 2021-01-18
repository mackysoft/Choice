using System;

namespace MackySoft.Choice {

	public interface IWeightedSelectMethod {

		/// <summary>
		/// Returns a selected index from weights.
		/// </summary>
		/// <param name="weights"> An array of weights consisting of 0 or more numbers. </param>
		/// <param name="value">
		///	<para> Value for selecting an index. </para>
		///	<para> The value must be between 0.0f and 1.0f. </para>
		/// </param>
		/// <returns> Selected index from weights. </returns>
		/// <exception cref="ArgumentNullException"></exception>
		int SelectIndex (float[] weights,float value);

	}

	public static class WeightedSelectMethod {

		/// <summary>
		/// <para> The simplest algorithm that walks linearly along the weights. </para>
		/// <para> This method is an O(n) operation, where n is number of weights. </para>
		/// </summary>
		public static readonly IWeightedSelectMethod Linear = LinearWeightedSelectMethod.Instance;

		/// <summary>
		/// <para> The binary search algorithm that is faster than linear scan by preprocessing to store the current sum of weights. </para>
		/// <para> It has an additional storage cost of O(n),  </para>
		/// <para> but is accelerated by up to O(log(n)) for each selection, </para>
		/// <para> where n is number of weights. </para>
		/// </summary>
		public static readonly IWeightedSelectMethod Binary = BinaryWeightedSelectMethod.Instance;

	}

}