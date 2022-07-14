using Microsoft.Data.Sqlite;
using System.IO;

namespace Logging.Abramchik
{

    public class Program
    {

        static void Main(string[] args)
        {           
            // Создаем новый экземпляр класса и указываем путь где будут созданы файл/база данных
            // и куда будут сохраняться соответствующие логи.  
            Logger log = new Logger("C:\\FileLog.txt", "C:\\DBLog.db");

            // Примеры
            log.Debug(Source.Console, "Запись лога в консоль");
            log.Error(Source.File, "Запись лога в файл");
            log.Info(Source.Database, "Запись лога в базу данных");
            log.Warn(Source.Database, "Еще одна запись лога в базу данных");

        }

    }
}