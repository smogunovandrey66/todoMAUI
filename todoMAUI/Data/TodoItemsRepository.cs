using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todoMAUI.Models;

namespace todoMAUI.Data
{
    public class TodoItemsRepository : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ILogger _logger;

        private void InitDb()
        {
            _connection.Open();
            var createTableCmd = _connection.CreateCommand();
            createTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS todos(
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT,
                    isDone INTEGER DEFAULT 0,
                    createAt Text
                )
            ";
            createTableCmd.ExecuteNonQuery();
        }

        public TodoItemsRepository(ILogger<TodoItemsRepository> logger)
        {
            string path = FileSystem.AppDataDirectory;
            System.Diagnostics.Debug.WriteLine($"Windows prefs: {path}");
            _connection = new SqliteConnection(Constants.DatabasePath);
            InitDb();
            _logger = logger;
            _logger.LogDebug($"path from logger = {path}");
        }

        public void Dispose()
        {
            _logger.LogDebug("TodoItemsRepository.Dispose begin");
            _connection.Dispose();
            _logger.LogDebug("TodoItemsRepository.Dispose end");
        }

        public async void Save(TodoItem item)
        {
            try
            {
                using var savedCmd = _connection.CreateCommand();
                savedCmd.CommandText = @"
                    INSERT INTO todos(title, createAt) VALUES (@title, @createAt);
                    SELECT last_insert_rowid();
                ";

                savedCmd.Parameters.AddWithValue("@title", item.Title);
                var dateNow = DateTime.Now;
                savedCmd.Parameters.AddWithValue("@createAt", dateNow.ToString("yyyy-MM-ddTHH:mm:ss"));

                var result = await savedCmd.ExecuteScalarAsync();
                var id = Convert.ToInt32(result);

                item.CreateAt = dateNow;
                item.Id = id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка сохранения TodoItemsRepository.Save: {ex.Message}");
                //Можно throw
            }
        }

        public List<TodoItem> GetTodoItems()
        {
            var result = new List<TodoItem>();

            try
            {
                using var listCmd = _connection.CreateCommand();
                listCmd.CommandText = "SELECT id, title, isDone, createAt FROM todos";
                var reader = listCmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new TodoItem
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        IsDone = reader.GetBoolean(2),
                        CreateAt = reader.GetDateTime(3)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"TodoItemsRepository.GetTodoItems error: {ex.Message}");
                //throw;
            }

            return result;
        }

        public bool ToggleItem(TodoItem item)
        {
            var result = false;
            try
            {
                var toggleCmd = _connection.CreateCommand();
                toggleCmd.CommandText = "UPDATE todos SET isDone = iif(isDone = 1, 0, 1) WHERE id = @id";
                toggleCmd.Parameters.AddWithValue("@id", item.Id);
                toggleCmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"TodoItemsRepository.ToggleItem error: {ex.Message}");
                //throw;
            }

            return result;
        }

        public bool RemoveItem(TodoItem item)
        {
            var result = false;

            try
            {
                var removeCmd = _connection.CreateCommand();
                removeCmd.CommandText = "DELETE FROM todos WHERE id = @id";
                removeCmd.Parameters.AddWithValue("@id", item.Id);
                removeCmd.ExecuteNonQuery();

                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"TodoItemsRepository.RemoveItem error: {ex.Message}");
                //throw;
            }

            return result;
        }
    }
}
