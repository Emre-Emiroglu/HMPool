## [1.0.0] - 2025-07-07

### Added
- Core pooling system architecture with PoolBase abstract class.
- IPoolable interface with lifecycle methods (OnCreated, OnGetFromPool, OnReturnToPool, OnDestroyed).
- MonoPool implementation for Unity MonoBehaviour objects.
- PurePool implementation for non-MonoBehaviour C# classes.
- MonoPoolable and PurePoolable abstract classes for easy implementation.
- PoolService for centralized pool management.
- PoolServiceUtilities with static helper methods for easy access.
- PoolConfig ScriptableObject for configuration management.
- PoolDatum structure for individual pool configuration.
- Custom PoolDatumDrawer for improved editor experience.
- Comprehensive PlayMode tests for all pooling functionality.
- Test cases for MonoPool and PurePool lifecycle methods.
- README with comprehensive documentation and usage examples.

### Changed
- N/A

### Fixed
- N/A

### Removed
- N/A