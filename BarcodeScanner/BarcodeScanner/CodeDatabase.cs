using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace BarcodeScanner
{
    public class CodeDatabase
    {
        private static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        private static SQLiteAsyncConnection Database => lazyInitializer.Value;

        private static bool initialized = false;

        public CodeDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        private async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Barcode).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Barcode)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }
        public Task<List<Barcode>> GetItemsAsync()
        {
            return Database.Table<Barcode>().OrderByDescending(x => x.Time).ToListAsync();
        }

        public Task<List<Barcode>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<Barcode>("SELECT * FROM [Barcode] WHERE [Done] = 0");
        }

        public Task<Barcode> GetItemAsync(int id)
        {
            return Database.Table<Barcode>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<Barcode> GetItemByCodeAsync(string code)
        {
            return Database.Table<Barcode>().Where(i => i.Text == code).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Barcode item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Barcode item)
        {
            return Database.DeleteAsync(item);
        }
    }
    public static class TaskExtensions
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
