namespace Dictionary.Service
{
    public class ServiceListSuggest
    {
        private Dictionary<char, List<string>> listSug = new Dictionary<char, List<string>>(30);

        public void CreateListEng()
        {
            for (int i = 97; i <= 122; i++)
            {
                List<string> listObj = new List<string>(1000);
                char x = (char)i;
                listObj.Add(x.ToString());
                this.listSug.Add(x, listObj);
            }
        }

        public void CreateListViet()
        {
            char[] arr1 = new char[] { 'á', 'à', 'ả', 'ã', 'ạ', 'â', 'ấ', 'ầ', 'ẩ', 'ẫ', 'ậ', 'ă', 'ắ', 'ằ', 'ẳ', 'ẵ', 'ặ',
                    'đ',
                    'ị',
                    'ý','ổ'
            };
            CreateListEng();
            for(int i=0; i<arr1.Length; i++)
            {
                List<string> listObj = new List<string>(1000);
                listObj.Add(arr1[i].ToString());
                this.listSug.Add(arr1[i], listObj);
            }
        }

        public Dictionary<char, List<string>> ListOption()
        {
            return this.listSug;
        }

        public void setWord(string word)
        {
            char valueFirst = word[0];
            List<string> new_list = this.listSug[valueFirst];
            new_list.Add(word);
            this.listSug[valueFirst] = new_list;
        }
    }
}
