using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using System.IO;
using KooshDaroo.Models;

namespace KooshDaroo.Data
{
    class KooshDarooDatabase
    {
        //Define SQLLite Database
        readonly SQLiteAsyncConnection database;
        private string databaseFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KooshDaroo.db3");

        public KooshDarooDatabase()
        {
            //CreateDatabaseAndTables();
            database = new SQLiteAsyncConnection(databaseFilePath);
            database.CreateTableAsync<tblPharmacy>().Wait();
            database.CreateTableAsync<tblCity>().Wait();
        }
        public KooshDarooDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<tblPharmacy>().Wait();
        }
        public Task<List<tblPharmacy>> GetPharmacysAsync()
        {
            return database.Table<tblPharmacy>().ToListAsync();
        }
        public Task<tblPharmacy> GetPharmacyAsync(string phoneno)
        {
            return database.Table<tblPharmacy>().Where(i => i.PhoneNo == phoneno).FirstOrDefaultAsync();
        }
        public Task<int> SavePharmacyAsync(tblPharmacy item)
        {
            if (item.id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
        public Task<int> DeletePharmacyAsync(tblPharmacy item)
        {
            return database.DeleteAsync(item);
        }


        public Task<List<tblCity>> GetCitysAsync()
        {
            return database.Table<tblCity>().ToListAsync();
        }
        public Task<tblCity> GetCityAsync(string cityName,string stateName)
        {
            return database.Table<tblCity>().Where(i => i.CityName == cityName & i.StateName == stateName).FirstOrDefaultAsync();
        }
        public Task<int> SaveCityAsync(tblCity item)
        {
            if (item.id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
        public Task<int> DeleteCityAsync(tblCity item)
        {
            return database.DeleteAsync(item);
        }
    }
}
