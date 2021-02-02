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

- [📥 Installation](#installation)
- [🔰 Usage](#usage)
  - [ToWeightedSelector Overloads](#toweightedselector-overloads)
  - [LINQ](#linq)
  - [Algorithms](#algorithms)
    - [Linear Scan](#linear-scan)
    - [Binary Search](#binary-search)
- [📔 Author Info](#author-info)
- [📜 License](#license)


# <a id="installation" href="#installation"> 📥 Installation </a>

Download any version from releases.

Releases: https://github.com/mackysoft/Choice/releases


# <a id="usage" href="#requirements"> 🔰 Usage </a>

```cs
// To use Choice, add this namespace.
using MackySoft.Choice;

public class WeightedItem {
	public string id;
	public float weight;
}

public WeightedItem SelectItem () {
	// Prepare weighted items.
	var items = new WeightedItem[] {
		new WeightedItem { id = "🍒", weight = 8f },
		new WeightedItem { id = "🍏", weight = 4f },
		new WeightedItem { id = "🍍", weight = 0f },
		new WeightedItem { id = "🍇", weight = 6f },
		new WeightedItem { id = "🍊", weight = -1f }
	};
	
	// Create the WeightedSelector.
	var weightedSelector = items.ToWeightedSelector(item => item.weight);
	
	// The probability of each item being selected,
	// 🍒 is 44%, 🍏 is 22%, and 🍇 is 33%.
	// 🍍 and 🍊 will never be selected because their weights are less or equal to 0.
	return weightedSelector.SelectItemWithUnityRandom();
	// Same as weightedSelector.SelectItem(UnityEngine.Random.value);
}
```


## <a id="toweightedselector-overloads" href="#toweightedselector-overloads"> ToWeightedSelector Overloads  </a>

The `ToWeightedSelector` method has many overloads and can be used for a variety of patterns.

```cs
public class WeightedItem {
	public string id;
	public float weight;
}

public class Item {
	public string id;
}

public struct ItemEntry {
	public Item item;
	public float weight;
}

public IWeightedSelector<WeightedItem> WeightedItemPattern () {
	var items = new WeightedItem[] {
		new WeightedItem { id = "🍒", weight = 1f },
		new WeightedItem { id = "🍏", weight = 5f },
		new WeightedItem { id = "🍍", weight = 3f }
	};

	// Create a WeightedSelector using the weight of the WeightedItem.
	return fromWeightedItem = items.ToWeightedSelector(weightSelector: item => item.weight);
}

public IWeightedSelector<Item> WeightedEntryPattern () {
	var entries = new ItemEntry[] {
		new ItemEntry { item = new Item { id = "🍒" }, weight = 1f },
		new ItemEntry { item = new Item { id = "🍏" }, weight = 5f },
		new ItemEntry { item = new Item { id = "🍍" }, weight = 3f }
	};

	// Create a WeightedSelector by selecting item and weight from entry respectively.
	return entries.ToWeightedSelector(
		itemSelector: entry => entry.item,
		weightSelector: entry => entry.weight
	);
}


public IWeightedSelector<Item> DictionaryPattern () {
	// This need a Dictionary<TItem,float>. (Strictly speaking, IEnumerable<KeyValuePair<TItem,float>>)
	var dictionary = new Dictionary<Item,float>(
		{ new Item { id = "🍒" }, 1f },
		{ new Item { id = "🍏" }, 5f },
		{ new Item { id = "🍍" }, 3f }
	);

	// Create a WeightedSelector with the dictionary key as item and value as weight.
	return dictionary.ToWeightedSelector();
}
```


## <a id="linq" href="#linq"> LINQ </a>

Since the `ToWeightedSelector` method is defined as an extension of `IEnumerable<T>`, it can be connected from the LINQ query operators.

```cs
var randomSelectedItem = items
	.Where(item => item != null) // null check
	.ToWeightedSelector(item => item.weight)
	.SelectItemWithUnityRandom();
```


## <a id="algorithms" href="#algorithms"> Algorithms </a>

When creating a WeightedSelector, you can specify the `IWeightedSelectMethod`.

```cs
var weightedSelector = items.ToWeightedSelector(
	item => item.weight,
	WeightedSelectMethod.Binary // Use the binary search algorithm.
);
```

All `ToWeightedSelector` methods can specify `IWeightedSelectMethod`.

If this is not specified, the linear scan algorithm will be used automatically.


### <a id="linear-scan" href="#linear-scan"> Linear Scan (`WeightedSelectMethod.Linear`) </a>

The simplest algorithm that walks linearly along the weights.
This method is an `O(n)` operation, where `n` is number of weights.


### <a id="binary-search" href="#binary-search"> Binary Search (`WeightedSelectMethod.Binary`) </a>

The binary search algorithm that is faster than linear scan by preprocessing to store the current sum of weights.

It has an additional storage cost of `O(n)`, but is accelerated by up to `O(log(n))` for each selection, where `n` is number of weights.


# <a id="author-info" href="#author-info"> 📔 Author Info </a>

Hiroya Aramaki is a indie game developer in Japan.

- Blog: [https://mackysoft.net/blog](https://mackysoft.net/blog)
- Twitter: [https://twitter.com/makihiro_dev](https://twitter.com/makihiro_dev)


# <a id="license" href="#license"> 📜 License </a>

This library is under the MIT License.
