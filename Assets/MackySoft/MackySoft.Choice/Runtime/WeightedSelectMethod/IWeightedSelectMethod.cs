using MackySoft.Choice.Internal;

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
		int SelectIndex (TemporaryArray<float> weights,float value);

		/// <summary>
		/// Calculate the required data.
		/// </summary>
		/// <param name="weights"></param>
		void Calculate (TemporaryArray<float> weights);

	}

	public static class WeightedSelectMethod {

		/// <summary>
		/// <para> The simplest algorithm that walks linearly along the weights. </para>
		/// <para> This method is an O(n) operation, where n is number of weights. </para>
		/// </summary>
		public static IWeightedSelectMethod Linear => new LinearWeightedSelectMethod();

		/// <summary>
		/// <para> The binary search algorithm that is faster than linear scan by preprocessing to store the current sum of weights. </para>
		/// <para> It has an additional storage cost of O(n),  </para>
		/// <para> but is accelerated by up to O(log(n)) for each selection, </para>
		/// <para> where n is number of weights. </para>
		/// </summary>
		public static IWeightedSelectMethod Binary => new BinaryWeightedSelectMethod();

		/// <summary>
		/// <para> The fastest algorithm. </para>
		/// <para> It takes O(n) run time to set up, but the selection is performed in O(1) run time, </para>
		/// <para> where n is number of weights. </para>
		/// <para> Therefore, this is a very effective algorithm for selecting multiple items. </para>
		/// </summary>
		public static IWeightedSelectMethod Alias => new AliasWeightedSelectMethod();

	}

}