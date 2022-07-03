using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.App.Helpers
{
    public interface ISyncMemoryStorageService
    {
        //event EventHandler<ChangingEventArgs> Changing;
        //event EventHandler<ChangedEventArgs> Changed;

        Task ClearAsync();
        Task<bool> ContainKey(string key);
        Task<T> GetItem<T>(string key);
        Task<string> GetItemAsString(string key);
        Task<string> Key(int index);
        Task<int> Length();
        Task RemoveItem(string key);
        Task SetItem<T>(string key, T data);
        Task SetItemAsString(string key, string data);

        /*
           void ClearAsync();
        bool ContainKey(string key);
        T GetItem<T>(string key);
        string GetItemAsString(string key);
        string Key(int index);
        int Length();
        void RemoveItem(string key);
        void SetItem<T>(string key, T data);
        void SetItemAsString(string key, string data);
         */
    }
}
