# Choice - Weighted Random Selector

**Created by Hiroya Aramaki ([Makihiro](https://twitter.com/makihiro_dev))**

## What is Weighted Random Selector ?

Weighted Random Selector is an algorithm for randomly selecting elements based on their weights.

For example.
- Drop items based on rarity.
- Events that occur with a certain probability

It can be used to determine things with probability.

Choice is a library that was created to make it easier to implement.

Great introduction article on Weighted Random Select: https://blog.bruce-hill.com/a-faster-weighted-random-choice

## <a id="index" href="#index"> Table of Contents </a>

- [Installation](#installation)
- [Usage](#usage)
  - [Select Algorithm](#algorithm)
- [Author Info](#author-info)
- [License](#license)

# <a id="installation" href="#installation"> Installation </a>

Download any version from releases.

Releases: https://github.com/mackysoft/Choice/releases

# <a id="usage" href="#requirements"> Usage </a>

```cs
public class Item {
	public string id;
	public bool enabled;
	public float rarity;
}

public Item[] items;

public IEnumerable<Item> SelectItems () {
	IWeightedSelector<Item> weightedSelector = items
		.ToWeightedSelector(item => item.rarity);
	
	for (int i = 0;i < 1000;i++) {
		// Same as weightedSelector.SelectItem(UnityEngine.Random.value)
		Item randomSelectedItem = weightedSelector.SelectItemWithUnityRandom();
		yield return randomSelectedItem;
	}
}
```

Since the `ToWeightedSelector` function is defined as an extension of `IEnumerable<T>`, it can be connected from the LINQ syntax.

```
items
	.Where(item => (item != null) && item.enabled)
	.ToWeightedSelector(weightSelector: item => item.rarity)
	.SelectItemWithUnityRandom();
```


## <a id="algorithm" href="#algorithm"> Select Algorithm </a>

When creating a WeightedSelector, you can specify the `IWeightedSelectMethod`.

```
var weightedSelector = items.ToWeightedSelector(
	weightSelector: item => item.rarity,
	method: WeightedSelectMethod.Binary // Use the binary search algorithm.
);
```

If this is not specified, the linear scan algorithm will be used automatically.

### <a id="linear" href="#linear"> Linear Scan (`WeightedSelectMethod.Linear`) </a>

The simplest algorithm that walks linearly along the weights.
This method is an `O(n)` operation, where `n` is number of weights.

### <a id="binary" href="#binary"> Binary Search (`WeightedSelectMethod.Binary`) </a>

The binary search algorithm that is faster than linear scan by preprocessing to store the current sum of weights.

It has an additional storage cost of `O(n)`, but is accelerated by up to `O(log(n))` for each selection, where `n` is number of weights.


# <a id="author-info" href="#author-info"> Author Info </a>

Hiroya Aramaki is a indie game developer in Japan.

- Blog: [https://mackysoft.net/blog](https://mackysoft.net/blog)
- Twitter: [https://twitter.com/makihiro_dev](https://twitter.com/makihiro_dev)

# <a id="license" href="#license"> License </a>

This library is under the MIT License.
