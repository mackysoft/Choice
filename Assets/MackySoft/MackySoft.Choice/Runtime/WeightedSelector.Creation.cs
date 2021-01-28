using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.Choice {
	public static class WeightedSelector {

		public static IReadOnlyWeightedSelector<T> Empty<T> () => EmptyWeightedSelector<T>.Instance;

	}
}