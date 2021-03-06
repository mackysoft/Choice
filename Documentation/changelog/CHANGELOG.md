# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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