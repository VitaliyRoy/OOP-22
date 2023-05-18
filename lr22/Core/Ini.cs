using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;


//██████╗░░█████╗░███╗░░░███╗░█████╗░███╗░░██╗  ░█████╗░██████╗░██╗░░░██╗███╗░░██╗███████╗████████╗░██████╗
//██╔══██╗██╔══██╗████╗░████║██╔══██╗████╗░██║  ██╔══██╗██╔══██╗╚██╗░██╔╝████╗░██║██╔════╝╚══██╔══╝██╔════╝
//██████╔╝██║░░██║██╔████╔██║███████║██╔██╗██║  ██║░░██║██║░░██║░╚████╔╝░██╔██╗██║█████╗░░░░░██║░░░╚█████╗░
//██╔══██╗██║░░██║██║╚██╔╝██║██╔══██║██║╚████║  ██║░░██║██║░░██║░░╚██╔╝░░██║╚████║██╔══╝░░░░░██║░░░░╚═══██╗
//██║░░██║╚█████╔╝██║░╚═╝░██║██║░░██║██║░╚███║  ╚█████╔╝██████╔╝░░░██║░░░██║░╚███║███████╗░░░██║░░░██████╔╝
//╚═╝░░╚═╝░╚════╝░╚═╝░░░░░╚═╝╚═╝░░╚═╝╚═╝░░╚══╝  ░╚════╝░╚═════╝░░░░╚═╝░░░╚═╝░░╚══╝╚══════╝░░░╚═╝░░░╚═════╝░

namespace lr22.Core
{
    /// <summary>
    /// Отвечает за работу с файлом конфигурации
    /// </summary>
    public class Ini
    {
        private readonly string _path;
        private readonly string _exe = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string @default, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Коструктор конфигурационного файла
        /// </summary>
        /// <param name="iniPath">Путь к файлу</param>
        public Ini(string iniPath = null)
        {
            _path = new FileInfo(iniPath ?? _exe + ".ini").FullName.ToString();
        }

        /// <summary>
        /// Чтение записи с конфигурации
        /// </summary>
        /// <param name="key">Ключ записи</param>
        /// <param name="section">Секция записи</param>
        /// <returns>Зачение записи</returns>
        public string read(string key, string section = null)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section ?? _exe, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        /// <summary>
        /// Запись в конфигурацию
        /// </summary>
        /// <param name="key">Ключ записи</param>
        /// <param name="value">Значение записи</param>
        /// <param name="section">Секция записи</param>
        public void write(string key, string value, string section = null)
        {
            WritePrivateProfileString(section ?? _exe, key, value, _path);
        }

        /// <summary>
        /// Удаление ключа с конфигурации
        /// </summary>
        /// <param name="key">Ключ записи</param>
        /// <param name="section">Секция записи</param>
        public void deleteKey(string key, string section = null)
        {
            write(key, null, section ?? _exe);
        }

        /// <summary>
        /// Удаление целой секции с конфигурации
        /// </summary>
        /// <param name="section">Секция</param>
        public void deleteSection(string section = null)
        {
            write(null, null, section ?? _exe);
        }

        /// <summary>
        /// Проверка на существование ключа в конфигурации
        /// </summary>
        /// <param name="key">Ключ записи</param>
        /// <param name="section">Секция записи</param>
        /// <returns>Булевый результат</returns>
        public bool keyExists(string key, string section = null)
        {
            return read(key, section).Length > 0;
        }
    }
}