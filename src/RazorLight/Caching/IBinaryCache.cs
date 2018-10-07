using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace RazorLight.Caching
{
    public interface IBinaryCache: IDisposable
    {
        IBinaryCacheEntry CreateEntry(string key, byte[] binary);

        void Remove(string key);

        bool TryGetValue(string key, out byte[] binary);
    }

    public class MemoryBinaryCache : IBinaryCache
    {
        private readonly IMemoryCache _cache;

        public MemoryBinaryCache()
        {
            var cacheOptions = Options.Create(new MemoryCacheOptions());
            _cache = new MemoryCache(cacheOptions);
        }

        public IBinaryCacheEntry CreateEntry(string key, byte[] binary)
        {
            var entry = _cache.CreateEntry(key);
            entry.Value = binary;
            entry.Size = binary?.Length;
            return new MemoryBinaryCacheEntry(entry);            
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public bool TryGetValue(string key, out byte[] binary)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                binary = value as byte[];
                return true;
            }

            binary = null;
            return false;
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }

    public interface IBinaryCacheEntry
    {
        //
        // Summary:
        //     Gets the key of the cache entry.
        string Key { get; }
        //
        // Summary:
        //     Gets or set the value of the cache entry.
        byte[] Value { get; }
        //
        // Summary:
        //     Gets or set the size of the cache entry value.
        long? Size { get; }
    }

    public class MemoryBinaryCacheEntry : IBinaryCacheEntry
    {
        private readonly ICacheEntry _entry;

        public MemoryBinaryCacheEntry(ICacheEntry entry)
        {
            _entry = entry;
        }

        public string Key => _entry.Key.ToString();

        public byte[] Value
        {
            get => _entry.Value as byte[];
            set => _entry.Value = value;
        }

        public long? Size
        {
            get => _entry.Size;
            set => _entry.Size = value;
        }
    }
}

