namespace Coffee_Machine
{
    internal class Program
    {
        class CoffeeMachine
        {
            // Menu of drinks
            static Dictionary<string, Drink> MENU = new Dictionary<string, Drink>()
            {
                { "espresso", new Drink(new Dictionary<string, int> { { "water", 50 }, { "coffee", 18 } }, 1.5) },
                { "latte", new Drink(new Dictionary<string, int> { { "water", 200 }, { "milk", 150 }, { "coffee", 24 } }, 2.5) },
                { "cappuccino", new Drink(new Dictionary<string, int> { { "water", 250 }, { "milk", 100 }, { "coffee", 24 } }, 3.0) }
            };

                    static double profit = 0;
                    static Dictionary<string, int> resources = new Dictionary<string, int>()
            {
                { "water", 300 },
                { "milk", 200 },
                { "coffee", 100 }
            };

            // Checks if resources are sufficient to make the selected drink
            static bool IsResourceSufficient(Dictionary<string, int> orderIngredients)
            {
                foreach (var item in orderIngredients)
                {
                    if (item.Value > resources[item.Key])
                    {
                        Console.WriteLine($"Sorry, there is not enough {item.Key}.");
                        return false;
                    }
                }
                return true;
            }

            // Processes coins and calculates the total amount inserted
            static double ProcessCoins()
            {
                double total = 0;
                total += GetCoinInput("quarters", 0.25);
                total += GetCoinInput("dimes", 0.1);
                total += GetCoinInput("nickels", 0.05);
                total += GetCoinInput("pennies", 0.01);
                return total;
            }

            // Helper method for processing each type of coin
            static double GetCoinInput(string coinName, double value)
            {
                Console.Write($"How many {coinName}?: ");
                if (int.TryParse(Console.ReadLine(), out int count))
                {
                    return count * value;
                }
                Console.WriteLine("Invalid input. Assuming 0 coins.");
                return 0;
            }

            // Determines if the transaction is successful and gives change if necessary
            static bool IsTransactionSuccessful(double moneyReceived, double drinkCost)
            {
                if (moneyReceived >= drinkCost)
                {
                    double change = Math.Round(moneyReceived - drinkCost, 2);
                    if (change > 0) Console.WriteLine($"Here is ${change} in change.");
                    profit += drinkCost;
                    return true;
                }
                else
                {
                    Console.WriteLine("Sorry, that's not enough money. Money refunded.");
                    return false;
                }
            }

            // Makes the coffee by deducting the ingredients from available resources
            static void MakeCoffee(string drinkName, Dictionary<string, int> orderIngredients)
            {
                foreach (var item in orderIngredients)
                {
                    resources[item.Key] -= item.Value;
                }
                Console.WriteLine($"Here is your {drinkName} ☕️. Enjoy!");
            }

            // Displays a report of current resources and profit
            static void DisplayReport()
            {
                Console.WriteLine($"Water: {resources["water"]}ml");
                Console.WriteLine($"Milk: {resources["milk"]}ml");
                Console.WriteLine($"Coffee: {resources["coffee"]}g");
                Console.WriteLine($"Money: ${profit}");
            }

            static void Main(string[] args)
            {
                bool isOn = true;

                while (isOn)
                {
                    Console.Write("What would you like? (espresso/latte/cappuccino/report/off): ");
                    string choice = Console.ReadLine().ToLower();

                    switch (choice)
                    {
                        case "off":
                            isOn = false;
                            break;

                        case "report":
                            DisplayReport();
                            break;

                        case "espresso":
                        case "latte":
                        case "cappuccino":
                            ProcessOrder(choice);
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please choose from espresso, latte, cappuccino, report, or off.");
                            break;
                    }
                }
            }

            // Processes the selected drink order
            static void ProcessOrder(string drinkName)
            {
                Drink selectedDrink = MENU[drinkName];
                if (IsResourceSufficient(selectedDrink.Ingredients))
                {
                    double payment = ProcessCoins();
                    if (IsTransactionSuccessful(payment, selectedDrink.Cost))
                    {
                        MakeCoffee(drinkName, selectedDrink.Ingredients);
                    }
                }
            }
        }

        // Class to represent a drink
        class Drink
        {
            public Dictionary<string, int> Ingredients { get; private set; }
            public double Cost { get; private set; }

            public Drink(Dictionary<string, int> ingredients, double cost)
            {
                Ingredients = ingredients;
                Cost = cost;
            }
        }
    }
}

