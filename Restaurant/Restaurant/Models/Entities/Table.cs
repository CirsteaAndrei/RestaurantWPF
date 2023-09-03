using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Entities
{
    public class Table : BaseEntity, INotifyPropertyChanged
    {
        public int SeatsTotal { get; set; }
        private int seatsTaken { get; set; }
        public int? EmployeeId { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        private Employee? employee { get; set; }


        public Employee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        public int SeatsTaken
        {
            get
            {
                return seatsTaken;
            }
            set
            {
                seatsTaken = value;
                OnPropertyChanged("SeatsTaken");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
