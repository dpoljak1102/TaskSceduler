using System;
using System.Threading.Tasks;
using TaskSceduler.App.Core;

namespace TaskSceduler.App.Models
{
    public class TaskModel:ObservableObject
    {

        private Guid _trackerId = Guid.NewGuid();
        public Guid TrackerId
        {
            get { return _trackerId; }
            set { _trackerId = value; OnPropertyChanged(); }
        }

        private string _subject = "";
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; OnPropertyChanged(); }
        }

        private string _projectType = "";
        public string ProjectType
        {
            get { return _projectType; }
            set { _projectType = value; OnPropertyChanged(); }
        }

        private string _status = "";
		public string Status
		{
			get { return _status; }
			set { _status = value; OnPropertyChanged(); }
		}

		private string _priority = "";
		public string Priority
        {
			get { return _priority; }
			set { _priority = value; OnPropertyChanged(); }
		}

		private DateTime _startDate = DateTime.Now;
		public DateTime StartDate
		{
			get { return _startDate;}
			set { _startDate = value; OnPropertyChanged();}
		}

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; OnPropertyChanged();}
        }

        private int _percentageDone;
        public int PercentageDone
        {
            get { return _percentageDone; }
            set
            {
                if (value < 0)
                    _percentageDone = 0;
                else if (value > 100)
                    _percentageDone = 100;
                else
                    _percentageDone = value;
                OnPropertyChanged();
            }
        }


        //Dummy task for later
        public async void StartUpdatingPercentage()
        {
            while (PercentageDone<100) 
            {
                UpdatePercentageDoneAsync();
                await Task.Delay(1000); 
            }
        }

        private void UpdatePercentageDoneAsync()
        {
            Random random = new Random();
            var randomPercentage = random.Next(5, 20);

            PercentageDone += randomPercentage;
         
            App.Current.Dispatcher.Invoke(() =>
            {
                OnPropertyChanged(nameof(PercentageDone));
            });
        }

    }
}
