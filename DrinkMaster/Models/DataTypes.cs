using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkMaster.Models
{
    public record Ingridient(string Name, string Ammount = "")
    {
        public string Ammount { get; set; }
    }
    public record Drink(string Name, string ImageLink = "", string Instruction = "", Ingridient[] Ingridients = null)
    {
        public string ImageLink { get; set; }
        public string Instruction { get; set; }
        public Ingridient[] Ingridients { get; set; }
    }
}
