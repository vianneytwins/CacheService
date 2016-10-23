using System;

namespace fody
{
    public interface ICache
    {
        bool Contains(string key);

        T Retrieve<T>(string key);

        void Store(string key, object data);

        void Remove(string key);
    }

}

