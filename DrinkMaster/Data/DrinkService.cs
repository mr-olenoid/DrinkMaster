using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DrinkMaster.Models;

namespace DrinkMaster.Data
{
    public class DrinkService
    {
        public async Task<Drink> GetRandomBooze()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://www.thecocktaildb.com/api/json/v1/1/random.php");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Response code Ok");
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                //Get coctail name
                var strIngredientPattern = "strDrink\":\"(.*?)\",";
                Drink drink = new Drink(Regex.Matches(responseBody, strIngredientPattern, RegexOptions.Singleline)[0].Groups[1].Value);
                strIngredientPattern = "strDrinkThumb\":\"(.*?)\",";
                //Get instructions
                drink.ImageLink = Regex.Matches(responseBody, strIngredientPattern, RegexOptions.Singleline)[0].Groups[1].Value;
                strIngredientPattern = "strInstructions\":\"(.*?)\",";
                drink.Instruction = Regex.Matches(responseBody, strIngredientPattern, RegexOptions.Singleline)[0].Groups[1].Value;
                //Get imagge link
                List<Ingridient> ingridients = new List<Ingridient>();
                strIngredientPattern = "strIngredient[0-9]\":\"(.*?)\",";
                //Get ingridients
                foreach (Match match in Regex.Matches(responseBody, strIngredientPattern, RegexOptions.Singleline))
                {
                    var text = match.Groups[1].Value;
                    if (text != "")
                    {
                        ingridients.Add(new Ingridient(Name: text));
                    }
                }
                strIngredientPattern = "strMeasure[0-9]\":\"(.*?)\",";
                var i = 0;
                foreach (Match match in Regex.Matches(responseBody, strIngredientPattern, RegexOptions.Singleline))
                {
                    var text = match.Groups[1].Value;
                    ingridients[i].Ammount = text;
                    i++;
                    if (i >= ingridients.Count)
                    {
                        break;
                    }
                }
                drink.Ingridients = ingridients.ToArray();
                foreach (Ingridient ingr in ingridients)
                {
                    Console.WriteLine(ingr);
                }
                return drink;
            }

            Console.WriteLine("Done");
            return null;
        }

        public async Task<Drink[]> GetMoreBooze()
        {
            Drink[] drinks = new Drink[6];
            for (int i=0; i<drinks.Length; i++)
            {
                drinks[i] = await GetRandomBooze();
            }

            return drinks;
        }
    }
}
