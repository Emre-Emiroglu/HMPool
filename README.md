# HMPool
HMPool is a modular object pool system designed for Unity. It manages pools for both MonoBehaviour and pure C# classes, optimizing performance by eliminating unnecessary object creation and destruction.

## Features
HMPool offers a flexible and efficient object pooling solution:
* Object Pool Management: Creates and manages separate pools for MonoBehaviour and pure C# classes.
* Dynamic Pool Management: Pools are automatically created when needed and objects are released when no longer in use.
* Performance Optimization: Reduces the overhead of frequent object instantiations and destructions by reusing objects.
* Customizable Pool Configurations: Configure the initial size, default capacity, and maximum size of each pool.

## Getting Started
Install via UPM with git URL

`https://github.com/Emre-Emiroglu/HMPool.git`

Clone the repository
```bash
git clone https://github.com/Emre-Emiroglu/HMPool.git
```
This project is developed using Unity version 6000.0.42f1.

## Usage
* Initializing the PoolService:
    ```csharp
    PoolConfig config = ...;  // ScriptableObject
    PoolServiceUtilities.Initialize(config);
    ```
  
* Getting an Object from the Pool:
    ```csharp
    MyMonoObject monoObj = PoolServiceUtilities.GetMono<MyMonoObject>();
    MyPureObject pureObj = PoolServiceUtilities.GetPure<MyPureObject>();
    ```
  
* Releasing an Object to the Pool:
    ```csharp
    PoolServiceUtilities.ReleaseMono(objMono);
    PoolServiceUtilities.ReleasePure(objPure);
    PoolServiceUtilities.ReleaseAllMono(objMono);
    PoolServiceUtilities.ReleaseAllPure(objPure);
    ```
  
* Destroying an Object:
    ```csharp
    PoolServiceUtilities.DestroyMono(objMono);
    PoolServiceUtilities.DestroyPure(objPure);
    PoolServiceUtilities.DestroyAllMono(objMono);
    PoolServiceUtilities.DestroyAllPure(objPure);
    ```
* Error Handling:
  * Pool Not Found: If you attempt to get an object from a pool that has not been initialized, an `InvalidOperationException` will be thrown, indicating that no pool has been found for the requested type.
  * Object Type Mismatch: When trying to release an object of a different type than expected (for example, releasing an object that doesn't belong to the correct pool), the system will log an error message, providing details of the type mismatch.

## Acknowledgments
Special thanks to the Unity community for their invaluable resources and tools.

For more information, visit the GitHub repository.
