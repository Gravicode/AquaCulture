using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCulture.App.Services
{
    public class TempStorageService:IDisposable
    {
        //public Dictionary<string,byte[]> DataStorage { get; set; }
        public ILocalStorageService DataStorage { get; set; }
        public TempStorageService(ILocalStorageService storage)
        {
            DataStorage = storage;
            //DataStorage = new Dictionary<string,byte[]>();  
        }

        public async Task<byte[]> GetItemAsync(string Key)
        {
            if (await DataStorage.ContainKeyAsync(Key))
            {
                return await DataStorage.GetItemAsync<byte[]>(Key);
            }
            else
                return null;
            /*
            if(DataStorage.ContainsKey(Key))
                return DataStorage[Key];
            else
                return null; 
        */
        }
        
        public bool SetItem(string Key,byte[] Data)
        {
            try
            {
                /*
                DataStorage.Add(Key, Data);
                */
                DataStorage.SetItemAsync<byte[]>(Key, Data);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public async void Dispose()
        {
            await DataStorage.ClearAsync();
            //DataStorage.Clear();
        }
    }
}
