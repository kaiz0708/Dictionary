using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dictionary.Models;
using Dictionary.Service;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Caching.Memory;
using Dictionary.Database;
using MySql.Data.MySqlClient;
using System;
using Microsoft.OpenApi.Validations;

namespace Dictionary.Controllers.Dictionary
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private IMemoryCache _cacheProvider;
        public DictionaryController(IMemoryCache memoryCache)
        {
            _cacheProvider = memoryCache;
        }

        IConfiguration config = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json")
                               .AddEnvironmentVariables()
                               .Build();
        [HttpGet("createDictionary")]

        public Dictionary<string, object> CreateDictionary(string lang)
        {
            ServiceDictionary createDic = new ServiceDictionary();
            ServiceListSuggest createList = new ServiceListSuggest();
            if (lang.Equals(config["LangEng"]))
            {
                createList.CreateListEng();
            }
            else
            {
                createList.CreateListViet();
            }
            var connectServer = new ConnectDB();
            MySqlConnection conn = connectServer.Connect();
            conn.Open();
            string sqlGetWord = $"Select * from WordDictionary";
            MySqlCommand getWord = new MySqlCommand(sqlGetWord, conn);
            MySqlDataReader reader = getWord.ExecuteReader();
            while (reader.Read())
            {
                var data = new DataReader(reader["word"].ToString(), reader["mean"].ToString(), reader["meanEng"].ToString(), reader["example"].ToString(), reader["typeWord"].ToString(), reader["lang"].ToString());
                if (lang.Equals(config["LangEng"]))
                {
                    if (data.getLang().Equals(config["LangEng"]))
                    {
                        createDic.addWord(data.getWord(), data.getMean(), data.getMeanEnglish(), data.getExample(), data.getType(), data.getLang());
                        createList.setWord(data.getWord());
                    }
                }
                else
                {
                    if (data.getLang().Equals(config["LangViet"]))
                    {
                        createDic.addWord(data.getWord(), data.getMean(), data.getMeanEnglish(), data.getExample(), data.getType(), data.getLang());
                        createList.setWord(data.getWord());
                    }
                }
            }
            conn.Close();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                   .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                   .SetPriority(CacheItemPriority.Normal)
                   .SetSize(10000);
            _cacheProvider.Set("dictionary", createDic, cacheEntryOptions);
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("createData", true);
            result.Add("ListSuggest", createList.ListOption());
            return result;
        }

        [HttpGet("search")]
        public Dictionary<string , object> searchWord(string word)
        {
            if(_cacheProvider.TryGetValue("dictionary", out ServiceDictionary dic))
            {
                var resultsReturn = dic.Search(word);
                return resultsReturn;
            }
            Dictionary<string, object> results = new Dictionary<string, object>();
            results.Add("result", false);
            return results;
        }

        [HttpPost("addWord")]

        public Dictionary<string, object> addNewWord([FromBody] dataRequest data)
        {
            var new_word = new dataRequest
            {
                word = data.word,
                mean = data.mean,
                meanEng = data.meanEng,
                example = data.example,
                typeWord = data.typeWord,
                lang = data.lang
            };  
            CheckWord checkWord = new CheckWord();
            Dictionary<string, object> results = new Dictionary<string, object>();
            string language = config["LangEng"];
            if (new_word.lang)
            {
                language = config["LangViet"];
            }
            var connect = new ConnectDB();
            string idWord = System.Guid.NewGuid().ToString();
            MySqlConnection conn = connect.Connect();
            conn.Open();
            string selectWordCheck = $"select * from WordDictionary where word='{new_word.word}'";
            MySqlCommand getWord = new MySqlCommand(selectWordCheck, conn);
            MySqlDataReader reader = getWord.ExecuteReader();
            while (reader.Read())
            {
                checkWord.Name = reader["word"].ToString();
            }
            conn.Close();
            if (checkWord.Name == null)
            {
                conn.Open();
                Console.WriteLine("lololololo");
                string insertWord = $"insert into WordDictionary(idWord, word, mean, meanEng, example, typeWord, lang)" +
                $"values('{idWord}', '{new_word.word}', '{new_word.mean}', '{new_word.meanEng}', '{new_word.example}', '{new_word.typeWord}', '{language}')";
                Console.WriteLine(insertWord);
                MySqlCommand insert = new MySqlCommand(insertWord, conn);
                insert.ExecuteNonQuery();
                conn.Close();
                results.Add("addWord", true);
                return results;
            }
            else
            {
                results.Add("addWord", false);
                return results;
            }
        }

        [HttpPost("updateWord")]

        public Dictionary<string , object> updateWord([FromBody] dataRequest data)
        {
            var new_update = new dataRequest
            {
                word = data.word,
                mean = data.mean,
                meanEng = data.meanEng,
                example = data.example,
                typeWord = data.typeWord,
                lang = data.lang
            };
            string language = config["LangEng"];
            Dictionary<string, object> results = new Dictionary<string, object>();
            if (new_update.lang)
            {
                language = config["LangViet"];
            }
            var connect = new ConnectDB();
            string idWord = System.Guid.NewGuid().ToString();
            MySqlConnection conn = connect.Connect();
            conn.Open();
            string updateWord = $"update WordDictionary set idWord='{idWord}', word='{new_update.word}', mean='{new_update.mean}'," +
                $"meanEng='{new_update.meanEng}', example='{new_update.example}', typeWord='{new_update.typeWord}', lang='{language}' where word='{new_update.word}'";
            MySqlCommand insertUpdate = new MySqlCommand(updateWord, conn);
            insertUpdate.ExecuteNonQuery();
            conn.Close();
            results.Add("update", true);
            return results;
        }
    }
}
