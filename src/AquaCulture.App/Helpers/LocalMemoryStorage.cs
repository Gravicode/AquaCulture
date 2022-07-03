using Blazored.LocalStorage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.App.Helpers
{
    public class LocalMemoryStorage : ISyncMemoryStorageService
    {
        public ILocalStorageService Datas { get; set; }
        //private Dictionary<string,object> Datas { get; set; }
        public LocalMemoryStorage(ILocalStorageService data)
        {
            Datas = data;
            //Datas = new();
        }
       


        public async Task ClearAsync()
        {
            await Datas.ClearAsync();
        }

        async Task<bool> ISyncMemoryStorageService.ContainKey(string key)
        {
            return await Datas.ContainKeyAsync(key);
        }

        async Task<T> ISyncMemoryStorageService.GetItem<T>(string key)
        {
            return await Datas.ContainKeyAsync(key) ? await Datas.GetItemAsync<T>(key) : default(T);
        }

        async Task<string> ISyncMemoryStorageService.GetItemAsString(string key)
        {
            return await Datas.ContainKeyAsync(key) ? await Datas.GetItemAsync<string>(key) : string.Empty;
        }

        async Task<string> ISyncMemoryStorageService.Key(int index)
        {
            var keys = ( await Datas.KeysAsync()).ToList();
            return keys[index];
        }

        async Task<int> ISyncMemoryStorageService.Length()
        {
            return await Datas.LengthAsync();
        }

        async Task ISyncMemoryStorageService.RemoveItem(string key)
        {
            if (await Datas.ContainKeyAsync(key))
            {
                await Datas.RemoveItemAsync(key);
            }
        }

        async Task ISyncMemoryStorageService.SetItem<T>(string key, T data)
        {
            if (await Datas.ContainKeyAsync(key))
            {
                await Datas.SetItemAsync<T>(key, data);
            }
            else
            {
                await Datas.SetItemAsync<T>(key, data);
            }
        }

        async Task ISyncMemoryStorageService.SetItemAsString(string key, string data)
        {
            if (await Datas.ContainKeyAsync(key))
            {
                await Datas.SetItemAsStringAsync(key, data);
            }
            else
            {
                await Datas.SetItemAsStringAsync(key, data);
            }
        }
    }
}
