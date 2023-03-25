using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaTelefonica
{
    internal class PhoneBook
    {
        public string NameContact { get; set; }
        public string NumberPhone { get; set; }
        public string DayBirt { get; set; }
        public string MonthBirt { get; set; }
        public string YearBirt { get; set; }

        public PhoneBook(string name, string phone, string day, string month, string year) 
        {
            NameContact = name;
            NumberPhone = phone;
            DayBirt = day;
            MonthBirt = month;
            YearBirt = year;
        }

        public int CheckNameList (string name)
        {
            int cont = 0;
            if(this.NameContact == name)
            {
                cont = 1;
            }
            return cont;
        }
        public override string ToString()
        {
            return $"Nome: {NameContact} - Número: {NumberPhone} - Data Nasc.: {DayBirt}/{MonthBirt}/{YearBirt}";
        }

        public string GravarArquiSemNum()
        {
            return $"{NameContact}" + ";" + $"{NumberPhone}" +";"+ $"{DayBirt}" + ";" + $"{MonthBirt}" + ";" + $"{YearBirt}";
        }
    }
}
