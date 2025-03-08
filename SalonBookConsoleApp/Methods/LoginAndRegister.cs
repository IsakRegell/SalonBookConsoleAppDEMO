using Microsoft.Identity.Client;
using SalonBookConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonBookConsoleApp.Methods
{
    public class LoginAndRegister
    {
        CustomerProgram customerProgram = new CustomerProgram();
        public void Login(string userType)
        {
            CustomerProgram menus = new CustomerProgram();

            using (var db = new SalonBookContext())
            {
                Console.WriteLine("*Inloggning*\n");
                Console.Write("Ange din e-post: ");
                string email = Console.ReadLine()!;
                Console.Write("Ange ditt lösenord: ");
                string password = Console.ReadLine()!;

                if (userType == "customer")
                {
                    var customer = db.Customers.FirstOrDefault(c => c.Email == email && c.Password == password);
                    if (customer != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"Välkommen {customer.Name}, du är nu inloggad");
                        menus.CustomerMenu(customer);
                    }
                    else
                    {
                        Console.WriteLine("Fel e-post eller lösenord, försök igen!");
                    }
                }
                else if (userType == "admin")
                {
                    var admin = db.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);
                    if(admin != null)
                    {
                        Console.WriteLine($"Välkommen {admin.Name}, du är nu inloggad som *ADMIN*");
                    }
                    else
                    {
                        Console.WriteLine("Fel e-post eller lösenord, försök igen!");
                    }
                }

            }
        }
        public void RegisterCustomer()
        {
            using var db = new SalonBookContext();
            {
                Console.WriteLine("*Regestrering*\n");
                Console.Write("Ange ditt namn: ");
                string registerName = Console.ReadLine()!;
                Console.Write("\nAnge din e-post: ");
                string registerEmail = Console.ReadLine()!;
                Console.Write("\nAnge ett lösenord: ");
                string registerPassword = Console.ReadLine()!;

                var customer = new Customer { Name = registerName, Email = registerEmail, Password = registerPassword };
                db.Customers.Add(customer);
                db.SaveChanges();

                Console.Clear() ;
                Console.WriteLine("Ny kund registrerad!");
                Console.WriteLine("\nTryck på valfri tangent för att logga in dig...");
                Console.ReadKey(); 

                // 🔹 Logga in användaren direkt
                Customer loggedInCustomer = customer;

                // Starta kundmenyn direkt
                customerProgram.CustomerMenu(loggedInCustomer); 
            }
        }
    }
}
