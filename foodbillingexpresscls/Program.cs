using System;
using System.Collections.Generic;

namespace FoodBillingApplication
{
    class Program
    {
        static int orderNumber = 1; // Initialize the order number
        static void Main(string[] args)
        {
            string inputUsername;
            string inputPassword;

            do
            {
                Console.Write("Enter username: ");
                inputUsername = Console.ReadLine();

                if (string.IsNullOrEmpty(inputUsername))
                {
                    Console.WriteLine("Must enter a username. Please provide both username and password.");
                }
            } while (string.IsNullOrEmpty(inputUsername));

            do
            {
                Console.Write("Enter password: ");
                inputPassword = Console.ReadLine();

                if (string.IsNullOrEmpty(inputPassword))
                {
                    Console.WriteLine("Must enter a password. Please provide both username and password.");
                }
            } while (string.IsNullOrEmpty(inputPassword));

            // Path to the CSV file containing user data (Replace with your actual CSV file path)
            string csvFilePath = "C:\\path\\to\\validate.csv";

            if (AuthenticateUser(inputUsername, inputPassword, csvFilePath))
            {
                do
                {
                    FoodBilling.ProcessTiffinOrder(orderNumber); // Pass the order number
                    orderNumber++; // Increment the order number

                    Console.Write("Do you want to place another order? (yes/no): ");
                } while (Console.ReadLine().Trim().ToLower() == "yes");
            }
            else
            {
                Console.WriteLine("Validation failed. User does not exist or invalid credentials.");
            }
        }

        static bool AuthenticateUser(string username, string password, string csvFilePath)
        {
            // Implement user authentication logic here, e.g., by checking the CSV file
            // Return true if authentication is successful, otherwise return false
            // You need to implement this part
            return true; // Replace with your actual authentication logic
        }
    }

    class MenuItem
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }

    class FoodBilling
    {
        private static List<MenuItem> orderList = new List<MenuItem>();

        public static void ProcessTiffinOrder(int orderNumber) // Accept orderNumber as a parameter
        {
            orderList.Clear(); // Clear the order list at the beginning of each order

            string shopName = "Udupi's Tiffin Billing";
            string gstNo = "GST3AE8D8G9";
            string phoneNumber = "8754544556";
            string address = "HYD";
            DateTime now = DateTime.Now;

            Console.WriteLine("Food Billing Express!");
            Console.WriteLine($"Order Number: {orderNumber}"); // Display the order number

            while (true)
            {
                Console.WriteLine("Enter the item you want to order:");
                string orderName = Console.ReadLine();

                if (orderName.ToLower() == "finish")
                {
                    DisplayBill(shopName, gstNo, phoneNumber, address, orderNumber, now, orderList);
                    break;
                }

                if (string.IsNullOrEmpty(orderName))
                {
                    Console.WriteLine("Invalid item name. Please try again.");
                    continue;
                }

                Console.Write($"How many {orderName} Quantity ? ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 1)
                {
                    Console.WriteLine("Please enter a valid quantity.");
                    continue;
                }

                int cost = GetPrice(orderName);
                if (cost == 0)
                {
                    Console.WriteLine("Invalid item name. Please try again.");
                    continue;
                }

                MenuItem orderItem = new MenuItem
                {
                    ItemName = orderName,
                    Quantity = quantity
                };

                orderList.Add(orderItem);
            }
        }

        static void DisplayBill(string shopName, string gstNo, string phoneNumber, string address, int orderNumber, DateTime now, List<MenuItem> orders)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"SHOP Name: {shopName}");
            Console.WriteLine($"GST NO: {gstNo}");
            Console.WriteLine($"PHNO: {phoneNumber}");
            Console.WriteLine($"Address: {address}");
            Console.WriteLine($"Order No #: {orderNumber}");
            Console.WriteLine($"Date and Time: {now}");
            Console.WriteLine("----------------------------");
            Console.WriteLine("SNO    ItemName   Qty  Cost Price");
            Console.ReadLine();
            int sno = 1;
            int total = 0;

            foreach (var orderItem in orders)
            {
                int cost = GetPrice(orderItem.ItemName);
                int price = orderItem.Quantity * cost;
                Console.WriteLine($"{sno}\t{orderItem.ItemName}\t{orderItem.Quantity}\t{cost}\t{price}");
                total += price;
                sno++;
            }

            double cgst = total * 0.025;
            double sgst = total * 0.025;
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Total                        {total} Rs");
            Console.WriteLine($"CGST 2.5%                   {cgst}");
            Console.WriteLine($"SGST 2.5%                   {sgst}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"GRAND TOTAL                   {total + cgst + sgst}");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Thank you. Visit Again");
            Console.ReadLine();
        }

        static int GetPrice(string item)
        {
            switch (item.ToLower())
            {
                case "dosa":
                    return 35;
                case "puri":
                    return 45;
                case "vada":
                    return 30;
                case "idly":
                    return 25;
                default:
                    return 0; // Item not found
            }
        }
    }
}
