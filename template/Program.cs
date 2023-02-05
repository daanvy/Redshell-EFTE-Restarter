using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedShelly
{
    internal class Program
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKeys);
        static void Main(string[] args)
        {
            bool instanceActive = false;

            Console.Title = $"Redshell Restarter - By Daanvy (Melon) [game: {(instanceActive == true ? "found" : "not found")}]";
            Console.BackgroundColor = ConsoleColor.Red;
            
            Thread checker = new Thread(CheckInstance) { IsBackground = true };
            bool debounce = false;
            Console.WriteLine($"Redshell EFTE Restarter v1");
            Console.WriteLine($"Made with <3 by Daanvy (twitch.tv/daanvy)\n");
            Console.WriteLine($"Press 0 (zero) to restart game\n");

            checker.Start();
            while (true)
            {
                if (GetAsyncKeyState(Keys.D0) < 0 && debounce == false)
                {
                    Console.WriteLine($"{GetAsyncKeyState(Keys.D0)} : Key pressed");
                    debounce = true;
                    // handle code here

                    if(instanceActive == true)
                    {
                        Console.WriteLine("Game loc maybe: " + GetExodus().MainModule.FileName);
                        string gamePath = GetExodus().MainModule.FileName;
                        KillExodus();
                        do
                        {
                            Console.WriteLine("Waiting for process to end...");
                        } while (GetExodus() != null);
                        Console.WriteLine("Starting game...");
                        StartExodus(gamePath);
                    }

                    // end
                    Thread.Sleep(1000);
                    debounce = false;
                }
            }

            void CheckInstance()
            {
                while (true)
                {
                    if (GetExodus() == null)
                    {
#if DEBUG
                        Console.WriteLine($"[DEBUG] instance inactive");
#endif
                        instanceActive = false;

                    }
                    else
                    {
#if DEBUG
                        Console.WriteLine($"[DEBUG] instance active");
#endif
                        instanceActive = true;
                    }
#if DEBUG
                    Console.WriteLine($"STATE: {instanceActive}");
#endif
                    Console.Title = $"Redshell Restarter - By Daanvy (Melon) [game: {(instanceActive == true ? "found" : "not found")}]";
                    Thread.Sleep(500);
                }

            }

            Process GetExodus()
            {
                if (Process.GetProcessesByName("efte").Length > 0)
                {
                    return Process.GetProcessesByName("efte")[0];
                }
                else
                {
                    return null;
                }
            }
            void KillExodus()
            {
                if (GetExodus() == null)
                {
                    Console.WriteLine("Tried to kill efte, but no instance found...");
                    return;
                }
                else
                {
                    Process.GetProcessesByName("efte")[0].Kill();
                }

            }
            void StartExodus(string path)
            {
                if (GetExodus() != null)
                {
                    Console.WriteLine("Tried to start efte, but at least 1 active instance was found...");
                    return;
                }
                else
                {
                    Process.Start(path);
                }
            }
        }
    }
}
