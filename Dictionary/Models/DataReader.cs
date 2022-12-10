namespace Dictionary.Models
{
    public class DataReader
    {
        private string word;
        private string mean;
        private string meanEng;
        private string example;
        private string typeWord;
        private string lang;

        public DataReader(string word, string mean, string meanEng, string example, string typeWord, string audio)
        {
            this.word = word;
            this.mean = mean;
            this.meanEng = meanEng;
            this.example = example;
            this.typeWord = typeWord;
            this.lang = audio;
        }

        public string getLang()
        {
            return this.lang;
        }

        public string getWord()
        {
            return this.word;
        }

        public string getMean()
        {
            return this.mean;
        }

        public string getMeanEnglish()
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
    }

    
}
