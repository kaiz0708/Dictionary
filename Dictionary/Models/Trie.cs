namespace Dictionary.Models
{
    public class Trie
    {
        private string mean;
        private string meanEng;
        private string typeWord;
        private string example;
        private string audio;
        private bool check;
        private Dictionary<char, Trie> list;
        public Trie(bool value)
        {
            check = value;
            list = new Dictionary<char, Trie>();
        }

        public void setAudio(string _audio)
        {
            this.audio = _audio;
        }

        public void setMeanEng(string _meanEng)
        {
            this.meanEng = _meanEng;
        }
        public void setMean(string _mean)
        {
            this.mean = _mean;
        }

        public void setExample(string _example)
        {
            this.example = _example;
        }

        public void setType(string _type)
        {
            this.typeWord = _type;
        }

        public string getMeanEng()
        {
            return this.meanEng;
        }

        public string getExample()
        {
            return this.example;
        }

        public string getType()
        {
            return this.typeWord;
        }
        public string getMean()
        {
            return this.mean;
        }

        public string getAudio()
        {
            return this.audio;
        }


        public void setCheck()
        {
            check = true;
        }

        public bool getCheck()
        {
            return check;
        }
        public void addList(char a, Trie b)
        {
            this.list.Add(a, b);
        }

        public bool checklist(char a)
        {
            if (this.list.ContainsKey(a))
            {
                return true;
            }
            return false;
        }

        public Trie getValue(char a)
        {
            return this.list[a];
        }
    }
}
