# Choice - Weighted Random Selector

**Created by Hiroya Aramaki ([Makihiro](https://twitter.com/makihiro_dev))**

[![Tests](https://github.com/mackysoft/Choice/actions/workflows/tests.yaml/badge.svg)](https://github.com/mackysoft/Choice/actions/workflows/tests.yaml)
[![Build](https://github.com/mackysoft/Choice/actions/workflows/build.yaml/badge.svg)](https://github.com/mackysoft/Choice/actions/workflows/build.yaml)
[![Release](https://img.shields.io/github/v/release/mackysoft/Choice)](https://github.com/mackysoft/Choice/releases)
[![openupm](https://img.shields.io/npm/v/com.mackysoft.choice?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.mackysoft.choice/)

## What is Weighted Random Selector ?

Weighted Random Selector is an algorithm for randomly selecting elements based on their weights.

For example.
- Drop items based on rarity.
- Events that occur with a certain probability

It can be used to determine things with probability.

Choice is a library that was created to make it easier to implement.

```cs
// This is the simplest usage.
var randomSelectedItem = items
	.ToWeightedSelector(item => item.weight)
	.SelectItemWithUnityRandom();
```

Great introduction article on Weighted Random Select: https://blog.bruce-hill.com/a-faster-weighted-random-choice


## <a id="index" href="#index"> Table of Contents </a>

- [📥 Installation](#installation)
- [🔰 Usage](#usage)
  - [ToWeightedSelector Overloads](#toweightedselector-overloads)
    - [from weighted entry pattern](#from-weighted-entry)
    - [from weighted item pattern](#from-weighted-item)
    - [from Dictionary<Item,float> pattern](#from-dictionary)
  - [LINQ](#linq)
  - [Algorithms](#algorithms)
    - [Linear Scan](#linear-scan)
    - [Binary Search](#binary-search)
    - [Alias Method](#alias-method)
    - [Speed Measurement](#speed-measurement)
- [📔 Author Info](#author-info)
- [📜 License](#license)


# <a id="installation" href="#installation"> 📥 Installation </a>

Download any version from releases.

Releases: https://github.com/mackysoft/Choice/releases

### Install via git URL

Or, you can add this package by opening PackageManager and entering

`https://github.com/mackysoft/Choice.git?path=Assets/MackySoft/MackySoft.Choice`

from the `Add package from git URL` option.


### Install via Open UPM

Or, you can install this package from the [Open UPM](https://openupm.com/packages/com.mackysoft.choice/) registry.

More details [here](https://openupm.com/).

```
openupm add com.mackysoft.choice
```

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


## <a id="toweightedselector-overloads" href="#toweightedselector-overloads"> `ToWeightedSelector` Overloads  </a>

The `ToWeightedSelector` method has many overloads and can be used for a variety of patterns.

### <a id="from-weighted-entry" href="#from-weighted-entry"> from weighted entry pattern </a>

```cs
public struct ItemEntry {
	public Item item;
	public float weight;
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
```

### <a id="from-weighted-item" href="#from-weighted-item"> from weighted item pattern </a>

```cs
public class WeightedItem {
	public string id;
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
```

### <a id="from-dictionary" href="#from-dictionary"> from `Dictionary<TItem,float>` pattern </a>

```cs
public class Item {
	public string id;
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


### <a id="alias-method" href="#alias-method"> Alias Method (`WeightedSelectMethod.Alias`) </a>

The fastest algorithm.

It takes `O(n)` run time to set up, but the selection is performed in `O(1)` run time,
where `n` is number of weights.

Therefore, this is a very effective algorithm for selecting multiple items.

## <a id="speed-measurement" href="#speed-measurement"> Speed Measurement </a>

### <a id="1-items" href="#1-items"> 1 items </a>

![Speed measurement of Weighted Random Selection Algorithms  (1 items)](https://user-images.githubusercontent.com/13536348/127739858-60f05a16-6e3b-42f6-b7f2-b4b106eb3dfa.png)

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|0.0104ms|**0.0055ms**|**0.0081ms**|**0.0393ms**|**0.3408ms**|
|Binary Search|0.0091ms|0.0083ms|0.0126ms|0.0659ms|0.5944ms|
|Alias Method|**0.0069ms**|0.0065ms|0.01ms|0.0459ms|0.4094ms|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|0.0155ms|**0.0064ms**|**0.0077ms**|**0.0381ms**|**0.353ms**|
|Binary Search|0.0077ms|0.008ms|0.0123ms|0.0659ms|0.5919ms|
|Alias Method|**0.0062ms**|0.0065ms|0.01ms|0.0462ms|0.41ms|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|0.009ms|**0.0053ms**|**0.0081ms**|**0.0378ms**|**0.3388ms**|
|Binary Search|0.0073ms|0.0079ms|0.0129ms|0.0653ms|0.5927ms|
|Alias Method|**0.0054ms**|0.0062ms|0.0104ms|0.0461ms|0.4194ms|


### <a id="10-items" href="#10-items"> 10 items </a>

![Speed measurement of Weighted Random Selection Algorithms  (10 items)](https://user-images.githubusercontent.com/13536348/127739862-bddbf2d2-6075-4d4e-bfc7-9ccd2f8fdbb3.png)

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|0.0113ms|**0.0077ms**|0.0182ms|0.1219ms|1.19ms|
|Binary Search|**0.0109ms**|0.0114ms|0.0237ms|0.158ms|1.4975ms|
|Alias Method|0.0136|0.022ms|**0.0151ms**|**0.0601ms**|**0.5041ms**|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|0.012ms|**0.0072ms**|0.0174ms|0.1272ms|1.1738ms|
|Binary Search|**0.0095ms**|0.0099ms|0.023ms|0.16ms|1.5503ms|
|Alias Method|0.0141ms|0.0104ms|**0.0148ms**|**0.0618ms**|**0.5235ms**|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**0.0095ms**|**0.009ms**|0.0179ms|0.1216ms|1.1503ms|
|Binary Search|0.0096ms|0.0103ms|0.0225ms|0.1572ms|1.4991ms|
|Alias Method|0.0129ms|0.0105ms|**0.015ms**|**0.0607ms**|**0.5176ms**|

### <a id="100-items" href="#100-items"> 100 items </a>

![Speed measurement of Weighted Random Selection Algorithms  (100 items)](https://user-images.githubusercontent.com/13536348/127739863-d9a3338b-1a40-45bb-a292-9330b9414561.png)

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**0.0201ms**|0.024ms|0.0822ms|0.741ms|7.2211ms|
|Binary Search|0.0212ms|**0.0211ms**|0.0433ms|0.3118ms|2.6434ms|
|Alias Method|0.0717ms|0.0364ms|**0.0395ms**|**0.086ms**|**0.5462ms**|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|0.0231ms|0.0247ms|0.0855ms|0.7027ms|7.0025ms|
|Binary Search|**0.0224ms**|**0.0231ms**|0.0441ms|0.2776ms|2.6521ms|
|Alias Method|*0.039ms|0.0358ms|**0.0405ms**|**0.0861ms**|**0.5561ms**|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**0.0194ms**|0.0232ms|0.0892ms|0.7582ms|7.1886ms|
|Binary Search|0.0206ms|**0.0218ms**|0.0447ms|0.2804ms|2.6375ms|
|Alias Method|0.0376ms|0.0381ms|**0.0413ms**|**0.0871ms**|**0.5728ms**|

### <a id="1000-items" href="#1000-items"> 1000 items </a>

![Speed measurement of Weighted Random Selection Algorithms  (1000 items)](https://user-images.githubusercontent.com/13536348/127739880-c2da6789-126b-4d8b-8182-f3ee8f9936d5.png)

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**0.1147ms**|0.1672ms|0.7792ms|6.7539ms|66.8329ms|
|Binary Search|0.1205ms|**0.1183ms**|**0.1504ms**|0.4758ms|3.7755ms|
|Alias Method|0.2783ms|0.2722ms|0.2925ms|**0.3238ms**|**0.7824ms**|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**0.1068ms**|0.1717ms|0.8331ms|6.8282ms|68.455ms|
|Binary Search|0.1217ms|**0.1173ms**|**0.1499ms**|0.5026ms|3.7627ms|
|Alias Method|0.2785ms|0.2889ms|0.2876ms|**0.3318ms**|**0.908ms**|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**0.102ms**|0.1636ms|0.7271ms|6.743ms|66.4393ms|
|Binary Search|0.1241ms|**0.1208ms**|**0.1501ms**|0.5216ms|4.0165ms|
|Alias Method|0.2782ms|0.2755ms|0.2777ms|**0.3454ms**|**0.8068ms**|


### <a id="10000-items" href="#10000-items"> 10000 items </a>

![Speed measurement of Weighted Random Selection Algorithms  (10000 items)](https://user-images.githubusercontent.com/13536348/127739886-c0e4bbea-f3cc-4ece-9abe-6eed68597f0a.png)

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**1.1885ms**|1.7971ms|8.0482ms|69.1749ms|664.8696ms|
|Binary Search|1.3329ms|**1.3181ms**|**1.3454ms**|**1.7735ms**|6.1215ms|
|Alias Method|2.8859ms|2.8719ms|2.8832ms|2.9779ms|**3.4764ms**|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**1.1676ms**|1.6953ms|8.0905ms|70.1629ms|668.3197ms|
|Binary Search|1.3118ms|**1.3361ms**|**1.3407ms**|**1.786ms**|6.1105ms|
|Alias Method|2.8833ms|2.934ms|2.951ms|2.9845ms|**3.6259ms**|

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|**1.3098ms**|1.9826ms|8.063ms|68.9301ms|666.9364ms|
|Binary Search|1.4456ms|**1.3787ms**|4.6233ms|**1.7861ms**|6.0783ms|
|Alias Method|2.9751ms|2.9144ms|**2.9236ms**|2.9851ms|**3.5149ms**|

### <a id="10000-items" href="#10000-items"> 10000 items without setup </a>

![Speed measurement of Weighted Random Selection Algorithms  (10000 items without setup)](https://user-images.githubusercontent.com/13536348/127739890-3d5a1cd3-5b93-4151-a29d-c5e48507b15c.png)

|Iterations|1|10|100|1000|10000|
|:--|:--|:--|:--|:--|:--|
|Linear Scan|0.0207ms|0.7364ms|6.5433ms|67.3963ms|671.3184ms|
|Binary Search|0.0015ms|0.0055ms|0.0492ms|0.496ms|4.828ms|
|Alias Method|**0.0005ms**|**0.0011ms**|**0.0066ms**|**0.0579ms**|**0.5559ms**|

# <a id="author-info" href="#author-info"> 📔 Author Info </a>

Hiroya Aramaki is a indie game developer in Japan.

- Blog: [https://mackysoft.net/blog](https://mackysoft.net/blog)
- Twitter: [https://twitter.com/makihiro_dev](https://twitter.com/makihiro_dev)


# <a id="license" href="#license"> 📜 License </a>

This library is under the MIT License.
