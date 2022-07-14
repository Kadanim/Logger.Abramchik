using Microsoft.Data.Sqlite;

namespace Logging.Abramchik
{
    public class Logger : ILogger
    {
        private readonly string filePath;
        private readonly string dbPath;

        public Logger(string filePath, string dbPath)
        {
            this.filePath = filePath;
            this.dbPath = dbPath;
        }

        /// <summary>
        /// Метод, к котому обращается пользователь для записи лога уровня debug
        /// </summary>
        /// <param name="source">Source.Console - для записи в консоль. 
        /// Source.File - для записи в файл. Source.Database - для записи в базу данных.</param>
        /// <param name="text">Сообщение лога</param>
        public void Debug(Source source, string text)
        {
            var prefix = "debug";
            Log(source, prefix, text);
        }
        /// <summary>
        /// Метод, к котому обращается пользователь для записи лога уровня error
        /// </summary>
        /// <param name="source">Source.Console - для записи в консоль. 
        /// Source.File - для записи в файл. Source.Database - для записи в базу данных.</param>
        /// <param name="text">Сообщение лога</param>
        public void Error(Source source, string text)
        {
            var prefix = "error";
            Log(source, prefix, text);
        }
        /// <summary>
        /// Метод, к котому обращается пользователь для записи лога уровня info
        /// </summary>
        /// <param name="source">Source.Console - для записи в консоль. 
        /// Source.File - для записи в файл. Source.Database - для записи в базу данных.</param>
        /// <param name="text">Сообщение лога</param>
        public void Info(Source source, string text)
        {
            var prefix = "info";
            Log(source, prefix, text);
        }
        /// <summary>
        /// Метод, к котому обращается пользователь для записи лога уровня warn
        /// </summary>
        /// <param name="source">Source.Console - для записи в консоль. 
        /// Source.File - для записи в файл. Source.Database - для записи в базу данных.</param>
        /// <param name="text">Сообщение лога</param>
        public void Warn(Source source, string text)
        {
            var prefix = "warn";
            Log(source, prefix, text);
        }
        // Метод, который определяет куда будет записываться лог от переданного Source,
        // а также добавляет пробел между префиксом (уровнем лога) и текстом для консоли и файла. 
        private void Log(Source source, string prefix, string text)
        {
            if (source == Source.Console)
            {
                LogToConsole(prefix + " " + text);
            }
            else if (source == Source.File)
            {
                LogToFile(prefix + " " + text);
            }
            else if (source == Source.Database)
            {
                LogToDatabase(prefix, text);
            }
            else
            {
                throw new ArgumentException("Choose right source");
            }
        }
        // Метод реализуюзий запись в консоль
        private void LogToConsole(string text)
        {
            Console.WriteLine(text);
        }
        // Метод реализующий запись в файл. Если файла нет - создает новый.
        private void LogToFile(string text)
        {
            string appendText = text + Environment.NewLine;
            File.AppendAllText(filePath, appendText);
        }
        // Метод реализуюзий запись в базу данных (использую Sqlite). Если базы данных нет - создает новую.
        private void LogToDatabase(string prefix, string text)
        {
            var isDatabaseExist = File.Exists(dbPath);
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {

                connection.Open();
                // Проверяет есть ли уже созданная база данных. 
                if (!isDatabaseExist)
                {
                    CreateLogsTable(connection);
                }
                //Сохраняет лог в базу. 
                InsertLog(connection, prefix, text);

            }
        }
        // Метод реализующий создание базы данных.
        private void CreateLogsTable(SqliteConnection connection)
        {
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText =
            @$"
                      CREATE TABLE logs
                        (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            type TEXT,
                            text TEXT
                        );
             ";
            createTableCommand.ExecuteNonQuery();
        }
        // Метот реализующий запись в базу данных. 
        private void InsertLog(SqliteConnection connection, string prefix, string text)
        {
            var insertLogCommand = connection.CreateCommand();
            insertLogCommand.CommandText =
                @$"
                       INSERT INTO Logs (type, text)
                       VALUES ('{prefix}', '{text}');
                 ";
            insertLogCommand.ExecuteNonQuery();
        }
    }
}

