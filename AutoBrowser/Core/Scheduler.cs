using Microsoft.Win32.TaskScheduler;
using System;
using System.Windows.Forms;

namespace AutoBrowser.Core
{
    public class Scheduler
    {
        public enum Frecuency
        {
            OneTime = 0,
            EachHour = 1,
            Diary = 2
        }

        public void AddTask(string name, string file, Frecuency frecuency)
        {
            // Get the service on the local machine
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = name;

                // Create a trigger that will fire the task at this time every other day
                switch (frecuency)
                {
                    case Frecuency.OneTime:
                        td.Triggers.Add(new TimeTrigger
                        {
                            StartBoundary = DateTime.Now
                        });
                        break;
                    case Frecuency.EachHour:
                        td.Triggers.Add(new TimeTrigger
                        {
                            StartBoundary = DateTime.Now,
                            Repetition = new RepetitionPattern(
                                new TimeSpan(1, 0, 0),
                                new TimeSpan())
                        });
                        break;
                    case Frecuency.Diary:
                        td.Triggers.Add(new DailyTrigger
                        {
                            DaysInterval = 1
                        });
                        break;
                }

                // Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(Application.ExecutablePath, file));

                // Register the task in the root folder
                ts.RootFolder.RegisterTaskDefinition(name, td);

                // Remove the task we just created
                //ts.RootFolder.DeleteTask(name);
            }
        }
    }
}
