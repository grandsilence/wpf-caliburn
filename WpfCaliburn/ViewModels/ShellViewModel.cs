using System;
using System.Collections.ObjectModel;
using System.Windows;
using Caliburn.Micro;
using WpfCaliburn.Models;

namespace WpfCaliburn.ViewModels
{
    public class ShellViewModel : Screen
    {
        public string TitleX { get; set; } = "Privet";

        public ObservableCollection<TodoTask> Todos { get; set; }
        public TodoTask SelectedTodo { get; set; }

        public ShellViewModel()
        {
            Todos = new ObservableCollection<TodoTask> {
                new TodoTask {Title = "Task n1"},
                new TodoTask {Title = "Task n2"}
            };
        }

        public bool CanAddTodo(string titleX)
        {
            return !string.IsNullOrWhiteSpace(titleX);
        }

        public void AddTodo(string titleX)
        {
            Todos.Add(new TodoTask {
                Title = TitleX
            });
        }

        public void RemoveTodo(TodoTask todo)
        {
            Todos.Remove(todo);
        }
    }
}