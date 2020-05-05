using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimeEditParser.Models
{
    class CheckedListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string title;
        private bool isChecked;

        public string Title {
            get
            {
                return title;
            }
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            } 
        }
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
    }
}
