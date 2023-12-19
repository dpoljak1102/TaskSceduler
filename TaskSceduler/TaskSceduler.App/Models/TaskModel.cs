using System;
using TaskSceduler.App.Core;

namespace TaskSceduler.App.Models
{
    public class TaskModel:ObservableObject
    {

        private int _trackerId;
        public int TrackerId
        {
            get { return _trackerId; }
            set { _trackerId = value; OnPropertyChanged(); }
        }

        private string _projectType;
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

		private DateTime _startDate;
		public DateTime StartDate
		{
			get { return _startDate;}
			set { _startDate = value; OnPropertyChanged();}
		}

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; OnPropertyChanged(); }
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

    }
}
