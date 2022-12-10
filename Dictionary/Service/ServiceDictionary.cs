using Dictionary.Models;
using Microsoft.AspNetCore.Routing.Tree;

namespace Dictionary.Service
{
    public class ServiceDictionary
    {
        public Trie root = new Trie(false);

        public void addWord(string words, string mean, string meanEng,  string example, string typeWord, string apiAudio)
        {
            CreateNode(words, mean, meanEng, example, typeWord, apiAudio, ref root);
        }
        public void CreateNode(string words, string mean, string meanEng, string example, string typeWord, string apiAudio, ref Trie node)
        {
           if(words.Length == 0)
           {
                node.setMeanEng(meanEng);
                node.setType(typeWord);
                node.setMean(mean);
                node.setExample(example);
                node.setCheck();
                return;
           }

           char x = words[0];
           if (node.checklist(x) == false)
           {
               Trie new_trie = new Trie(false);
               node.addList(x, new_trie);
           }
           Trie value = node.getValue(x);
           CreateNode(words.Substring(1), mean, meanEng, example, typeWord, apiAudio, ref value);
        }

        public Dictionary<string, object> Search(string word)
        {
            Trie temp = this.root;
            var results = new Dictionary<string, object>();
            var resultsReturn = new Dictionary<string, object>();
            for (int i=0; i<word.Length; i++)
            {
                if (temp.checklist(word[i]) == false)
                {
                    results.Add("checkSearch", false);
                    resultsReturn.Add("resultsSearch", results);
                    return resultsReturn;
                }
                temp = temp.getValue(word[i]);
            }
            if (temp.getCheck())
            {
                results.Add("checkSearch", true);
                results.Add("mean", temp.getMean());
                results.Add("meanEng", temp.getMeanEng());
                results.Add("example", temp.getExample());
                results.Add("type", temp.getType());
            }
            resultsReturn.Add("resultsSearch", results);
            return resultsReturn;
        }
    }
}
