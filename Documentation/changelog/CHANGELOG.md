# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.1] - 2021-07-31
### Added
- Added `WeightedSelectMethodSpeedTests`.

### Changed
- Optimized the setup of `AliasWeightedSelectMethod`.

## [1.2.0] - 2021-07-30
### Added
- Added alias method and tests.
    - This is a very effective algorithm for selecting multiple items.

## [1.1.0] - 2021-05-17
### Added
- Added `IWeightedSelectMethod.Calculate` method.
    - This allows the results of the pre-calculation to be cached, resulting in improved performance.
- Added `CompareAllMethods` test.
    - All implemented `IWeightedSelectMethod`s must return the same result.

## [1.0.0] - 2021-02-01
### Added
- Added the `IWeightedSelector<T>` and `WeightedSelector<T>`.
- Added an unit tests.

### Changes
- Renamed the `ToReadOnlyWeightedSelector` method to `ToWeightedSelector`.
- Renamed the `SelectItem` method that used `UnityEngine.Random.value` to `SelectItemWithUnityRandom`.
- All WeightedSelectMethods now support weights less than or equal to 0.
- IWeightedSelectMethod now takes a TemporaryArray<float> as an argument instead of a float[].

### Fixes
- Fixed a fatal bug that prevented BinaryWeightedSelectMethod from working.

## [0.1.0] - 2021-01-19
First release