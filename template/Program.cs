using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WindowWidth = 70;
            Console.WindowHeight = 40;
            
            Thread checker = new Thread(CheckInstance) { IsBackground = true };
            bool debounce = false;
            void clr()
            {
                Console.Clear();
            }

            void set()
            {
                clr();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Redshell EFTE Restarter v1");
                Console.WriteLine($"Made with <3 by Daanvy (twitch.tv/daanvy)\n");
                Console.WriteLine($"Press 0 (zero) to restart game\n");
            }
            set();
            checker.Start();
            while (true)
            {
                if (GetAsyncKeyState(Keys.D0) < 0 && debounce == false)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Key pressed : {GetAsyncKeyState(Keys.D0)}");
                    debounce = true;
                    // handle code here

                    if(instanceActive == true)
                    {
                        string gamePath = GetExodus().MainModule.FileName;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("proc exec loc: " + gamePath);
                        KillExodus();
                        do
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Waiting for process to end...");
                            Thread.Sleep(500);
                        } while (GetExodus() != null);
                        Console.ForegroundColor = ConsoleColor.Green;
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
                        instanceActive = false;

                    }
                    else
                    {
                        instanceActive = true;
                    }
                    Console.Title = $"Redshell Restarter - By Daanvy (Melon) [game: {(instanceActive == true ? "found" : "not found")}]";
                    Thread.Sleep(500);
                }

            }

            Process GetExodus()
            {
                //Console.WriteLine("Length: " + Process.GetProcessesByName("efte").Length);
                //foreach(var proc in Process.GetProcessesByName("efte"))
                //{
                //    Console.WriteLine($": {proc.ProcessName} | {proc.Id}");
                //    return null;
                //}
                //return null;
                var procs = Process.GetProcessesByName("efte");
                if (procs.Length > 0)
                {
                    return procs[0];
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
                    Console.ForegroundColor = ConsoleColor.Red;
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
                    Console.ForegroundColor = ConsoleColor.Red;
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
